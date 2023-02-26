using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Profiteering.Response;

internal class MarketDataResponse
{
    [JsonPropertyName("items")]
    public Dictionary<int, Item> items { get; set; } = new Dictionary<int, Item>();
}
