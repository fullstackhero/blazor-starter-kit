using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Shared.Responses.Identity
{
    public class GetAllUsersReponse
    {
        public IEnumerable<UserResponse> Users { get; set; }

    }
}
