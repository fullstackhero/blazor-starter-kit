using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity
{
    public interface ICurrentUserService
    {
        string UserId { get; }
    }
}
