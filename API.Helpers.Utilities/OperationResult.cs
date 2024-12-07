namespace API.Helpers.Utilities;

public class OperationResult<T>
{
    public string Message { set; get; }
    public bool Succeeded { set; get; }
    public T Data { set; get; }

    public OperationResult()
    {
    }

    public OperationResult(string message)
    {
        Message = message;
    }

    public OperationResult(bool succeeded)
    {
        Succeeded = succeeded;
    }

    public OperationResult(T data)
    {
        Data = data;
    }

    public OperationResult(bool succeeded, string message)
    {
        Message = message;
        Succeeded = succeeded;
    }

    public OperationResult(bool succeeded, T data)
    {
        Succeeded = succeeded;
        Data = data;
    }

    public OperationResult(string message, T data)
    {
        Message = message;
        Data = data;
    }

    public OperationResult(bool succeeded, string message, T data)
    {
        Message = message;
        Succeeded = succeeded;
        Data = data;
    }

    public static OperationResult<T> ErrorResult(string message)
    {
        return new OperationResult<T>(false, message);
    }

    public static OperationResult<T> ErrorResult(string message, T data)
    {
        return new OperationResult<T>(false, message, data);
    }

    public static OperationResult<T> SuccessResult(string message)
    {
        return new OperationResult<T>(true, message);
    }

    public static OperationResult<T> SuccessResult(string message, T data)
    {
        return new OperationResult<T>(true, message, data);
    }
}