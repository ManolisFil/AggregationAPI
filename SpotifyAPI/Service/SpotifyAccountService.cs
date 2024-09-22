using SpotifyAPI.Models;
using SpotifyAPI.Service.IService;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SpotifyAPI.Service
{
    public class SpotifyAccountService : ISpotifyAccountService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SpotifyAccountService(IHttpClientFactory httpClientFactory)
        {                
            _httpClientFactory = httpClientFactory;
        }


        public async Task<string> GetToken(string clientId, string clientSecret)
        {
            var client = _httpClientFactory.CreateClient("SpotifyToken");

            var request = new HttpRequestMessage(HttpMethod.Post, "token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")));

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {
                    "grant_type","client_credentials"
                }
            });

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            using var respStream = await response.Content.ReadAsStreamAsync();
            var authResult = await JsonSerializer.DeserializeAsync<AuthResult>(respStream);

            return authResult.access_token;
        }
    }
}
