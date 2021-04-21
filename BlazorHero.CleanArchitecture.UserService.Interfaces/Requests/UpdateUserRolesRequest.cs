using System.Collections.Generic;
using BlazorHero.CleanArchitecture.UserService.Interfaces.Responses;

namespace BlazorHero.CleanArchitecture.UserService.Interfaces.Requests
{
    public class UpdateUserRolesRequest
    {
        public string UserId { get; set; }
        public IList<UserRoleModel> UserRoles { get; set; }
    }
}