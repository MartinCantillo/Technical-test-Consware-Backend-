namespace backend.Utils
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public string? Error { get; set; }
        public int Status { get; set; }

        public static ApiResponse<T> Success(T data, int status = 200)
        {
            return new ApiResponse<T> { Data = data, Status = status };
        }

        public static ApiResponse<T> Fail(string error, int status = 400)
        {
            return new ApiResponse<T> { Error = error, Status = status };
        }

        public static ApiResponse<T> NotFound(string error = "Resource not found")
        {
            return new ApiResponse<T> { Error = error, Status = 404 };
        }

        public static ApiResponse<T> Unauthorized(string error = "Unauthorized")
        {
            return new ApiResponse<T> { Error = error, Status = 401 };
        }

        public static ApiResponse<T> Forbidden(string error = "Forbidden")
        {
            return new ApiResponse<T> { Error = error, Status = 403 };
        }

        public static ApiResponse<T> ServerError(string error = "Internal server error")
        {
            return new ApiResponse<T> { Error = error, Status = 500 };
        }

        public static ApiResponse<T> Created(T data, int status = 201)
        {
            return new ApiResponse<T> { Data = data, Status = status };
        }

        public static ApiResponse<T> NoContent(string? message = null)
        {
            return new ApiResponse<T> { Error = message, Status = 204 };
        }
    }
}
