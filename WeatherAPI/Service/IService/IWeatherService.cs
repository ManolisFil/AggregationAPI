using WeatherAPI.Models;

namespace WeatherAPI.Service.IService
{
    public interface IWeatherService
    {
        Task<WeatherModel> FetchWeatherData(string city);
    }
}