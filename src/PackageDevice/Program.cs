using Microsoft.Extensions.Configuration;
using PackageDevice.Interfaces;
using System;
using System.IO;
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

            var path = Path.Combine(AppContext.BaseDirectory, "configs");
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddKeyPerFile(directoryPath: path, optional: true)
#if DEBUG
                .AddUserSecrets<Program>()
#endif
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            var iotHubConnectionString = configuration.GetValue<string>("IoTHubConnectionString") ?? string.Empty;
            var packageDeviceManager = new PackageDeviceManager(iotHubConnectionString);

            var azureMapsSubscriptionKey = configuration.GetValue<string>("AzureMapsSubscriptionKey");
            var routeFrom = configuration.GetValue<string>("RouteFrom");
            var routeTo = configuration.GetValue<string>("RouteTo");

            RouteData routeData;
            if (string.IsNullOrEmpty(routeFrom) && string.IsNullOrEmpty(routeFrom))
            {
                var exampleRoutes = Directory.GetFiles("ExampleRoutes");
                var json = File.ReadAllText(exampleRoutes[0]);
                routeData = JsonSerializer.Deserialize<RouteData>(json);
            }
            else
            {
                var query = $"{routeFrom}:{routeTo}";
                var requestUri = $"https://atlas.microsoft.com/route/directions/json?api-version=1.0&query={query}&language=en-US&subscription-key={azureMapsSubscriptionKey}";

                using var client = new HttpClient();
                var json = await client.GetStringAsync(requestUri);
                routeData = JsonSerializer.Deserialize<RouteData>(json);
            }

            await packageDeviceManager.StartRouteAsync(routeData);
        }
    }
}
