using AggregationAPI.Models;
using AggregationAPI.Service.IService;
using AggregationAPI.Utility;

namespace AggregationAPI.Service
{
    public class SpotifyService : ISpotifyService
    {
        private readonly IBaseService _baseService;

        public SpotifyService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseModel?> FetchNewReleases()
        {
            return await _baseService.SendAsync(new RequestModel()
            {
                Url = SD.SpotifyAPIBase + "/api/Spotify/GetNewReleases/"
            });
        } 
    }
}
