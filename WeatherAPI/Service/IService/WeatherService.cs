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