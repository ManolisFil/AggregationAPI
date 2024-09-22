using NewsAPI.Models;

namespace NewsAPI.Service.IService
{
    public interface INewsService
    {
        Task<List<NewsModel>> FetchNewsData(string city);
    }
}
