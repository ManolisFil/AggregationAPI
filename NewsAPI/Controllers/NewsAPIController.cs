using Microsoft.AspNetCore.Mvc;
using NewsAPI.Models;
using NewsAPI.Service.IService;
using Newtonsoft.Json;

namespace NewsAPI.Controllers
{
    [ApiController]
    [Route("api/news")]
    public class NewsAPIController
    {

        private readonly INewsService _newsService;

        private ResponseDto _responseDto;


        public NewsAPIController(INewsService newsService)
        {
            _responseDto = new ResponseDto();
            _newsService = newsService;
        }

        [HttpGet("GetNews/{city}")]
        public async Task<ResponseDto> GetNews(string city)
        {
            try
            {
                List<NewsModel> newsData = new List<NewsModel>();
                if (!string.IsNullOrWhiteSpace(city))
                {
                    newsData = await _newsService.FetchNewsData(city);
                }
                _responseDto.Result = JsonConvert.SerializeObject(newsData);
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
                _responseDto.IsSuccess = false;
            }

            return _responseDto;
        }
    }
}