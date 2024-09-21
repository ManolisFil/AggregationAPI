using NewsAPI.Models;
using NewsAPI.Service.IService;
using Newtonsoft.Json.Linq;

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

        public async Task<NewsModel> FetchNewsData(string city)
        {
            NewsModel newsData = null;
            var client = _httpClientFactory.CreateClient("News");
            string newsUriStr = _configuration.GetValue<string>("NewsURI");
            string newsApiKey = _configuration.GetValue<string>("NewsAPIKey");

            client.DefaultRequestHeaders.Add("user-agent", "News-API-csharp/0.1");
            client.DefaultRequestHeaders.Add("x-api-key", newsApiKey);

            var response = await client.GetAsync(string.Format(newsUriStr, city));
            if (response.IsSuccessStatusCode)
            {
                var apiContent = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(apiContent);
                newsData = new()
                {
                    Title = json["articles"][0]["title"].ToString(),
                    Description = json["articles"][0]["description"].ToString(),
                    Date = DateTime.Parse(json["articles"][0]["publishedAt"].ToString()), 
                };
            }

            return newsData;
        }
    }
}
