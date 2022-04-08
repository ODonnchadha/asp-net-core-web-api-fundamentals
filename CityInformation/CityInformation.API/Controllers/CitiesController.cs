using CityInformation.API.DataStores;
using CityInformation.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CityInformation.API.Controllers
{
    [ApiController(), Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        [HttpGet("{id}")]
        public ActionResult<City> GetCity(int id)
        {
            var city = CityDataStore.Instance.Cities
                .FirstOrDefault(c => c.Id == id);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }

        [HttpGet()]
        public ActionResult<IEnumerable<City>> GetCities() => Ok(CityDataStore.Instance.Cities);


        //[HttpGet()]
        //public JsonResult GetCities()
        //{
        //    //return new JsonResult(
        //    //    new List<object>
        //    //    {
        //    //        new { id = 1, Name = "Duluth" },
        //    //        new { id = 2, Name = "Superior" },
        //    //    });

        //    var json = new JsonResult(CityDataStore.Instance.Cities);
        //    json.StatusCode = 200;

        //    return json;
        //}
    }
}
