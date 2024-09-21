using AggregationAPI.Models;

namespace AggregationAPI.Service.IService
{
    public interface INewsService
    {
        Task<ResponseDto?> FetchNewsData(string city);
    }
}
