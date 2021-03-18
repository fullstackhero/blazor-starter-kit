using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Dashboard.GetData
{
    public class DashboardDataResponse
    {
        public int ProductCount { get; set; }
        public int BrandCount { get; set; }
        public int UserCount { get; set; }
        public int RoleCount { get; set; }
    }
}
