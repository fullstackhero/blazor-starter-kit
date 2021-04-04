using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Responses.Audit;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services
{
    public class AuditService : IAuditService
    {
        public Task<IResult<IEnumerable<AuditResponse>>> GetUserTrailsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> SaveTrailAsync()
        {
            throw new NotImplementedException();
        }
    }
}
