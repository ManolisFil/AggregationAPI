using AggregationAPI.Models;

namespace AggregationAPI.Service.IService
{
    public interface INewsService
    {
        Task<ResponseModel?> FetchNewsData(string city);
    }
}
