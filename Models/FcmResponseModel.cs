using Newtonsoft.Json;

namespace HomeStrategiesApi.Models
{
    public class FcmResponseModel
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
