using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Identity
{
    public partial class UserProfile
    {
        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public string Title { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Title = "Mukesh";
        }
    }
}