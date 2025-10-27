
namespace Talabat.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int statuscode,string? message=null)
        {
            StatusCode = statuscode;
            Message = message?? GetDefaultMessageForStatusCode(statuscode);
        }

        private string? GetDefaultMessageForStatusCode(int statuscode)
        {
            return statuscode switch
            {
                400 => "A bad reques",
                401 => "Authorized, You are not Authorized",
                404 => "Resource wasn't found",
                500 => "Server Error, Please try again later",
                _ => null
            };
        }
    }
}
