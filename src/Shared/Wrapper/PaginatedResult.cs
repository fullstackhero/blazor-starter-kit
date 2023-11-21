using System.Text.Json.Serialization;

namespace CleanBlazor.Shared.Wrapper;

public class PaginatedResult<T> : Result
{
    /// <summary>
    /// Parameterless constructor is required for deserialization.
    /// </summary>
    [JsonConstructor]
    public PaginatedResult() { }

    private PaginatedResult(
        bool isSuccess,
        List<T> data = default,
        string successMessage = null,
        IEnumerable<string> errorMessages = null,
        int count = 0,
        int page = 1,
        int pageSize = 10) : base(isSuccess, successMessage, errorMessages)
    {
        Data = data;
        CurrentPage = page;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
    }

    [JsonInclude]
    public List<T> Data { get; private set; }

    [JsonInclude]
    public int CurrentPage { get; private set; }

    [JsonInclude]
    public int TotalPages { get; private set; }

    [JsonInclude]
    public int TotalCount { get; private set; }

    [JsonInclude]
    public int PageSize { get; private set; }

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < TotalPages;

    public static PaginatedResult<T> Ok(List<T> data, int count, int page, int pageSize) =>
        new(true, data, null, null, count, page, pageSize);
}
