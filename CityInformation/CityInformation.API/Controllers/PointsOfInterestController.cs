using AutoMapper;
using CityInformation.API.DataStores;
using CityInformation.API.DTOs;
using CityInformation.API.Interfaces.Repositories;
using CityInformation.API.Interfaces.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInformation.API.Controllers
{
    [ApiController(), Route("api/cities/{cityId}/pointsofinterest")]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ICityInformationRepository _repository;
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _service;
        private readonly IMapper _mapper;
        public PointsOfInterestController(
            ICityInformationRepository repository,
            ILogger<PointsOfInterestController> logger, IMailService service, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
            
        [HttpGet("{interestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterest>> GetPointOfInterest(
            int cityId, int interestId)
        {
            var exists = await _repository.CityExistsAsync(cityId);

            if (exists.Equals(false))
            {
                _logger.LogInformation(
                    $"City {cityId} was not found when attempting to access GetPointOfInterest.");

                return NotFound();
            }

            var pointOfInterest = await _repository.GetPointOfInterestAsync(cityId, interestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<PointOfInterest>(pointOfInterest);

            return Ok(result);
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<PointOfInterest>>> GetPointsOfInterest(int cityId)
        {
            try
            {
                var exists = await _repository.CityExistsAsync(cityId);
                
                if (exists.Equals(false))
                {
                    _logger.LogInformation(
                        $"City {cityId} was not found when attempting to access GetPointsOfInterest.");

                    return NotFound();
                }

                var points = await _repository.GetPointsOfInterestAsync(cityId);

                var result = _mapper.Map<IEnumerable<PointOfInterest>>(points);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    $"City {cityId} assisted in a critical GetPointsOfInterest error.", ex);

                return StatusCode(500, "Keyboard not detected. Press F2 to continue.");
            }
        }

        [HttpDelete("{interestId}")]
        public ActionResult DeletePointOfInterest(int cityId, int interestId)
        {
            var city = CityDataStore.Instance.Cities.FirstOrDefault(
                c => c.Id == cityId);

            var point = city?.PointsOfInterest?.FirstOrDefault(p => p.Id == interestId);

            if (city == null || point == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(point);

            _service.Send($"PoI for {city.Name}.", $"PoI {point.Name} was deleted.");

            return NoContent();
        }

        [HttpPatch("{interestId}")]
        public ActionResult<PointOfInterest> PartiallyUpdatePointOfInterest(
            int cityId, int interestId, JsonPatchDocument<PointOfInterestForUpdate> patchDocument)
        {
            var city = CityDataStore.Instance.Cities.FirstOrDefault(
                c => c.Id == cityId);

            var point = city?.PointsOfInterest?.FirstOrDefault(p => p.Id == interestId);

            if (city == null || point == null)
            {
                return NotFound();
            }

            var patch = new PointOfInterestForUpdate { Name = point.Name, Description = point.Description };

            patchDocument.ApplyTo(patch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!TryValidateModel(patch))
            {
                return BadRequest(ModelState);
            }

            point.Name = patch.Name;
            point.Description = patch.Description;

            return NoContent();
        }

        [HttpPost()]
        public async Task<ActionResult<PointOfInterest>> CreatePointOfInterest(
            int cityId, PointOfInterestForCreation dto)
        {
            //var city = CityDataStore.Instance.Cities.FirstOrDefault(
            //    c => c.Id == cityId);
            if (!await _repository.CityExistsAsync(cityId))
            {
                _logger.LogInformation(
                    $"City {cityId} was not found when attempting to CreatePointOfInterest.");

                return NotFound();
            }
            //if (city == null)
            //{
            //    return NotFound();
            //}

            //var max = CityDataStore.Instance.Cities.SelectMany(
            //    c => c.PointsOfInterest).Max(p => p.Id);

            //var p = new PointOfInterest { Id = ++max, 
            //    Name = dto.Name, Description = dto.Description };
            var p = _mapper.Map<Entities.PointOfInterest>(dto);

            //city.PointsOfInterest.Add(p);
            await _repository.AddPointOfInterestForCityAsync(cityId, p);
            await _repository.SaveChangesAsync();

            var point = _mapper.Map<DTOs.PointOfInterest>(p);

            return CreatedAtRoute("GetPointOfInterest", 
                new { cityId = cityId, interestId = point.Id }, point);
        }

        [HttpPut("{interestId}")]
        public ActionResult<PointOfInterest> UpdatePointOfInterest(
            int cityId, int interestId, PointOfInterestForUpdate dto)
        {
            var city = CityDataStore.Instance.Cities.FirstOrDefault(
                c => c.Id == cityId);

            var point = city?.PointsOfInterest?.FirstOrDefault(p => p.Id == interestId);

            if (city == null || point == null)
            {
                return NotFound();
            }

            point.Name = dto.Name;
            point.Description = dto.Description;

            return NoContent();
        }
    }
}
