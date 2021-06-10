using System;
using System.Collections.Generic;
using BlazorHero.CleanArchitecture.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace BlazorHero.CleanArchitecture.Infrastructure.Models.Identity
{
    public class BlazorHeroRole : IdentityRole, IAuditableEntity<string>
    {
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public virtual ICollection<BlazorHeroRoleClaim> RoleClaims { get; set; }

        public BlazorHeroRole() : base()
        {
            RoleClaims = new HashSet<BlazorHeroRoleClaim>();
        }

        public BlazorHeroRole(string roleName, string roleDescription = null) : base(roleName)
        {
            RoleClaims = new HashSet<BlazorHeroRoleClaim>();
            Description = roleDescription;
        }
    }
}