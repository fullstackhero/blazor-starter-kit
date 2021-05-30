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
            var response = await _dashboardManager.GetDataAsync();
            if (response.Succeeded)
            {
                ProductCount = response.Data.ProductCount;
                BrandCount = response.Data.BrandCount;
                UserCount = response.Data.UserCount;
                RoleCount = response.Data.RoleCount;
                foreach (var item in response.Data.DataEnterBarChart)
                {
                    DataEnterBarChartSeries.Add(new ChartSeries { Name = item.Name, Data = item.Data });
                }
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        [CascadingParameter] public HubConnection hubConnection { get; set; }
    }
}