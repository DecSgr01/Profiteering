using System.Text.Json.Serialization;

namespace Profiteering.Response;

internal class Listing
{
    [JsonPropertyName("pricePerUnit")]
    internal int PricePerUnit { get; set; }
    [JsonPropertyName("worldName")]
    internal required string WorldName { get; set; }
}
