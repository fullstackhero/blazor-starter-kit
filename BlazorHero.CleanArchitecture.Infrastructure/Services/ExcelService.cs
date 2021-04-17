using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services
{
    public class ExcelService : IExcelService
    {
        private readonly IStringLocalizer<ExcelService> _localizer;

        public ExcelService(IStringLocalizer<ExcelService> localizer)
        {
            _localizer = localizer;
        }

        public async Task<string> ExportAsync<TData>(IEnumerable<TData> data
            , Dictionary<string, Func<TData, object>> mappers
            , string sheetName = "Sheet1")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var p = new ExcelPackage();
            p.Workbook.Properties.Author = "BlazorHero";
            p.Workbook.Worksheets.Add(_localizer["Audit Trails"]);
            var ws = p.Workbook.Worksheets[0];
            ws.Name = sheetName;
            ws.Cells.Style.Font.Size = 11;
            ws.Cells.Style.Font.Name = "Calibri";

            var colIndex = 1;
            var rowIndex = 1;

            var headers = mappers.Keys.Select(x => x).ToList();

            foreach (var header in headers)
            {
                var cell = ws.Cells[rowIndex, colIndex];

                var fill = cell.Style.Fill;
                fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.LightBlue);

                var border = cell.Style.Border;
                border.Bottom.Style =
                    border.Top.Style =
                        border.Left.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;

                cell.Value = header;

                colIndex++;
            }

            foreach (var item in data)
            {
                colIndex = 1;
                rowIndex++;

                var result = headers.Select(header => mappers[header](item));

                foreach (var value in result)
                {
                    ws.Cells[rowIndex, colIndex++].Value = value;
                }
            }

            var byteArray = await p.GetAsByteArrayAsync();
            return Convert.ToBase64String(byteArray);
        }
    }
}