using MudBlazor;
using System.Collections.Generic;

namespace BlazorHero.CleanArchitecture.Application.Features.Dashboard.GetData
{
    public class DashboardDataResponse
    {
        public int ProductCount { get; set; }
        public int BrandCount { get; set; }
        public int UserCount { get; set; }
        public int RoleCount { get; set; }
        public List<ChartSeries> DataEnterBarChart { get; set; } = new List<ChartSeries>();
        public Dictionary<string, double> ProductByBrandTypePieChart { get; set; }
    }
}