using AggregationAPI.Models;

namespace AggregationAPI.Service.IService
{
    public interface IWeatherService
    {
        Task<ResponseDto?> FetchWeatherData(string city);
    }
}
