using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;

namespace BlazorHero.CleanArchitecture.Client.Pages.Content
{
    public partial class Dashboard
    {
        [Parameter]
        public int ProductCount { get; set; }

        [Parameter]
        public int BrandCount { get; set; }

        [Parameter]
        public int UserCount { get; set; }

        [Parameter]
        public int RoleCount { get; set; }

        public string[] DataEnterBarChartXAxisLabels = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        public List<ChartSeries> DataEnterBarChartSeries = new List<ChartSeries>();


        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            hubConnection = new HubConnectionBuilder()
            .WithUrl(_navigationManager.ToAbsoluteUri(ApplicationConstants.SignalR.HubUrl))
            .Build();
            hubConnection.On(ApplicationConstants.SignalR.ReceiveUpdateDashboard, async () =>
            {
                await LoadDataAsync();
                StateHasChanged();
            });
            await hubConnection.StartAsync();
        }

        private async Task LoadDataAsync()
        {
            var data = await _dashboardManager.GetDataAsync();
            if (data.Succeeded)
            {
                ProductCount = data.Data.ProductCount;
                BrandCount = data.Data.BrandCount;
                UserCount = data.Data.UserCount;
                RoleCount = data.Data.RoleCount;
                foreach (var item in data.Data.DataEnterBarChart)
                {
                    DataEnterBarChartSeries.Add(new ChartSeries { Name = localizer[item.Name], Data = item.Data });
                }
            }
        }

        [CascadingParameter] public HubConnection hubConnection { get; set; }
    }
}