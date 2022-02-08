namespace PaymentGateway.Web.Api.Models;

public class ApiError
{
    public string ErrorCode { get; set; }

    public string Message { get; set; }

    public static ApiError Create(string code, string message)
    {
        return new ApiError
        {
            Message = message,
            ErrorCode = code
        };
    }
}