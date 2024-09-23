using FakeItEasy;
using WeatherAPI.Controllers;
using WeatherAPI.Models;
using WeatherAPI.Service.IService;
using FluentAssertions;

namespace WeatherTest
{
    public class WeatherServiceTest
    {
        private readonly IWeatherService _weatherService;
        private readonly WeatherAPIController _controller;

        public WeatherServiceTest()
        {
            _weatherService = A.Fake<IWeatherService>();
            _controller = new WeatherAPIController(_weatherService);
        }

        [Fact]
        public async Task WeatherAPIController_GetNews_ReturnOK()
        {
            // Arrange
            string city = "athens";
            var weatherModel = A.Fake<WeatherModel>();
            A.CallTo(() => _weatherService.FetchWeatherData(city)).Returns(weatherModel);

            //// Act
            var result = await _controller.GetWeather(city);

            //Assert  
            result.Should().NotBeNull();
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task WeatherAPIController_GetNews_EmptyCityParam_ReturnNotOK()
        {
            // Arrange
            string city = "";
            var weatherModel = A.Fake<WeatherModel>();
            A.CallTo(() => _weatherService.FetchWeatherData(city)).Returns(weatherModel);

            //// Act
            var result = await _controller.GetWeather(city);

            //Assert 
            Assert.False(result.IsSuccess);
            Assert.NotNull(result);
            Assert.Null(result.Result);
        }

        [Fact]
        public async Task WeatherAPIController_GetNews_NullCityParam_ReturnNotOK()
        {
            // Arrange
            string city = null;
            var weatherModel = A.Fake<WeatherModel>();
            A.CallTo(() => _weatherService.FetchWeatherData(city)).Returns(weatherModel);

            //// Act
            var result = await _controller.GetWeather(city);

            //Assert 
            Assert.False(result.IsSuccess);
            Assert.NotNull(result);
            Assert.Null(result.Result);
        }
    }
}