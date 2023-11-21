using System.Data;
using CleanBlazor.Shared.Wrapper;

namespace CleanBlazor.Application.Abstractions.Infrastructure.Services;

public interface IExcelService
{
    Task<string> ExportAsync<TData>(
        IEnumerable<TData> data,
        Dictionary<string, Func<TData, object>> mappers,
        string sheetName = "Sheet1");

    Task<Result<IEnumerable<TEntity>>> ImportAsync<TEntity>(
        Stream data,
        Dictionary<string, Func<DataRow, TEntity, object>> mappers,
        string sheetName = "Sheet1");
}
