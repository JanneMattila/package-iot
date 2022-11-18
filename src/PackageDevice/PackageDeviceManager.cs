using Microsoft.Azure.Devices.Client;
using PackageDevice.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PackageDevice
{
    public class PackageDeviceManager
    {
        private readonly DeviceClient _deviceClient;
        private readonly HttpClient _httpClient = new HttpClient();
        private bool _isLocalOnly;

        public PackageDeviceManager(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                _isLocalOnly = true;
            }
            else
            {
                _deviceClient = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Mqtt);
            }
        }

        public async Task SendMovementAsync(PackageDeviceMovement movement, bool isMoving)
        {
            var json = JsonSerializer.Serialize(movement);
            Console.WriteLine(json);

            if (_isLocalOnly)
            {
                var response = await _httpClient.PutAsJsonAsync<PackageDeviceMovement>("https://localhost:44370/api/packages/1", movement);
                response.EnsureSuccessStatusCode();
            }
            else
            {
                var message = new Message(Encoding.UTF8.GetBytes(json));
                message.Properties.Add("isMoving", isMoving.ToString());

                await _deviceClient.SendEventAsync(message);
            }
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
            movement.Start.Timestamp = DateTimeOffset.UtcNow;

            movement.End.Latitude = end.Latitude;
            movement.End.Longitude = end.Longitude;
            movement.End.Timestamp = DateTimeOffset.UtcNow.AddSeconds((arrivalTime - departureTime).TotalSeconds);

            Console.WriteLine($"Started package delivery:");
            var movementSteps = new List<MovementStep>();
            foreach (var route in routeData.Routes)
            {
                foreach (var legs in route.Legs)
                {
                    var stepTime = (legs.Summary.ArrivalTime - legs.Summary.DepartureTime) / legs.Points.Count;
                    foreach (var point in legs.Points)
                    {
                        var step = new MovementStep();
                        step.Location.Latitude = point.Latitude;
                        step.Location.Longitude = point.Longitude;
                        step.TravelTimeInMilliseconds = stepTime.TotalMilliseconds;
                        movementSteps.Add(step);
                    }
                }
            }

            for (int i = 0; i < movementSteps.Count - 1; i++)
            {
                var current = movementSteps[i];
                var next = movementSteps[i + 1];

                movement.Current.Latitude = current.Location.Latitude;
                movement.Current.Longitude = current.Location.Longitude;
                movement.Current.Timestamp = DateTimeOffset.UtcNow;

                movement.Next.Latitude = next.Location.Latitude;
                movement.Next.Longitude = next.Location.Longitude;
                movement.Next.Timestamp = DateTimeOffset.UtcNow.AddMilliseconds(current.TravelTimeInMilliseconds);

                await SendMovementAsync(movement, isMoving: true);

                var timespan = TimeSpan.FromMilliseconds(current.TravelTimeInMilliseconds);
                await Task.Delay(timespan);
            }

            await SendMovementAsync(movement, isMoving: false);
            Console.WriteLine($"Package delivered!");
        }
    }
}
