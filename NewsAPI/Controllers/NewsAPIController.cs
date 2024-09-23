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
                if (!string.IsNullOrEmpty(city))
                {
                    List<NewsModel> newsData = await _newsService.FetchNewsData(city);
                    _responseDto.Result = JsonConvert.SerializeObject(newsData);
                }
                else
                {
                    _responseDto.Result = null;
                    _responseDto.Message = "City name is missing or invalid.";
                    _responseDto.IsSuccess = false;
                }
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