using Newtonsoft.Json.Linq;
using System.Text.Json;
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
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                using var respStream = await response.Content.ReadAsStreamAsync();
                var responseObj = await JsonSerializer.DeserializeAsync<WeatherResponseModel>(respStream);
                weatherModel = new()
                {
                    Condition = responseObj.weather.FirstOrDefault().description,
                    Temperature = responseObj.main.temp,
                    City = responseObj.name,
                    DataGetDate =  DateTime.Now.Add(new TimeSpan(responseObj.dt * 1000 - responseObj.timezone * 10000))
                };
            }
            return weatherModel;
        }
    }
}