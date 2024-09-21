using AggregationAPI.Models;
using AggregationAPI.Service.IService;
using AggregationAPI.Utility;

namespace AggregationAPI.Service
{
    public class NewsService : INewsService
    {
        private readonly IBaseService _baseService;

        public NewsService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> FetchNewsData(string city)
        {
            return await _baseService.SendAsync(new RequestDto()
            { 
                Data = city,
                Url = SD.NewsAPIBase + "/api/news/GetNews"
            });
        }
    }
}
