using NewsAPI.Models;

namespace NewsAPI.Service.IService
{
    public interface INewsService
    {
        Task<NewsModel> FetchNewsData(string city);
    }
}
