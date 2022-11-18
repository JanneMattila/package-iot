using System.Text.Json.Serialization;

namespace PackageDevice.Interfaces;

public class PackageDeviceMovement
{
    [JsonPropertyName("start")]
    public Location Start { get; set; }

    [JsonPropertyName("current")]
    public Location Current { get; set; }

    [JsonPropertyName("next")]
    public Location Next { get; set; }

    [JsonPropertyName("end")]
    public Location End { get; set; }

    public PackageDeviceMovement()
    {
        Start = new Location();
        Current = new Location();
        Next = new Location();
        End = new Location();
    }
}
