using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Profiteering.Response;

internal class Item
{
    [JsonPropertyName("listings")]
    internal required List<Listing> Listings { get; set; }
}
