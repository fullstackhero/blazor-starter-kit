using System.Collections.Generic;
using BlazorHero.CleanArchitecture.UserService.Interfaces.Responses;

namespace BlazorHero.CleanArchitecture.Application.Responses.Identity
{
    public class GetAllUsersResponse
    {
        public IEnumerable<UserResponse> Users { get; set; }
    }
}