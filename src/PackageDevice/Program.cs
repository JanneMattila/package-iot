using Microsoft.Extensions.Configuration;
using System;
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

            var connectionString = configuration.GetValue<string>("ConnectionString");

            var packageDeviceManager = new PackageDeviceManager(connectionString);
            await packageDeviceManager.SendD2CAsync();
        }
    }
}
