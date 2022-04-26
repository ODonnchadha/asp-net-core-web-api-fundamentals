namespace CityInformation.API.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string City { get; private set; }

        public User(int id, string name, string first, string last, string city)
        {
            Id = id;
            Name = name;
            FirstName = first;
            LastName = last;
            City = city;
        }
    }
}
