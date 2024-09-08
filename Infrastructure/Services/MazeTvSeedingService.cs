using Domain.Models;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Infrastructure.Services
{
    public class MazeTvSeedingService(HttpClient httpClient, IOptions<SeedingOptions> options) : ISeedingService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly SeedingOptions _options = options.Value;

        private readonly DateTime _premieredFrom = new(2024, 01, 01);

        public async IAsyncEnumerable<Show> GetShowsAsync()
        {
            var index = 0;
            while (true)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"/{_options.ShowsEndpoint}?page={index}");
                var response = await _httpClient.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    await Task.Delay(4000);
                    response = await _httpClient.SendAsync(request);
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    break;

                await foreach (var item in GetShowsFromResponseAsync(response))
                    yield return item;

                index++;
            }
        }

        private async IAsyncEnumerable<Show> GetShowsFromResponseAsync(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            await foreach (var item in response.Content.ReadFromJsonAsAsyncEnumerable<Show>())
            {
                if (item is not null && item.Premiered >= _premieredFrom)
                {
                    yield return item;
                }
            }
        }
    }
}
