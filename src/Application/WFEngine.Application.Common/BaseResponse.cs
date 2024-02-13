using System.Net;
using System.Text.Json.Serialization;

namespace WFEngine.Application.Common
{
    public class BaseResponse
    {
        [JsonIgnore]
        public bool IsSuccess { get; private set; }
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; private set; }
        public List<string> Messages { get; private set; }
        public object Data { get; private set; }

        public BaseResponse(
            bool isSuccess,
            HttpStatusCode statusCode,
            List<string> messages,
            object data)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Messages = messages;
            Data = data;
        }

        public static BaseResponse Success(object data, HttpStatusCode statusCode = HttpStatusCode.OK, string message = "")
        {
            return new BaseResponse(true, statusCode, new List<string> { message }, data);
        }

        public static BaseResponse Failure(object data, HttpStatusCode statusCode = HttpStatusCode.NotFound, string message = "")
        {
            return new BaseResponse(true, statusCode, new List<string> { message }, data);
        }

        public static BaseResponse Failure(object data, HttpStatusCode statusCode = HttpStatusCode.NotFound, List<string> messages = null)
        {
            messages = messages ?? new List<string>();
            return new BaseResponse(true, statusCode, messages, data);
        }
    }

    public class NoContent
    {
    }
}
