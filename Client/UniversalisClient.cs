using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Profiteering.Response;

namespace Profiteering.Client;
internal static class UniversalisClient
{
    internal static async Task<MarketDataResponse> GetCurrentDataAsync(int[] itemId, string worldName, CancellationToken cancellationToken, int historyCount = 0)
    {
        var uriBuilder = new UriBuilder($"https://universalis.app/api/v2/{worldName}/{String.Join(",", itemId)}?listings=+10&entries=0&fields=Citems.listings.worldName%2Citems.listings.pricePerUnit");

        cancellationToken.ThrowIfCancellationRequested();

        using var client = new HttpClient();
        var res = await client
          .GetStreamAsync(uriBuilder.Uri, cancellationToken)
          .ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        MarketDataResponse marketDataResponse = await JsonSerializer
                  .DeserializeAsync<MarketDataResponse>(res, cancellationToken: cancellationToken)
                  .ConfigureAwait(false);

        return marketDataResponse;
    }
}