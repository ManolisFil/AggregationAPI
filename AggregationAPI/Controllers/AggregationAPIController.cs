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
        private readonly ISpotifyService  _spotifyService;

        public AggregationAPIController(IWeatherService weatherService, INewsService newsService, ISpotifyService spotifyService)
        {
            _weatherService = weatherService;
            _newsService = newsService;
            _spotifyService = spotifyService;
        }


        [HttpGet("GetData/{city}")]
        public async Task<ResponseModel> GetData(string city)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var weatherTask = _weatherService.FetchWeatherData(city);
                var newsTask = _newsService.FetchNewsData(city); 
                var spotifyTask = _spotifyService.FetchNewReleases(); 

                await Task.WhenAll(weatherTask, newsTask, spotifyTask);

                var weatherData = weatherTask.Result;
                var newsData = newsTask.Result;
                var spotifyData = spotifyTask.Result;


                // AggregationModel aggregationModel = new AggregationModel();
                //response.Result = newsData;
                response.Result = new List<Object>() { weatherData.Result, newsData.Result };
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
