using FakeItEasy;
using FluentAssertions;
using NewsAPI.Controllers;
using NewsAPI.Models;
using NewsAPI.Service.IService;

namespace NewsTest
{
    public class NewsServiceTest
    {
        private readonly INewsService _newsService;
        private readonly NewsAPIController _controller;

        public NewsServiceTest()
        {
            _newsService = A.Fake<INewsService>();
            _controller = new NewsAPIController(_newsService);
        }

        [Fact]
        public async Task NewsAPIController_GetNews_ReturnOK()
        {
            // Arrange
            string city = "athens";
            var newsList = A.Fake<List<NewsModel>>();
            A.CallTo(() => _newsService.FetchNewsData(city)).Returns(newsList);

            //// Act
            var result = await _controller.GetNews(city);

            //Assert
            result.Should().NotBeNull();
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task GetNews_ReturnsNull_WhenCityIsEmpty()
        {
            // Arrange
            string city = ""; // Empty city input

            // Act
            var newsList = A.Fake<List<NewsModel>>();
            A.CallTo(() => _newsService.FetchNewsData(city)).Returns(newsList);
            var result = await _controller.GetNews(city);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result);
            Assert.Null(result.Result);
        }

        [Fact]
        public async Task GetNews_ReturnsNull_WhenCityIsNull()
        {
            // Arrange
            string city = null;

            // Act
            var newsList = A.Fake<List<NewsModel>>();
            A.CallTo(() => _newsService.FetchNewsData(city)).Returns(newsList);
            var result = await _controller.GetNews(city);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result);
            Assert.Null(result.Result);
        }
    }
}