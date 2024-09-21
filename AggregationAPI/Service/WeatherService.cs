using AggregationAPI.Models;
using AggregationAPI.Service.IService;
using AggregationAPI.Utility;

namespace AggregationAPI.Service
{
    public class WeatherService : IWeatherService
    {
        private readonly IBaseService _baseService;

        public WeatherService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseModel?> FetchWeatherData(string city)
        {
            return await _baseService.SendAsync(new RequestModel()
            { 
                Url = SD.WeatherAPIBase + "/api/weather/GetWeather/" + city,
            });
        }        
    }
}
