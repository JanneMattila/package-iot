using Microsoft.Azure.Devices.Client;
using PackageDevice.Interfaces;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PackageDevice
{
    public class PackageDeviceManager
    {
        private readonly DeviceClient _deviceClient;

        public PackageDeviceManager(string connectionString)
        {
            _deviceClient = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Mqtt);
        }

        public async Task SendD2CAsync()
        {
            var movement = new PackageDeviceMovement();
            var json = JsonSerializer.Serialize(movement);
            
            var message = new Message(Encoding.UTF8.GetBytes(json));
            message.Properties.Add("isMoving", "true");

            await _deviceClient.SendEventAsync(message);
        }
    }
}
