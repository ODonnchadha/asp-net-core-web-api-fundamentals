namespace CityInformation.API.DTOs
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string? Description { get; set; }
        public int NmberOfPointsOfInterest { get { return PointsOfInterest.Count; } }
        public ICollection<PointOfInterest> PointsOfInterest { get; set; } 
            = new List<PointOfInterest>();
    }
}
