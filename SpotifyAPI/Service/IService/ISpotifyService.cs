using SpotifyAPI.Models;

namespace SpotifyAPI.Service.IService
{
    public interface ISpotifyService
    {
        Task<List<ReleaseModel>> GetNewReleases(string accessToken);
    }
}