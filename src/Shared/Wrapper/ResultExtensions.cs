namespace CleanBlazor.Shared.Wrapper;

public static class ResultExtensions
{
    // MATCH EXTENSIONS
    // 1. Non-generic, void return, both sync
    public static async Task Match(
        this Task<Result> resultTask,
        Action<string> onSuccess,
        Action<IReadOnlyList<string>> onFailure)
    {
        var result = await resultTask;
        if (result.IsSuccess)
            onSuccess(result.SuccessMessage);
        else
            onFailure(result.ErrorMessages);
    }

    // 2. Non-generic, void return, async onSuccess, sync onFailure
    public static async Task Match(
        this Task<Result> resultTask,
        Func<string, Task> onSuccessAsync,
        Action<IReadOnlyList<string>> onFailure)
    {
        var result = await resultTask;
        if (result.IsSuccess)
            await onSuccessAsync(result.SuccessMessage);
        else
            onFailure(result.ErrorMessages);
    }

    // 3. Non-generic, void return, sync onSuccess, async onFailure
    public static async Task Match(
        this Task<Result> resultTask,
        Action<string> onSuccess,
        Func<IReadOnlyList<string>, Task> onFailureAsync)
    {
        var result = await resultTask;
        if (result.IsSuccess)
            onSuccess(result.SuccessMessage);
        else
            await onFailureAsync(result.ErrorMessages);
    }

    // 4. Non-generic, void return, both async
    public static async Task Match(
        this Task<Result> resultTask,
        Func<string, Task> onSuccessAsync,
        Func<IReadOnlyList<string>, Task> onFailureAsync)
    {
        var result = await resultTask;
        if (result.IsSuccess)
            await onSuccessAsync(result.SuccessMessage);
        else
            await onFailureAsync(result.ErrorMessages);
    }

    // 5. Generic, void return, both sync
    public static async Task Match<TIn>(
        this Task<Result<TIn>> resultTask,
        Action<string, TIn> onSuccess,
        Action<IReadOnlyList<string>> onFailure)
    {
        var result = await resultTask;
        if (result.IsSuccess)
            onSuccess(result.SuccessMessage, result.Data);
        else
            onFailure(result.ErrorMessages);
    }

    // 6. Generic, void return, async onSuccess, sync onFailure
    public static async Task Match<TIn>(
        this Task<Result<TIn>> resultTask,
        Func<string, TIn, Task> onSuccessAsync,
        Action<IReadOnlyList<string>> onFailure)
    {
        var result = await resultTask;
        if (result.IsSuccess)
            await onSuccessAsync(result.SuccessMessage, result.Data);
        else
            onFailure(result.ErrorMessages);
    }

    // 7. Generic, void return, sync onSuccess, async onFailure
    public static async Task Match<TIn>(
        this Task<Result<TIn>> resultTask,
        Action<string, TIn> onSuccess,
        Func<IReadOnlyList<string>, Task> onFailureAsync)
    {
        var result = await resultTask;
        if (result.IsSuccess)
            onSuccess(result.SuccessMessage, result.Data);
        else
            await onFailureAsync(result.ErrorMessages);
    }

    // 8. Non-generic, void return, both async
    public static async Task Match<TIn>(
        this Task<Result<TIn>> resultTask,
        Func<string, TIn, Task> onSuccessAsync,
        Func<IReadOnlyList<string>, Task> onFailureAsync)
    {
        var result = await resultTask;
        if (result.IsSuccess)
            await onSuccessAsync(result.SuccessMessage, result.Data);
        else
            await onFailureAsync(result.ErrorMessages);
    }

    // PaginatedResult
    // 1. Generic, void return, both sync
    public static async Task Match<TIn>(
        this Task<PaginatedResult<TIn>> resultTask,
        Action<string, PaginatedResult<TIn>> onSuccess,
        Action<IReadOnlyList<string>> onFailure)
    {
        var result = await resultTask;
        if (result.IsSuccess)
            onSuccess(result.SuccessMessage, result);
        else
            onFailure(result.ErrorMessages);
    }
}
