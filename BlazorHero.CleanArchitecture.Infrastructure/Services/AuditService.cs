using System;
using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Responses.Audit;
using BlazorHero.CleanArchitecture.Infrastructure.Contexts;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services
{
    public class AuditService : IAuditService
    {
        private readonly BlazorHeroContext _context;
        private readonly IMapper _mapper;

        public AuditService(IMapper mapper, BlazorHeroContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IResult<IEnumerable<AuditResponse>>> GetCurrentUserTrailsAsync(string userId)
        {
            var trails = await _context.AuditTrails.Where(a => a.UserId == userId).OrderByDescending(a => a.Id).Take(250).ToListAsync();
            var mappedLogs = _mapper.Map<List<AuditResponse>>(trails);
            return Result<IEnumerable<AuditResponse>>.Success(mappedLogs);
        }

        public async Task<byte[]> ExportToExcelAsync()
        {
            var trails = await _context.AuditTrails.OrderByDescending(a => a.DateTime).ToListAsync();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                using var p = new ExcelPackage();
                p.Workbook.Properties.Author = "BlazorHero";
                p.Workbook.Worksheets.Add("Audit Trails");
                var ws = p.Workbook.Worksheets[0];
                ws.Name = "Audit Trails";
                ws.Cells.Style.Font.Size = 11;
                ws.Cells.Style.Font.Name = "Calibri";
                string[] arrColumnHeader = {
                    "Table Name",
                    "Type",
                    "Actor",
                    "Date Time (Local)",
                    "Date Time (UTC)",
                    "Primary Key",
                    "Old Values",
                    "New Values"
                };

                var colIndex = 1;
                var rowIndex = 1;

                foreach (var item in arrColumnHeader)
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


                foreach (var item in trails)
                {
                    colIndex = 1;
                    rowIndex++;
                    ws.Cells[rowIndex, colIndex++].Value = item.TableName;
                    ws.Cells[rowIndex, colIndex++].Value = item.Type;
                    ws.Cells[rowIndex, colIndex++].Value = item.UserId;
                    ws.Cells[rowIndex, colIndex++].Value = item.DateTime;
                    ws.Cells[rowIndex, colIndex++].Value = item.DateTime;
                    ws.Cells[rowIndex, colIndex++].Value = item.PrimaryKey;
                    ws.Cells[rowIndex, colIndex++].Value = item.OldValues;
                    ws.Cells[rowIndex, colIndex++].Value = item.NewValues;
                }

                return await p.GetAsByteArrayAsync();
                //await File.WriteAllBytesAsync("test.xlsx", bin);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
