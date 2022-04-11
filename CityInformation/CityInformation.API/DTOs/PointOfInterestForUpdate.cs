using System.ComponentModel.DataAnnotations;

namespace CityInformation.API.DTOs
{
    public class PointOfInterestForUpdate
    {
        [Required(ErrorMessage = "public string Name is required."), MaxLength(50)]
        public string Name { get; set; } = String.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
