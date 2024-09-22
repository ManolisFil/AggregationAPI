using AggregationAPI.Models;

namespace AggregationAPI.Service.IService
{
    public interface ISpotifyService
    {
        Task<ResponseModel> FetchNewReleases();
    }
}
