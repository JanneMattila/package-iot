using Microsoft.Extensions.Configuration;
using PackageDevice.Interfaces;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PackageDevice
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Package Device started");

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

#if DEBUG
            builder.AddUserSecrets<Program>();
#endif

            var configuration = builder.Build();

            var iotHubConnectionString = configuration.GetValue<string>("IoTHubConnectionString");
            var packageDeviceManager = new PackageDeviceManager(iotHubConnectionString);
            await packageDeviceManager.SendD2CAsync();

            var azureMapsSubscriptionKey = configuration.GetValue<string>("AzureMapsSubscriptionKey");
            var routeFrom = configuration.GetValue<string>("RouteFrom");
            var routeTo = configuration.GetValue<string>("RouteTo");
            var query = $"{routeFrom}:{routeTo}";
            var requestUri = $"https://atlas.microsoft.com/route/directions/json?api-version=1.0&query={query}&language=en-US&subscription-key={azureMapsSubscriptionKey}";

            using var client = new HttpClient();
            var json = await client.GetStringAsync(requestUri);
            var routeData = JsonSerializer.Deserialize<RouteData>(json);

            packageDeviceManager.StartRoute(routeData);
        }
    }
}
