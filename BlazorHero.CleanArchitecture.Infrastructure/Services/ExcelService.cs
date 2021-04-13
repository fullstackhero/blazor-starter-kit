using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services
{
    public class ExcelService : IExcelService
    {
        public async Task<string> ExportAsync<TData>(IEnumerable<TData> data
            , string[] headers = null
            , string sheetName = "Sheet1"
            , string[] valueIgnore = null
            , Dictionary<string, Func<TData, object>> converters = null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var p = new ExcelPackage();
            p.Workbook.Properties.Author = "BlazorHero";
            p.Workbook.Worksheets.Add("Audit Trails");
            var ws = p.Workbook.Worksheets[0];
            ws.Name = sheetName;
            ws.Cells.Style.Font.Size = 11;
            ws.Cells.Style.Font.Name = "Calibri";

            var colIndex = 1;
            var rowIndex = 1;
            var props = typeof(TData).GetProperties();

            headers ??= props.Select(x => x.Name.ToString()).ToArray();

            foreach (var item in headers)
            {
                var cell = ws.Cells[rowIndex, colIndex];

                var fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;

                var border = cell.Style.Border;
                border.Bottom.Style =
                    border.Top.Style =
                        border.Left.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;

                cell.Value = item;

                colIndex++;
            }

            foreach (var item in data)
            {
                colIndex = 1;
                rowIndex++;
                foreach (var propInfo in props)
                {
                    if (valueIgnore != null && valueIgnore.Contains(propInfo.Name))
                    {
                        continue;
                    }

                    var value = converters != null && converters.TryGetValue(propInfo.Name, out var funcConvert) 
                        ? funcConvert(item)
                        : propInfo.GetValue(item);
   
                    ws.Cells[rowIndex, colIndex++].Value = value;
                }
            }

            var byteArray = await p.GetAsByteArrayAsync();
            return Convert.ToBase64String(byteArray);
        }
    }
}
