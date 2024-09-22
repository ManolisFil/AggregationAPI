using SpotifyAPI.Models;

namespace SpotifyAPI.Service.IService
{
    public interface ISpotifyService
    {
        Task<IEnumerable<Release>> GetNewReleases(string countryCode, int limit, string accessToken);
    }
}
