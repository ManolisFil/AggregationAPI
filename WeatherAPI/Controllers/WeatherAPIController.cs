using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Models;
using WeatherAPI.Service.IService;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class WeatherAPIController
    {
       private readonly IWeatherService _weatherService;

        private ResponseDto _responseDto;


        public WeatherAPIController(IWeatherService weatherService)
        {
            _responseDto = new ResponseDto();
            _weatherService = weatherService;
        }


        [HttpGet("GetWeather/{city}")]
        public async Task<ResponseDto> GetCart(string city)
        {
            try
            {
                WeatherModel weatherData = new WeatherModel();
                //if city is empty
                if (!string.IsNullOrWhiteSpace(city))
                {
                    weatherData = await _weatherService.FetchWeatherData(city);
                }
                _responseDto.Result = weatherData;
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
                _responseDto.IsSuccess = false;
            }

            return _responseDto;
        }


    }
}
