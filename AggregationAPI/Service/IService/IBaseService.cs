using AggregationAPI.Models;

namespace AggregationAPI.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseModel> SendAsync(RequestModel requestDto);
    }
}
