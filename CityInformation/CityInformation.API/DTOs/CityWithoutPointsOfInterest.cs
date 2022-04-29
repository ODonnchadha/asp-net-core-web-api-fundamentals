namespace CityInformation.API.DTOs
{
    /// <summary>
    /// A City that does not contain any associated Points of Interest.
    /// </summary>
    public class CityWithoutPointsOfInterest
    {
        /// <summary>
        /// City Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// City Name.
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// City Description.
        /// </summary>
        public string? Description { get; set; }
    }
}
