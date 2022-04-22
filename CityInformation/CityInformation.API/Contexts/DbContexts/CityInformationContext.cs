using CityInformation.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInformation.API.Contexts.DbContexts
{
    public class CityInformationContext : DbContext
    {
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;
        public CityInformationContext(DbContextOptions<CityInformationContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<City>().HasData(
                new City("Duluth")
                {
                    Id = 1,
                    Description = "For a cheapish haircut in a communist-free haven."
                },
                new City("Superior")
                {
                    Id=2,
                    Description = "Next to Duluth, we're Superior."
                });
            builder.Entity<PointOfInterest>().HasData(
                new PointOfInterest("Aerial Lift Bridge")
                {
                    Id = 1,
                    CityId = 1,
                    Description = "A rather uncommon vertical lift bridge."
                },
                new PointOfInterest("Enger Tower")
                {
                    Id = 2,
                    CityId = 1,
                    Description = "A five-story pavilion featuring broken glass and the smell of urine."
                },
                new PointOfInterest("Anchor Bar & Grill")
                {
                    Id = 3,
                    CityId = 2,
                    Description = "As featured in Diners, Drive-Ins and Dives."
                });

            base.OnModelCreating(builder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder builder)
        //{
        //    builder.UseSqlite("ConnectionString");
        //    base.OnConfiguring(builder);
        //}
    }
}
