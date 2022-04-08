namespace CityInformation.API.DTOs
{
    public class PointOfInterest
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string? Description { get; set; }
    }
}
