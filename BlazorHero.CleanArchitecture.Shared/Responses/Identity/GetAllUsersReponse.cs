using System.Collections.Generic;

namespace BlazorHero.CleanArchitecture.Shared.Responses.Identity
{
    public class GetAllUsersReponse
    {
        public IEnumerable<UserResponse> Users { get; set; }
    }
}