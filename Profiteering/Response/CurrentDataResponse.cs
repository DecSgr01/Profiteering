using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Profiteering.Response;

internal class MarketDataResponse
{
    [JsonPropertyName("items")]
    internal Dictionary<int, Item> Items { get; set; } = new();
}
