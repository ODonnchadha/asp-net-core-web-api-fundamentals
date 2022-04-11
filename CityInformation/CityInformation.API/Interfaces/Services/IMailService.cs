namespace CityInformation.API.Interfaces.Services
{
    public interface IMailService
    {
        void Send(string subject, string message);
    }
}
