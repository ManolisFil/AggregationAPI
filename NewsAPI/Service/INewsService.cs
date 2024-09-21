using NewsAPI.Models;

namespace NewsAPI.Service
{
    public interface INewsService
    {
        Task<NewsModel> FetchNewsData(string city);
    }
}
