using Newtonsoft.Json.Linq;
using WeatherAPI.Models;

namespace WeatherAPI.Service.IService
{
    public class WeatherService : IWeatherService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<WeatherModel> FetchWeatherData(string city)
        {
            WeatherModel weatherModel = null;
            var client = _httpClientFactory.CreateClient("Weather");
            //var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat=37.9838&lon=23.7275&appid=5d3338979da798f3fdeff91e53b55d57");
            var response = await client.GetAsync(string.Format(client.BaseAddress.OriginalString, city));
            if (response.IsSuccessStatusCode)
            {
                var apiContent = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(apiContent);
                weatherModel = new()
                {
                    Condition = json["weather"][0]["description"].ToString(),
                    Temperature = json["main"]["temp"].ToString(),
                    City = json["name"].ToString(),
                    DataGetDate = DateTime.Now.Add(new TimeSpan((int.Parse(json["dt"].ToString()) * 1000) - (int.Parse(json["timezone"].ToString()) * 10000)))
                };
            }
            return weatherModel;
        }
    }
}