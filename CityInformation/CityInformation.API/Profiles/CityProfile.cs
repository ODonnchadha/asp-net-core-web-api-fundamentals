using AutoMapper;

namespace CityInformation.API.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<Entities.City, DTOs.CityWithoutPointsOfInterest>();
            CreateMap<Entities.City, DTOs.City>();
        }
    }
}
