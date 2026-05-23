namespace BackendAromaseoLimpieza;

public class Result<Tvalue, TStatus>
{
    public Tvalue Value { get; set; }
    public bool IsSuccess { get; set; }
    public string Error { get; set; }
    public TStatus Status { get; set; }
    
    private Result(Tvalue value,  bool success, string error, TStatus status)
    {
        Value = value;
        IsSuccess = success;
        Error = error;
        Status = status;
    }

    public static Result<Tvalue, TStatus> Success(Tvalue value, TStatus status) => new Result<Tvalue, TStatus>(value, true, null, status);
    
    public static Result<Tvalue, TStatus> Failure(string error, TStatus status) => new Result<Tvalue, TStatus>(default, false, error, status);
}