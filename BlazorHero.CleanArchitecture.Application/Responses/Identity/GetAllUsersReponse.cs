using System.Collections.Generic;

namespace BlazorHero.CleanArchitecture.Application.Responses.Identity
{
    public class GetAllUsersReponse
    {
        public IEnumerable<UserResponse> Users { get; set; }
    }
}