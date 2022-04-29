using CityInformation.API.Contexts.DbContexts;
using CityInformation.API.Entities;
using CityInformation.API.Interfaces.Repositories;
using CityInformation.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CityInformation.API.Repositories
{
    public class CityInformationRepository : ICityInformationRepository
    {
        private readonly CityInformationContext _context;
        public CityInformationRepository(CityInformationContext context) =>
            _context = context ?? throw new ArgumentNullException(nameof(context));
        public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest point)
        {
            var city = await GetCityAsync(cityId, false);

            if (city != null)
            {
                city.PointsOfInterest.Add(point);
            }
        }
        public async Task<bool> CityExistsAsync(int cityId) =>
            await _context.Cities.AnyAsync(c => c.Id == cityId);

        public async Task<bool> CityNameMatchesCityIdAsync(string? cityName, int cityId) =>
            await _context.Cities.AnyAsync(c => c.Id == cityId && c.Name == cityName);

        public void DeletePointOfInterest(PointOfInterest point) => _context.PointsOfInterest.Remove(point);
        public async Task<IEnumerable<City>> GetCitiesAsync() => 
            await _context.Cities.OrderBy(c => c.Name).ToListAsync();

        /// <summary>
        /// Tuple.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="searchQuery"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(
            string? name, string? searchQuery, int pageNumber, int pageSize)
        {
            // Obtain collection, via deferred execution, from which to work:
            var collection = _context.Cities as IQueryable<City>;

            if (!string.IsNullOrWhiteSpace(name))
            {
                collection = collection.Where(c => c.Name == name.Trim());
            }
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(c => c.Name.Contains(searchQuery) ||
                    (c.Description != null && c.Description.Contains(searchQuery)));
            }

            var count = await collection.CountAsync();

            var meta = new PaginationMetadata(count, pageSize, pageNumber);

            var result = await collection.OrderBy(
                c => c.Name).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync();

            return(result, meta);
        }
        public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return await _context.Cities.Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == cityId).FirstOrDefaultAsync(); 
            }
            else
            {
                return await _context.Cities.Where(c => c.Id == cityId).FirstOrDefaultAsync();
            }
        }
        public async Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int interestId) =>
            await _context.PointsOfInterest
                .Where(p => p.CityId == cityId && p.Id == interestId).FirstOrDefaultAsync();
        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestAsync(int cityId) =>
            await _context.PointsOfInterest.Where(p => p.CityId == cityId).ToListAsync();
        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() >= 0;
    }
}
