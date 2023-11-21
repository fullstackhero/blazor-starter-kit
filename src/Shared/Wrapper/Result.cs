using System.Text.Json.Serialization;

namespace CleanBlazor.Shared.Wrapper;

public class Result
{
    /// <summary>
    /// Parameterless constructor is required for deserialization.
    /// </summary>
    public Result() {}
    protected Result(bool isSuccess, string successMessage, IEnumerable<string> errorMessages)
    {
        IsSuccess = isSuccess;

        if (isSuccess)
        {
            if (errorMessages?.Any() == true)
            {
                throw new ArgumentException("Cannot provide error messages for a successful result.");
            }
            SuccessMessage = successMessage;
        }
        else
        {
            if (!string.IsNullOrEmpty(successMessage))
            {
                throw new ArgumentException("Cannot provide a success message for a failed result.");
            }
            ErrorMessages = (errorMessages ?? Array.Empty<string>()).ToList().AsReadOnly();
        }
    }

    [JsonInclude]
    public bool IsSuccess { get; private set; }

    public bool IsFailure => !IsSuccess;


    [JsonInclude]
    public string SuccessMessage { get; private set; }

    [JsonInclude]
    public IReadOnlyList<string> ErrorMessages { get; private set; }

    public static Result Ok(string message = null) => new(true, message, null);
    public static Result<T> Ok<T>(T data, string message = null) => new(data, true, message, null);

    public static Result Fail() => new(false, null, null);
    public static Result<T> Fail<T>() => new (default, false, null, null);
    public static Result Fail(string message) => new(false, null, new []{ message });
    public static Result<T> Fail<T>(string message) => new(default, false, null, new []{ message });
    public static Result Fail(IEnumerable<string> messages) => new(false, null, messages);
    public static Result<T> Fail<T>(IEnumerable<string> messages) => new(default, false, null, messages);
}

public class Result<T> : Result
{
    /// <summary>
    /// Parameterless constructor is required for deserialization.
    /// </summary>
    public Result() {}
    protected internal Result(T data, bool isSuccess, string successMessage, IEnumerable<string> errorMessages)
        : base(isSuccess, successMessage, errorMessages)
    {
        Data = data;
    }

    public static implicit operator Result<T>(T data) => Ok(data);

    [JsonInclude]
    public T Data { get; private set; }
}
