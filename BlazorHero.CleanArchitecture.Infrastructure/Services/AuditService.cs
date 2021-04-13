using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Models.Audit;
using BlazorHero.CleanArchitecture.Application.Responses.Audit;
using BlazorHero.CleanArchitecture.Infrastructure.Contexts;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services
{
    public class AuditService : IAuditService
    {
        private readonly BlazorHeroContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;

        public AuditService(IMapper mapper, BlazorHeroContext context, IExcelService excelService)
        {
            _mapper = mapper;
            _context = context;
            _excelService = excelService;
        }

        public async Task<IResult<IEnumerable<AuditResponse>>> GetCurrentUserTrailsAsync(string userId)
        {
            var trails = await _context.AuditTrails.Where(a => a.UserId == userId).OrderByDescending(a => a.Id).Take(250).ToListAsync();
            var mappedLogs = _mapper.Map<List<AuditResponse>>(trails);
            return Result<IEnumerable<AuditResponse>>.Success(mappedLogs);
        }

        public async Task<string> ExportToExcelAsync(string userId)
        {
            var trails = await _context.AuditTrails.Where(x => x.UserId == userId)
                .OrderByDescending(a => a.DateTime).ToListAsync();
            var result = await _excelService.ExportAsync(trails, sheetName: "Audit trails",
                mappers: new Dictionary<string, Func<Audit, object>>()
                {
                    { "Table Name", item => item.TableName },
                    { "Type", item => item.Type },
                    { "Date Time (Local)", item => DateTime.SpecifyKind(item.DateTime, DateTimeKind.Utc).ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss") },
                    { "Date Time (UTC)", item => item.DateTime.ToString("dd/MM/yyyy HH:mm:ss") },
                    { "Primary Key", item => item.PrimaryKey },
                    { "Old Values", item => item.OldValues },
                    { "New Values", item => item.NewValues },
                });

            return result;
        }
    }
}