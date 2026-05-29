using System.Net;

namespace MarketList_API.Response
{
    public class ApiResponse
    {
        public object Response { get; }
        public HttpStatusCode StatusResult { get; }

        public ApiResponse(object response, HttpStatusCode statusResult)
        {
            Response = response;
            StatusResult = statusResult;
        }
    }
}