using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Responses.Identity
{
    public class RoleClaimsResponse
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }
}
