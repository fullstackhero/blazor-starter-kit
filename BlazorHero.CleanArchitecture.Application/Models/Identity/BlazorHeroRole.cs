using System;
using BlazorHero.CleanArchitecture.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace BlazorHero.CleanArchitecture.Application.Models.Identity
{
    public class BlazorHeroRole : IdentityRole, IAuditableEntity
    {
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }

        public BlazorHeroRole() : base()
        {
        }

        public BlazorHeroRole(string roleName, string roleDescription = null) : base(roleName)
        {
            Description = roleDescription;
        }
    }
}
