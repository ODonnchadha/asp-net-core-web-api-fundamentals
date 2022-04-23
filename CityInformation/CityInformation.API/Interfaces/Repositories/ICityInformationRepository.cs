using CityInformation.API.Entities;

namespace CityInformation.API.Interfaces.Repositories
{
    /// <summary>
    /// Task<IQueryable<City>> GetCitiesAsync();
    /// </summary>
    public interface ICityInformationRepository
    {
        void DeletePointOfInterest(PointOfInterest point);
        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest point);
        Task<bool> CityExistsAsync(int cityId);
        Task<bool> SaveChangesAsync();
        Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int interestId);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId);
    }
}
