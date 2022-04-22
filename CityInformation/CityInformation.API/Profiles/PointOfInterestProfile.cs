using AutoMapper;

namespace CityInformation.API.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<Entities.PointOfInterest, DTOs.PointOfInterest>();
            CreateMap<DTOs.PointOfInterestForCreation, Entities.PointOfInterest>();
        }
    }
}
