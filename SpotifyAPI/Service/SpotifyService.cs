﻿using SpotifyAPI.Models;
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

        public async Task<List<ReleaseModel>> GetNewReleases(string accessToken)
        {
            var client = _httpClientFactory.CreateClient("Spotify");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync($"browse/new-releases?limit={10}");
            //var response = await client.GetAsync($"browse/new-releases"); 
            response.EnsureSuccessStatusCode();
            using var respStream = await response.Content.ReadAsStreamAsync();
            var responseObj = await JsonSerializer.DeserializeAsync<GetNewReleasesResult>(respStream);

            return (responseObj?.albums?.items.Select(x => new ReleaseModel()
            {
                Name = x.name,
                Date = DateTime.ParseExact(x.release_date, "yyyy-MM-dd", null),
                Link = x.external_urls.spotify,
                Artist = string.Join(",", x.artists.Select(y => y.name))
            }).ToList()) ?? new List<ReleaseModel>();
        }
    }
}