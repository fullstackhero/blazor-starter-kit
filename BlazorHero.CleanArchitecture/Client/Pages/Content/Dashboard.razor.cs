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
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [Parameter] public int ProductCount { get; set; }
        [Parameter] public int BrandCount { get; set; }
        [Parameter] public int UserCount { get; set; }
        [Parameter] public int RoleCount { get; set; }

        private readonly string[] _dataEnterBarChartXAxisLabels = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        private readonly List<ChartSeries> _dataEnterBarChartSeries = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            HubConnection = new HubConnectionBuilder()
            .WithUrl(_navigationManager.ToAbsoluteUri(ApplicationConstants.SignalR.HubUrl))
            .Build();
            HubConnection.On(ApplicationConstants.SignalR.ReceiveUpdateDashboard, async () =>
            {
                await LoadDataAsync();
                StateHasChanged();
            });
            await HubConnection.StartAsync();
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
                    _dataEnterBarChartSeries.Add(new ChartSeries { Name = item.Name, Data = item.Data });
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
    }
}