using NewsAPI.Models;
using NewsAPI.Service.IService;
using System.Text.Json;

namespace NewsAPI.Service
{
    public class NewsService : INewsService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IConfiguration _configuration;

        public NewsService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<List<NewsModel>> FetchNewsData(string city)
        {
            var client = _httpClientFactory.CreateClient("News");
            string newsUriStr = _configuration.GetValue<string>("NewsURI");
            string newsApiKey = _configuration.GetValue<string>("NewsAPIKey");
            client.DefaultRequestHeaders.Add("user-agent", "News-API-csharp/0.1");
            client.DefaultRequestHeaders.Add("x-api-key", newsApiKey);

            var response = await client.GetAsync(string.Format(newsUriStr, city));
            response.EnsureSuccessStatusCode();
           
            using var respStream = await response.Content.ReadAsStreamAsync();
            var responseObj = await JsonSerializer.DeserializeAsync<GetNewsResultModel>(respStream);

            return (responseObj?.articles?.Select(x => new NewsModel()
            {
                Title = x.title,
                Description = x.description,
                Date = x.publishedAt
            }).Take(10).ToList() ?? new List<NewsModel>());
        }
    }
}
