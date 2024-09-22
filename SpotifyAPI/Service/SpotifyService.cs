using SpotifyAPI.Models;
using SpotifyAPI.Service.IService;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SpotifyAPI.Service
{
    public class SpotifyService : ISpotifyService
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public SpotifyService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IEnumerable<Release>> GetNewReleases(string countryCode, int limit, string accessToken)
        {
            var client = _httpClientFactory.CreateClient("Spotify");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync($"browse/new-releases?country={countryCode}&limit={limit}");
            //var response = await client.GetAsync($"browse/new-releases"); 
            response.EnsureSuccessStatusCode(); 
            using  var respStream = await response.Content.ReadAsStreamAsync();
            var responseObj = await JsonSerializer.DeserializeAsync<GetNewReleasesResult>(respStream);

            return responseObj?.albums?.items.Select(x => new Release()
            {
                Name = x.name,
                Date = x.release_date,
                Link = x.external_urls.spotify,
                Artist = string.Join(",", x.artists.Select(y => y.name))
            });
        }
    }
}
