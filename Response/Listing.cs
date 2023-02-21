using System.Text.Json.Serialization;

namespace Profiteering.Response;

internal class Listing
{
    [JsonPropertyName("pricePerUnit")]
    public int pricePerUnit { get; set; }
    [JsonPropertyName("worldName")]
    public string WorldName { get; set; }
}
