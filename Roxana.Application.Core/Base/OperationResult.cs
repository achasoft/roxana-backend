using Roxana.Application.Core.Enums.Base;

namespace Roxana.Application.Core.Base;

public class OperationResult<T>
{
    public T Data { get; set; }
    public OperationResultStatus Status { get; set; }
    public string Message { get; set; }
    
    public static OperationResult<T> Success(T data = default, string message = null)
    {
        return new OperationResult<T>
        {
            Data = data,
            Message = message ?? "OPERATION_SUCCESSFUL",
            Status = OperationResultStatus.Success
        };
    }

    public static OperationResult<T> Fail(T data = default, string message = null)
    {
        return new OperationResult<T>
        {
            Data = data,
            Message = message ?? "OPERATION_FAILED",
            Status = OperationResultStatus.Failed
        };
    }

    public static OperationResult<T> NotFound(T data = default, string message = null)
    {
        return new OperationResult<T>
        {
            Data = data,
            Message = message ?? "NOT_FOUND",
            Status = OperationResultStatus.NotFound
        };
    }

    public static OperationResult<T> Unauthorized(T data = default, string message = null)
    {
        return new OperationResult<T>
        {
            Data = data,
            Message = message ?? "NOT_AUTHORIZED",
            Status = OperationResultStatus.Unauthorized
        };
    }
}