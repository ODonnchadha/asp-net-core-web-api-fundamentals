using CityInformation.API.DataStores;
using CityInformation.API.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInformation.API.Controllers
{
    [ApiController(), Route("api/cities/{cityId}/pointsofinterest")]
    public class PointsOfInterestController : ControllerBase
    {
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
            var city = CityDataStore.Instance.Cities.FirstOrDefault(
                c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city.PointsOfInterest);
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
