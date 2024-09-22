using SpotifyAPI.Models;

namespace SpotifyAPI.Service.IService
{
    public interface ISpotifyService
    {
        Task<IEnumerable<Release>> GetNewReleases(string accessToken);
    }
}