using AggregationAPI.Models;
using AggregationAPI.Service.IService;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AggregationAPI.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IMemoryCache _cache;

        public BaseService(IHttpClientFactory httpClientFactory, IMemoryCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }

        public async Task<ResponseModel> SendAsync(RequestModel requestDto)
        {
            try
            {
                if(_cache.TryGetValue(requestDto.Url, out ResponseModel response))
                    return response;

                HttpClient client = _httpClientFactory.CreateClient("AggregationAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(requestDto.Url);
                message.Method = HttpMethod.Get;
                // in case we need a post request in the future
                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage apiResponse = await client.SendAsync(message);

                switch (apiResponse.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return new ResponseModel() { IsSuccess = false, Message = "Not Found" };
                    case System.Net.HttpStatusCode.Forbidden:
                        return new ResponseModel() { IsSuccess = false, Message = "Access Denied" };
                    case System.Net.HttpStatusCode.InternalServerError:
                        return new ResponseModel() { IsSuccess = false, Message = "Internal Server Error" };
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseModel>(apiContent);

                        var cacheEntryOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10), // Cache for 10 minutes
                            SlidingExpiration = TimeSpan.FromMinutes(5)                 // Reset timer if accessed within 5 minutes
                        };

                        _cache.Set(requestDto.Url, apiResponseDto, cacheEntryOptions); // Cache the dat
                        return apiResponseDto;
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel() { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}