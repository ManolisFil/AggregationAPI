using AggregationAPI.Models;
using AggregationAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AggregationAPI.Controllers
{
    [ApiController]
    [Route("api/aggregation")]
    public class AggregationAPIController : Controller
    {
        private readonly IWeatherService _weatherService;
        private readonly INewsService _newsService;
        private readonly ISpotifyService _spotifyService;

        public AggregationAPIController(IWeatherService weatherService, INewsService newsService, ISpotifyService spotifyService)
        {
            _weatherService = weatherService;
            _newsService = newsService;
            _spotifyService = spotifyService;
        }

        [HttpGet("GetData/{city}")]
        public async Task<AggregationModel> GetData(string city)
        {
            AggregationModel respAgrModel = new AggregationModel();
            try
            {
                var weatherTask = _weatherService.FetchWeatherData(city);
                var newsTask = _newsService.FetchNewsData(city);
                var spotifyTask = _spotifyService.FetchNewReleases();

                await Task.WhenAll(weatherTask, newsTask, spotifyTask);

                var weatherData = weatherTask.Result;
                var newsData = newsTask.Result;
                var spotifyData = spotifyTask.Result;

                respAgrModel.Weather = weatherData.Result != null ? JsonConvert.DeserializeObject<WeatherModel>(weatherData.Result) : null;
                respAgrModel.News = newsData.Result != null ? JsonConvert.DeserializeObject<List<NewsModel>>(newsData.Result) : null;
                respAgrModel.Release = spotifyData.Result != null ? JsonConvert.DeserializeObject<List<ReleaseModel>>(spotifyData.Result) : null;

                return respAgrModel;
            }
            catch (Exception ex)
            {
                respAgrModel.Message = ex.Message;
                respAgrModel.IsSuccess = false;
            }

            return respAgrModel;
        }
    }
}