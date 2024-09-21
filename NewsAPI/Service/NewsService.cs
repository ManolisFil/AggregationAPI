using NewsAPI.Models;
using NewsAPI.Service.IService;

namespace NewsAPI.Service
{
    public class NewsService : INewsService
    {
        public Task<NewsModel> FetchNewsData(string city)
        {
            throw new NotImplementedException();
        }
    }
}
