using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PackageDevice.Interfaces
{
    public partial class RouteData
    {
        [JsonPropertyName("formatVersion")]
        public string FormatVersion { get; set; }

        [JsonPropertyName("routes")]
        public List<Route> Routes { get; set; }
    }

    public partial class Route
    {
        [JsonPropertyName("summary")]
        public Summary Summary { get; set; }

        [JsonPropertyName("legs")]
        public List<Leg> Legs { get; set; }

        [JsonPropertyName("sections")]
        public List<Section> Sections { get; set; }
    }

    public partial class Leg
    {
        [JsonPropertyName("summary")]
        public Summary Summary { get; set; }

        [JsonPropertyName("points")]
        public List<Point> Points { get; set; }
    }

    public partial class Point
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }

    public partial class Summary
    {
        [JsonPropertyName("lengthInMeters")]
        public long LengthInMeters { get; set; }

        [JsonPropertyName("travelTimeInSeconds")]
        public long TravelTimeInSeconds { get; set; }

        [JsonPropertyName("trafficDelayInSeconds")]
        public long TrafficDelayInSeconds { get; set; }

        [JsonPropertyName("departureTime")]
        public DateTimeOffset DepartureTime { get; set; }

        [JsonPropertyName("arrivalTime")]
        public DateTimeOffset ArrivalTime { get; set; }
    }

    public partial class Section
    {
        [JsonPropertyName("startPointIndex")]
        public long StartPointIndex { get; set; }

        [JsonPropertyName("endPointIndex")]
        public long EndPointIndex { get; set; }

        [JsonPropertyName("sectionType")]
        public string SectionType { get; set; }

        [JsonPropertyName("travelMode")]
        public string TravelMode { get; set; }
    }
}
