using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity
{
    public interface IAuthService
    {
        Task<Result> Register(RegisterRequest model);

        Task<Result> Login(LoginRequest model);

        Task Logout();
    }
}
