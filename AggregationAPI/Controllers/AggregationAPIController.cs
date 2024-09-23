using AggregationAPI.Models;
using AggregationAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace AggregationAPI.Controllers
{
    [ApiController]
    [Route("api/aggregation")]
    public class AggregationAPIController : Controller
    {
        private readonly IWeatherService _weatherService;
        private readonly INewsService _newsService;
        private readonly ISpotifyService _spotifyService;

        private readonly IMemoryCache _cache;

        public AggregationAPIController(IWeatherService weatherService, INewsService newsService, ISpotifyService spotifyService, IMemoryCache cache)
        {
            _weatherService = weatherService;
            _newsService = newsService;
            _spotifyService = spotifyService;
            _cache = cache;
        }

        [HttpGet("GetData/{city}")]
        public async Task<AggregationModel> GetData(string city)
        {
            AggregationModel respAgrModel = new AggregationModel();
            if (string.IsNullOrWhiteSpace(city))
                return new AggregationModel() { IsSuccess = false, Message = "City name is missing." };
            try
            {
                respAgrModel = await MakeCall(city);
                if (!respAgrModel.IsSuccess)
                {
                    respAgrModel.News = null;
                    respAgrModel.Release = null;
                    respAgrModel.IsSuccess = false;
                    respAgrModel.Message = respAgrModel.Message;
                }               
            }
            catch (Exception ex)
            {
                respAgrModel.Message = ex.Message;
                respAgrModel.IsSuccess = false;
            }

            return respAgrModel;
        }

        [HttpPost("GetAggregatedDataSorted")]
        public async Task<AggregationModel> GetAggregatedDataSorted([FromBody] SortModel model)    
        {
            if (model == null || string.IsNullOrWhiteSpace(model.City))
                return new AggregationModel() { IsSuccess = false, Message = "City name is invalid/missing." };

            AggregationModel respAgrModel = await MakeCall(model.City);
            if (respAgrModel.Weather == null)
            {
                respAgrModel.News = null;
                respAgrModel.Release = null;
                respAgrModel.IsSuccess = false;
                respAgrModel.Message = "City name is invalid.";
                return respAgrModel;
            }

            if (model.Date.HasValue)
            {
                respAgrModel.News = respAgrModel.News.Where(d => d.Date >= model.Date).ToList();
                respAgrModel.Release = respAgrModel.Release.Where(d => d.Date >= model.Date).ToList();
            }

            switch (model.SortBy.ToLower())
            {
                case "date":
                    if (model.Ascending)
                    {
                        respAgrModel.News = respAgrModel.News.OrderBy(x => x.Date).ToList();
                        respAgrModel.Release = respAgrModel.Release.OrderBy(x => x.Date).ToList();
                    }
                    else
                    {
                        respAgrModel.News = respAgrModel.News.OrderByDescending(x => x.Date).ToList();
                        respAgrModel.Release = respAgrModel.Release.OrderByDescending(x => x.Date).ToList();
                    }                
                    break;
                case "title":
                    if (model.Ascending)
                    {
                        respAgrModel.News = respAgrModel.News.OrderBy(x => x.Title).ToList();
                        respAgrModel.Release = respAgrModel.Release.OrderBy(x => x.Name).ToList();
                    }
                    else
                    {
                        respAgrModel.News = respAgrModel.News.OrderByDescending(x => x.Title).ToList();
                        respAgrModel.Release = respAgrModel.Release.OrderByDescending(x => x.Name).ToList();
                    } 
                    break;
            }
            return respAgrModel;
        }


        [HttpGet("GatherStatistics")]
        public async Task<List<StatisticsModel>> GatherStatistics()
        {
            List<StatisticsModel> statList = new List<StatisticsModel>();
            statList.Add(await FetchStats("News"));
            statList.Add(await FetchStats("Spotify"));
            statList.Add(await FetchStats("Weather"));
            return statList;
        }

        private async Task<StatisticsModel> FetchStats(string apiName)
        {
            if (_cache.TryGetValue(apiName, out StatisticsModel stats))
            {
                stats.AverageResponseTime = stats.TotalRequests == 0 ? 0 : stats.Time / stats.TotalRequests;
                stats.Performance = stats.AverageResponseTime < 1000 ? "fast" :
                                    stats.AverageResponseTime < 2000 ? "average" : "slow";
            }
            else
            {
                stats = new StatisticsModel()
                {
                    ApiName = apiName
                };
            }

            return stats;
        }
         

        private async Task<AggregationModel> MakeCall(string city)
        {
            AggregationModel respAgrModel = new AggregationModel();
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
            respAgrModel.IsSuccess = weatherData.IsSuccess && newsData.IsSuccess && spotifyData.IsSuccess;
            respAgrModel.Message = weatherData.Message + " " + newsData.Message + " " + spotifyData.Message;

            await CalcMetrics("News", newsData.Time);
            await CalcMetrics("Spotify", newsData.Time);
            await CalcMetrics("Weather", weatherData.Time);

            return respAgrModel;
        }

        private async Task CalcMetrics(string apiName, long time)
        {
            if (_cache.TryGetValue(apiName, out StatisticsModel stats))
            {
                stats.TotalRequests++;
                stats.Time += time;
            }
            else
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };
                stats = new StatisticsModel()
                {
                    TotalRequests = 1,
                    ApiName = apiName,
                    Time = time
                };
                _cache.Set(apiName, stats, cacheEntryOptions);
            }
        }
    }
}