﻿using AutoMapper;
using CityInformation.API.DTOs;
using CityInformation.API.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CityInformation.API.Controllers
{
    [ApiController(), Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInformationRepository _repository;
        private readonly IMapper _mapper;
        public CitiesController(ICityInformationRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// With filter, ensure nullable name: string? name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterest>>> GetCities(
            [FromQuery]string? name, [FromQuery]string? searchQuery)
        {
            var cities = await _repository.GetCitiesAsync(name, searchQuery);

            var results = _mapper.Map<IEnumerable<CityWithoutPointsOfInterest>>(cities);

            return Ok(results);
        }

        [HttpGet("{id}")]
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
