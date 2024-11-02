namespace Ticket_Service.Common.Api;

public class ApiResponse<T>
{
        public T? Data { get; init; }
        public ApiError? Error { get; init; }

        public static ApiResponse<T> Success(T data)
        {
                return new ApiResponse<T> { Data = data };
        }

        public static ApiResponse<T> Failure(string message, int statusCode = 500)
        {
                return new ApiResponse<T>
                {
                        Error = new ApiError
                        {
                                Message = message,
                                Status = statusCode
                        }
                };
        }
}


public  class ApiError
{
        public string? Message { get; set; } = string.Empty;
        public int Status { get; set; }
}