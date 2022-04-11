using CityInformation.API.DataStores;
using CityInformation.API.DTOs;
using CityInformation.API.Interfaces.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInformation.API.Controllers
{
    [ApiController(), Route("api/cities/{cityId}/pointsofinterest")]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _service;
        public PointsOfInterestController(
            ILogger<PointsOfInterestController> logger, IMailService service)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }
            
        [HttpGet("{interestId}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterest> GetPointOfInterest(
            int cityId, int interestId)
        {
            var city = CityDataStore.Instance.Cities.FirstOrDefault(
                c => c.Id == cityId);
            var pointOfInterest = city?.PointsOfInterest?.FirstOrDefault(
                p => p.Id == interestId);

            if (city == null || pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterest);
        }

        [HttpGet()]
        public ActionResult<IEnumerable<PointOfInterest>> GetPointsOfInterest(
            int cityId)
        {
            try
            {
                var city = CityDataStore.Instance.Cities.FirstOrDefault(
                    c => c.Id == cityId);

                if (city == null)
                {
                    _logger.LogInformation(
                        $"City {cityId} was not found when attempting to access GetPointsOfInterest.");

                    return NotFound();
                }

                return Ok(city.PointsOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    $"City {cityId} resulted in a critical GetPointsOfInterest error.", ex);

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
        public ActionResult<PointOfInterest> CreatePointOfInterest(
            int cityId, PointOfInterestForCreation dto)
        {
            var city = CityDataStore.Instance.Cities.FirstOrDefault(
                c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            // Demo Only.
            var max = CityDataStore.Instance.Cities.SelectMany(
                c => c.PointsOfInterest).Max(p => p.Id);

            var p = new PointOfInterest { Id = ++max, 
                Name = dto.Name, Description = dto.Description };

            city.PointsOfInterest.Add(p);

            return CreatedAtRoute("GetPointOfInterest", 
                new { cityId = cityId, interestId = p.Id }, p);
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
