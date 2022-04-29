using AutoMapper;
using CityInformation.API.DTOs;
using CityInformation.API.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CityInformation.API.Controllers
{
    [ApiController(), ApiVersion("1.0"), Authorize(Policy="MustBeFormerDuluthMayorJohnFedo"), Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        const int MAX_PAGE_SIZE = 20;

        private readonly ICityInformationRepository _repository;
        private readonly IMapper _mapper;
        public CitiesController(ICityInformationRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterest>>> GetCities(
            [FromQuery]string? name, [FromQuery]string? searchQuery,
            [FromQuery]int pageNumber = 1, [FromQuery]int pageSize = 10)
        {
            // With filter, ensure nullable fields: e.g.: string? name.

            if (pageSize > MAX_PAGE_SIZE) pageSize = MAX_PAGE_SIZE;

            var (cities, meta) = await _repository.GetCitiesAsync(
                name, searchQuery, pageNumber, pageSize);

            var results = _mapper.Map<IEnumerable<CityWithoutPointsOfInterest>>(cities);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(meta));

            return Ok(results);
        }

        /// <summary>
        /// Obtain a beautiful City by Id.
        /// </summary>
        /// <param name="id">The Id of the City to obtain.</param>
        /// <param name="includePointsOfInterest">Shall we include Points of Interest in the City Response?</param>
        /// <returns></returns>
        /// <response code="200">Successfully return of a beautiful city.</response>
        [HttpGet("{id}"), 
            ProducesResponseType(StatusCodes.Status200OK), 
            ProducesResponseType(StatusCodes.Status400BadRequest),
            ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = await _repository.GetCityAsync(id, includePointsOfInterest);

            if (city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                return Ok(_mapper.Map<City>(city));
            }

            return Ok(_mapper.Map<CityWithoutPointsOfInterest>(city));
        }
    }
}
