using System.Data;
using System.Drawing;
using CleanBlazor.Application.Abstractions.Infrastructure.Services;
using CleanBlazor.Shared.Wrapper;
using Microsoft.Extensions.Localization;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace CleanBlazor.Infrastructure.Services;

public class ExcelService : IExcelService
{
    private readonly IStringLocalizer<ExcelService> _localizer;

    public ExcelService(IStringLocalizer<ExcelService> localizer) => _localizer = localizer;

    public async Task<string> ExportAsync<TData>(
        IEnumerable<TData> data,
        Dictionary<string, Func<TData, object>> mappers,
        string sheetName = "Sheet1")
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var p = new ExcelPackage();
        p.Workbook.Properties.Author = "CleanBlazor";
        p.Workbook.Worksheets.Add(_localizer["Audit Trails"]);
        ExcelWorksheet ws = p.Workbook.Worksheets[0];
        ws.Name = sheetName;
        ws.Cells.Style.Font.Size = 11;
        ws.Cells.Style.Font.Name = "Calibri";

        var colIndex = 1;
        var rowIndex = 1;

        List<string> headers = mappers.Keys.Select(x => x).ToList();

        foreach (var header in headers)
        {
            ExcelRange cell = ws.Cells[rowIndex, colIndex];

            ExcelFill fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(Color.LightBlue);

            Border border = cell.Style.Border;
            border.Bottom.Style =
                border.Top.Style =
                    border.Left.Style =
                        border.Right.Style = ExcelBorderStyle.Thin;

            cell.Value = header;

            colIndex++;
        }

        List<TData> dataList = data.ToList();
        foreach (TData item in dataList)
        {
            colIndex = 1;
            rowIndex++;

            IEnumerable<object> result = headers.Select(header => mappers[header](item));

            foreach (var value in result)
            {
                ws.Cells[rowIndex, colIndex++].Value = value;
            }
        }

        using (ExcelRange autoFilterCells = ws.Cells[1, 1, dataList.Count + 1, headers.Count])
        {
            autoFilterCells.AutoFilter = true;
            autoFilterCells.AutoFitColumns();
        }

        var byteArray = await p.GetAsByteArrayAsync();
        return Convert.ToBase64String(byteArray);
    }

    public async Task<Result<IEnumerable<TEntity>>> ImportAsync<TEntity>(
        Stream data,
        Dictionary<string, Func<DataRow, TEntity, object>> mappers,
        string sheetName = "Sheet1")
    {
        var result = new List<TEntity>();
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var p = new ExcelPackage();
        data.Position = 0;
        await p.LoadAsync(data);
        ExcelWorksheet ws = p.Workbook.Worksheets[sheetName];
        if (ws == null)
        {
            return Result.Fail<IEnumerable<TEntity>>(string.Format(_localizer["Sheet with name {0} does not exist!"],
                sheetName));
        }

        var dt = new DataTable();
        foreach (ExcelRangeBase firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
        {
            dt.Columns.Add(firstRowCell.Text);
        }

        const int startRow = 2;
        List<string> headers = mappers.Keys.Select(x => x).ToList();
        var errors = mappers.Keys
            .Select(x => x)
            .Where(h => !dt.Columns.Contains(h))
            .Select(header => string.Format(_localizer["Header '{0}' does not exist in table!"], header))
            .ToList();

        if (errors.Any())
        {
            return Result.Fail<IEnumerable<TEntity>>(errors);
        }

        for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
        {
            try
            {
                ExcelRange wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                DataRow row = dt.Rows.Add();
                var item = (TEntity)Activator.CreateInstance(typeof(TEntity));
                foreach (ExcelRangeBase cell in wsRow)
                {
                    row[cell.Start.Column - 1] = cell.Text;
                }

                headers.ForEach(x => mappers[x](row, item));
                result.Add(item);
            }
            catch (Exception e)
            {
                return Result.Fail<IEnumerable<TEntity>>(_localizer[e.Message]);
            }
        }

        return Result.Ok<IEnumerable<TEntity>>(result, _localizer["Import Success"]);
    }
}
