using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity;
using BlazorHero.CleanArchitecture.Shared.Models.Identity;
using BlazorHero.CleanArchitecture.Shared.Responses.Identity;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services.Identity
{
    public class UserService : IUserService
    {
        private readonly UserManager<BlazorHeroUser> _userManager;

        public UserService(UserManager<BlazorHeroUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        private IMapper _mapper;
        public async Task<Result<GetAllUsersReponse>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var model = _mapper.Map<IEnumerable<UserResponse>>(users);
            var result = new GetAllUsersReponse { Users = model };
            return Result<GetAllUsersReponse>.Success(result);
        }
    }
}
