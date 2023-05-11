using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Profiteering.Response;

internal class Item
{
    [JsonPropertyName("listings")]
    public List<Listing> listings { get; set; }
}
