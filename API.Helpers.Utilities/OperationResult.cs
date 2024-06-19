namespace API.Helpers.Utilities;

public class OperationResult<T>
{
    public string Error { set; get; }
    public bool IsSuccess { set; get; }
    public T Data { set; get; }

    public OperationResult()
    {
    }

    public OperationResult(string error)
    {
        this.Error = error;
    }

    public OperationResult(bool isSuccess)
    {
        this.IsSuccess = isSuccess;
    }

    public OperationResult(T data)
    {
        this.Data = data;
    }

    public OperationResult(bool isSuccess, string error)
    {
        this.Error = error;
        this.IsSuccess = isSuccess;
    }

    public OperationResult(bool isSuccess, T data)
    {
        this.IsSuccess = isSuccess;
        this.Data = data;
    }

    public OperationResult(string error, T data)
    {
        this.Error = error;
        this.Data = data;
    }

    public OperationResult(bool isSuccess, string error, T data)
    {
        this.Error = error;
        this.IsSuccess = isSuccess;
        this.Data = data;
    }
}