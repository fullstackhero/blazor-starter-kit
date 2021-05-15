using System.Collections.Generic;

namespace BlazorHero.CleanArchitecture.Application.Responses.Identity
{
    public class GetAllUsersResponse
    {
        public IEnumerable<UserResponse> Users { get; set; }
    }
}