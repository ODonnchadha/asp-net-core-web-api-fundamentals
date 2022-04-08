using CityInformation.API.DataStores;
using CityInformation.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CityInformation.API.Controllers
{
    [ApiController(), Route("api/cities/{cityId}/pointsofinterest")]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet()]
        public ActionResult<IEnumerable<PointOfInterest>> GetPointsOfInterest(int cityId)
        {
            var city = CityDataStore.Instance.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{interestId}")]
        public ActionResult<PointOfInterest> GetPointOfInterest(int cityId, int interestId)
        {
            var city = CityDataStore.Instance.Cities.FirstOrDefault(c => c.Id == cityId);
            var pointOfInterest = city?.PointsOfInterest?.FirstOrDefault(p => p.Id == interestId);

            if (city == null || pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterest);
        }
    }
}
