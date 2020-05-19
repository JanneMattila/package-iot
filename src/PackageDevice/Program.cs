using System;
using System.Threading.Tasks;

namespace PackageDevice
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Package Device started");

            var packageDeviceManager = new PackageDeviceManager("");
            await packageDeviceManager.SendD2CAsync();
        }
    }
}
