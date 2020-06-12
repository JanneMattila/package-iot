using System;
using System.Text.Json.Serialization;

namespace PackageDevice.Interfaces
{
    public class Location
    {
        [JsonPropertyName("lat")]
        public double Latitude { get; set; }

        [JsonPropertyName("lon")]
        public double Longitude { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        public override string ToString()
        {
            return $"Latitude: {Latitude}, Longitude: {Longitude}, Timestamp: {Timestamp.ToLocalTime()}";
        }
    }
}
