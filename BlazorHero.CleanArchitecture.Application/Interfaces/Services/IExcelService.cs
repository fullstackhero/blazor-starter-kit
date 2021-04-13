using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services
{
    public interface IExcelService
    {
        Task<string> ExportAsync<TData>(IEnumerable<TData> data
            , string[] headers = null
            , string sheetName = "Sheet1"
            , string[] columnIgnore = null
            , Dictionary<string, Func<TData, object>> converters = null);
    }
}
