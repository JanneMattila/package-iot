using Microsoft.Azure.Devices.Client;
using PackageDevice.Interfaces;
using System;
using System.Linq;
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

        public async Task SendMovementD2CAsync(PackageDeviceMovement movement, bool isMoving)
        {
            Print(movement, isMoving);
            var json = JsonSerializer.Serialize(movement);
            var message = new Message(Encoding.UTF8.GetBytes(json));
            message.Properties.Add("isMoving", isMoving.ToString());

            await _deviceClient.SendEventAsync(message);
        }

        private void Print(PackageDeviceMovement movement, bool isMoving)
        {
            var status = isMoving ? "package is moving" : "package has arrived to destination";
            Console.WriteLine($"{movement.Current} - {status}");
        }

        public async Task StartRouteAsync(RouteData routeData)
        {
            var departureTime = routeData.Routes.First().Summary.DepartureTime;
            var arrivalTime = routeData.Routes.Last().Summary.ArrivalTime;

            var start = routeData.Routes.First().Legs.First().Points.First();
            var end = routeData.Routes.Last().Legs.Last().Points.Last();

            var movement = new PackageDeviceMovement();
            movement.Start.Latitude = start.Latitude;
            movement.Start.Longitude = start.Longitude;
            movement.Start.Timestamp = departureTime;

            movement.End.Latitude = end.Latitude;
            movement.End.Longitude = end.Longitude;
            movement.End.Timestamp = arrivalTime;

            Console.WriteLine($"Started package delivery:");
            Console.WriteLine($" from {movement.Start}");
            Console.WriteLine($" to {movement.End}");
            Console.WriteLine($" with {routeData.Routes.Count} routes");
            foreach (var route in routeData.Routes)
            {
                Console.WriteLine($"Route length: {route.Summary.LengthInMeters} in meters");
                Console.WriteLine($"Route travel time: {route.Summary.TravelTimeInSeconds} in seconds");
                Console.WriteLine($"Route legs: {route.Legs.Count}");
                foreach (var legs in route.Legs)
                {
                    Console.WriteLine($"Leg length: {legs.Summary.LengthInMeters} in meters");
                    Console.WriteLine($"Leg travel time: {legs.Summary.TravelTimeInSeconds} in seconds");
                    Console.WriteLine($"Leg points: {legs.Points.Count}");
                    foreach (var point in legs.Points)
                    {
                        movement.Current.Latitude = point.Latitude;
                        movement.Current.Longitude = point.Longitude;
                        movement.Current.Timestamp = DateTimeOffset.UtcNow;

                        await SendMovementD2CAsync(movement, isMoving: true);
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                }
            }

            await SendMovementD2CAsync(movement, isMoving: false);
            Console.WriteLine($"Package delivered!");
        }
    }
}
