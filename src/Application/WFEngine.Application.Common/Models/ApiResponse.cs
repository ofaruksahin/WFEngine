using System.Net;
using System.Text.Json.Serialization;

namespace WFEngine.Application.Common.Models
{
    public class ApiResponse<TModel>
        where TModel : class
    {
        public TModel Data { get; private set; }
        public List<string> Messages { get; private set; }

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; private set; }

        [JsonIgnore]
        public bool IsSuccess { get; set; }

        public ApiResponse()
        {
            Messages = new List<string>();
        }

        public ApiResponse<TModel> SetSuccess(HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            IsSuccess = true;
            StatusCode = statusCode;

            return this;
        }

        public ApiResponse<TModel> SetFailure(HttpStatusCode statusCode = HttpStatusCode.NotFound)
        {
            IsSuccess = false;
            StatusCode = statusCode;

            return this;
        }

        public ApiResponse<TModel> AddData(TModel data)
        {
            Data = data;

            return this;
        }

        public ApiResponse<TModel> AddMessage(string message)
        {
            Messages.Add(message);

            return this;
        }

        public ApiResponse<TModel> AddMessages(IEnumerable<string> messages)
        {
            Messages.AddRange(messages);

            return this;
        }
    }
}
