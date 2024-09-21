using Newtonsoft.Json.Linq;
using WeatherAPI.Models;
using WeatherAPI.Service.IService;

namespace WeatherAPI.Service
{
    public class WeatherService : IWeatherService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IConfiguration _configuration;

        public WeatherService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<WeatherModel> FetchWeatherData(string city)
        {
            WeatherModel weatherModel = null;
            var client = _httpClientFactory.CreateClient("Weather");
            string weatherUriStr = _configuration.GetValue<string>("WeatherUrl");
            var response = await client.GetAsync(string.Format(weatherUriStr, city));
            if (response.IsSuccessStatusCode)
            {
                var apiContent = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(apiContent);
                weatherModel = new()
                {
                    Condition = json["weather"][0]["description"].ToString(),
                    Temperature = json["main"]["temp"].ToString(),
                    City = json["name"].ToString(),
                    DataGetDate = DateTime.Now.Add(new TimeSpan(int.Parse(json["dt"].ToString()) * 1000 - int.Parse(json["timezone"].ToString()) * 10000))
                };
            }
            return weatherModel;
        }
    }
}