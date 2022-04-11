using CityInformation.API.Interfaces.Services;

namespace CityInformation.API.Services
{
    public class CloudMailService : IMailService
    {
        public string To { get; private set; } = String.Empty;
        public string From { get; private set; } = String.Empty;

        public CloudMailService(IConfiguration configuration)
        {
            To = configuration["MailService:To"];
            From = configuration["MailService:From"];
        }

        public void Send(string subject, string message)
        {
            Console.WriteLine($"Mail from {From} to {To} with {nameof(CloudMailService)}:");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
