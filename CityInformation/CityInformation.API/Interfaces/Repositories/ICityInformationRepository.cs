using CityInformation.API.Entities;
using CityInformation.API.Models;

namespace CityInformation.API.Interfaces.Repositories
{
    /// <summary>
    /// IEnumerable versus Task<IQueryable<City>> GetCitiesAsync();
    /// </summary>
    public interface ICityInformationRepository
    {
        void DeletePointOfInterest(PointOfInterest point);
        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest point);
        Task<bool> CityExistsAsync(int cityId);
        Task<bool> CityNameMatchesCityIdAsync(string? cityName, int cityId);
        Task<bool> SaveChangesAsync();
        Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(
            string? name, string? searchQuery, int pageNumber, int pageSize);
        Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int interestId);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId);
    }
}
