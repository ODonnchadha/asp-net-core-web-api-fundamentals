namespace CityInformation.API.DTOs
{
    public class CityWithoutPointsOfInterest
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string? Description { get; set; }
    }
}
