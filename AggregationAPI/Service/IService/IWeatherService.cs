using AggregationAPI.Models;

namespace AggregationAPI.Service.IService
{
    public interface IWeatherService
    {
        Task<ResponseModel> FetchWeatherData(string city);
    }
}
