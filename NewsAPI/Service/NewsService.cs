using NewsAPI.Models;

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
