using AggregationAPI.Models;
using AggregationAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace AggregationAPI.Controllers
{
    [ApiController]
    [Route("api/aggregation")]
    public class AggregationAPIController : Controller
    {
        private readonly IWeatherService _weatherService;
        private readonly INewsService  _newsService;

        public AggregationAPIController(IWeatherService weatherService, INewsService newsService)
        {
            _weatherService = weatherService;
            _newsService = newsService;
        }


        [HttpGet("GetData/{city}")]
        public async Task<ResponseModel> GetData(string city)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var weatherTask = _weatherService.FetchWeatherData(city);
                var newsTask = _newsService.FetchNewsData(city); 

                await Task.WhenAll(weatherTask, newsTask);

                var weatherData = weatherTask.Result;
                var newsData = newsTask.Result;


                // AggregationModel aggregationModel = new AggregationModel();
                //response.Result = newsData;
                response.Result = new List<Object>() { weatherData, newsData };
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
            }

            return response;
        }

    }
}
