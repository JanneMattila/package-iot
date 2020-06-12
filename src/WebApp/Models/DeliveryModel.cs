using System.Text.Json.Serialization;

namespace WebApp.Models
{
    public class DeliveryModel
    {
        [JsonPropertyName("request")]
        public string Request { get; internal set; } = string.Empty;
    }
}
