using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Models;
using SpotifyAPI.Service.IService;

namespace SpotifyAPI.Controllers
{
    [ApiController]
    [Route("api/spotify")]
    public class SpotifyAPIController
    {

        private readonly ISpotifyAccountService _spotifyAccountService;
        private readonly ISpotifyService _spotifyService;
        private readonly IConfiguration _configuration;
        
        public SpotifyAPIController(ISpotifyAccountService spotifyAccountService, ISpotifyService spotifyService,  IConfiguration configuration)
        {
            _spotifyService = spotifyService;
            _spotifyAccountService = spotifyAccountService;
            _configuration = configuration;
        }


        [HttpGet("GetNewReleases")]
        public async Task<ResponseDto> GetNewReleases()
        {
            ResponseDto resp = new ResponseDto();
            try
            {
                var token = await _spotifyAccountService.GetToken(_configuration.GetValue<string>("Spotify:ClientId"), _configuration.GetValue<string>("Spotify:ClientSecret"));

                var newReleases = await _spotifyService.GetNewReleases(token);
                resp.Result = newReleases;
            }
            catch (Exception ex) 
            {
                resp.Message = ex.Message;
                resp.IsSuccess = false;
            }

            return resp; ;
        }
    }
}