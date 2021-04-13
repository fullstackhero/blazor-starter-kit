using BlazorHero.CleanArchitecture.Application.Specifications.Base;
using BlazorHero.CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Specifications
{
    public class DocumentFilterSpecification : HeroSpecification<Document>
    {
        public DocumentFilterSpecification(string searchString, string userId)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p =>  (p.Title.Contains(searchString) || p.Description.Contains(searchString)) && ( p.IsPublic == true || (p.IsPublic==false && p.CreatedBy == userId));
            }
            else
            {
                Criteria = p => (p.IsPublic == true || (p.IsPublic == false && p.CreatedBy == userId));
            }
        }
    }
}
