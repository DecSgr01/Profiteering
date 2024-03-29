using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Dalamud.Logging;
using Profiteering.Response;

namespace Profiteering.Client;
internal static class UniversalisClient
{
    internal static async Task<MarketDataResponse> GetMaterialsPriceAsync(int[] itemId, string worldName, int historyCount = 0)
    {
        var uriBuilder = new UriBuilder($"https://universalis.app/api/v2/{worldName}/{String.Join(",", itemId)}?listings=+10&entries=0&fields=items.listings.worldName%2Citems.listings.pricePerUnit");
        CancellationToken none = CancellationToken.None;
        none.ThrowIfCancellationRequested();

        using var client = new HttpClient();
        var res = await client
          .GetStreamAsync(uriBuilder.Uri, none)
          .ConfigureAwait(false);

        none.ThrowIfCancellationRequested();

        MarketDataResponse marketDataResponse = await JsonSerializer
                  .DeserializeAsync<MarketDataResponse>(res, cancellationToken: none)
                  .ConfigureAwait(false);

        return marketDataResponse;
    }

    internal static async Task<Item> GetRecipePriceAsync(int itemId, string worldName, bool isHq, int historyCount = 0)
    {
        var uriBuilder = new UriBuilder($"https://universalis.app/api/v2/{worldName}/{itemId}?listings=1&entries=0&noGst=true&hq={isHq}&fields=listings.pricePerUnit");
        CancellationToken none = CancellationToken.None;
        none.ThrowIfCancellationRequested();
        using var client = new HttpClient();
        var res = await client
          .GetStreamAsync(uriBuilder.Uri, none)
          .ConfigureAwait(false);
        none.ThrowIfCancellationRequested();

        Item item = await JsonSerializer
                  .DeserializeAsync<Item>(res, cancellationToken: none)
                  .ConfigureAwait(false);

        return item;
    }
}