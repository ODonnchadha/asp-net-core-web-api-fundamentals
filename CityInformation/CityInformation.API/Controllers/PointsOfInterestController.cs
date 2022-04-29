using AutoMapper;
using CityInformation.API.DTOs;
using CityInformation.API.Interfaces.Repositories;
using CityInformation.API.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInformation.API.Controllers
{
    [ApiController(), Authorize(), Route("api/cities/{cityId}/pointsofinterest")]
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
                var cityName = User.Claims.FirstOrDefault(c => c.Type == "city")?.Value;
                if (!await _repository.CityNameMatchesCityIdAsync(cityName, cityId))
                {
                    _logger.LogInformation(
                        $"City {cityId} does not match city name {cityName} when attempting to access GetPointsOfInterest.");

                    return Forbid();
                }

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
        public async Task<ActionResult> DeletePointOfInterest(int cityId, int interestId)
        {
            if (!await _repository.CityExistsAsync(cityId))
            {
                _logger.LogInformation(
                    $"City {cityId} was not found when attempting to DeletePointOfInterest.");
                return NotFound();
            }

            var point = await _repository.GetPointOfInterestAsync(cityId, interestId);
            if (point == null)
            {
                return NotFound();
            }

            _repository.DeletePointOfInterest(point);

            await _repository.SaveChangesAsync();

            _service.Send($"{point.Name}", $"Point {point.Name} was deleted.");

            return NoContent();
        }

        [HttpPatch("{interestId}")]
        public async Task<ActionResult<PointOfInterest>> PartiallyUpdatePointOfInterest(
            int cityId, int interestId, JsonPatchDocument<PointOfInterestForUpdate> patchDocument)
        {
            if (!await _repository.CityExistsAsync(cityId))
            {
                _logger.LogInformation(
                    $"City {cityId} was not found when attempting to PartiallyUpdatePointOfInterest.");
                return NotFound();
            }

            var entity = await _repository.GetPointOfInterestAsync(cityId, interestId);
            if (entity == null)
            {
                return NotFound();
            }

            // Map entity to a DTO due to JsonPatchDocument<T>.
            var patch = _mapper.Map<PointOfInterestForUpdate>(entity);

            // Validation.
            patchDocument.ApplyTo(patch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!TryValidateModel(patch))
            {
                return BadRequest(ModelState);
            }

            // Map patch to entity to preserve changes.
            _mapper.Map(patch, entity);

            await _repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost()]
        public async Task<ActionResult<PointOfInterest>> CreatePointOfInterest(
            int cityId, PointOfInterestForCreation dto)
        {
            if (!await _repository.CityExistsAsync(cityId))
            {
                _logger.LogInformation(
                    $"City {cityId} was not found when attempting to CreatePointOfInterest.");
                return NotFound();
            }

            var p = _mapper.Map<Entities.PointOfInterest>(dto);

            await _repository.AddPointOfInterestForCityAsync(cityId, p);
            await _repository.SaveChangesAsync();

            var point = _mapper.Map<DTOs.PointOfInterest>(p);

            return CreatedAtRoute("GetPointOfInterest", 
                new { cityId = cityId, interestId = point.Id }, point);
        }

        [HttpPut("{interestId}")]
        public async Task<ActionResult<PointOfInterest>> UpdatePointOfInterest(
            int cityId, int interestId, PointOfInterestForUpdate dto)
        {
            if (!await _repository.CityExistsAsync(cityId))
            {
                _logger.LogInformation(
                    $"City {cityId} was not found when attempting to UpdatePointOfInterest.");
                return NotFound();
            }

            var point = await _repository.GetPointOfInterestAsync(cityId, interestId);
            if (point == null)
            {
                _logger.LogInformation(
                    $"Point {interestId} was not found when attempting to UpdatePointOfInterest.");
                return NotFound();
            }

            _mapper.Map(dto, point);

            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
