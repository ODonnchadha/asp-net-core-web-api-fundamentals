using CityInformation.API.Entities;

namespace CityInformation.API.Interfaces.Repositories
{
    public interface ICityInformationRepository
    {
        // Task<IQueryable<City>> GetCitiesAsync();
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int interestId);
    }
}
