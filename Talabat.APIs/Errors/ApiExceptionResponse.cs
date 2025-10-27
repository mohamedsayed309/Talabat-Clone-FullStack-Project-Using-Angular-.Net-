namespace Talabat.APIs.Errors
{
    public class ApiExceptionResponse:ApiResponse
    {
        public string? Details { get; set; }
        public ApiExceptionResponse(int statusCode, string? Message=null, string? details = null):base(statusCode,Message)
        {
            Details = details;
        }
    }
}
