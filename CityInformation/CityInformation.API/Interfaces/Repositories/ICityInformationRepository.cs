using CityInformation.API.Entities;

namespace CityInformation.API.Interfaces.Repositories
{
    /// <summary>
    /// Task<IQueryable<City>> GetCitiesAsync();
    /// </summary>
    public interface ICityInformationRepository
    {
        Task<bool> CityExistsAsync(int cityId);
        Task<bool> SaveChangesAsync();
        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest point);
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int interestId);
    }
}
