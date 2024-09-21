using AggregationAPI.Models;
using AggregationAPI.Service.IService;
using Newtonsoft.Json;
using System.Text;

namespace AggregationAPI.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ResponseModel?> SendAsync(RequestModel requestDto)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("AggregationAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(requestDto.Url);
                message.Method = HttpMethod.Get;
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