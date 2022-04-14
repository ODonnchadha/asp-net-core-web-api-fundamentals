using CityInformation.API.DTOs;

namespace CityInformation.API.DataStores
{
    public class CityDataStore
    {
        public static CityDataStore Instance { get; } = new();
        public List<City> Cities { get; set; }
        public CityDataStore()
        {
            Cities = new List<City>()
            {
                new City
                {
                    Id = 1,
                    Name = "Duluth",
                    Description = "For a cheapish haircut in a communist-free haven.",
                    PointsOfInterest = new List<PointOfInterest>
                    { 
                        new PointOfInterest 
                        { 
                            Id = 1,
                            Name = "Aerial Lift Bridge",
                            Description = "A rather uncommon vertical lift bridge."
                        },
                        new PointOfInterest
                        {
                            Id = 2,
                            Name = "Enger Tower",
                            Description = "A pavilion featuring broken glass everywhere along with the smell of urine."
                        },
                    }
                },
                new City
                {
                    Id = 2,
                    Name = "Superior",
                    Description = "Next to Duluth, we're Superior.",
                    PointsOfInterest = new List<PointOfInterest>
                    {
                        new PointOfInterest
                        {
                            Id = 1,
                            Name = "Anchor Bar & Grill",
                            Description = "As featured in Diners, Drive-Ins and Dives."
                        }
                    }
                },
            };
        }
    }
}
