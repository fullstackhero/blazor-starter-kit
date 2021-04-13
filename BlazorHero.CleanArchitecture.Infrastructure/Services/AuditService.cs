using System;
using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Responses.Audit;
using BlazorHero.CleanArchitecture.Infrastructure.Contexts;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Models.Audit;

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

        public async Task<string> ExportToExcelAsync()
        {
            var trails = await _context.AuditTrails.OrderByDescending(a => a.DateTime).ToListAsync();
            var result = await _excelService.ExportAsync(trails, sheetName: "Audit trails", headers: new[]
            {
                "Table Name",
                "Type",
                "Actor",
                "Date Time (Local)",
                "Date Time (UTC)",
                "Primary Key",
                "Old Values",
                "New Values"
            }, columnIgnore: new []
            {
                "Id",
                "AffectedColumns"
            }, converters: new Dictionary<string, Func<Audit, object>>()
            {
                {
                    "Date Time (Local)", item => item.DateTime
                }
            });

            return result;
        }
    }
}
