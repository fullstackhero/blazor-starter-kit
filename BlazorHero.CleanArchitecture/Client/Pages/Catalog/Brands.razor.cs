using BlazorHero.CleanArchitecture.Application.Features.Brands.Queries.GetAll;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Catalog
{
    public partial class Brands
    {
        public List<GetAllBrandsResponse> BrandList = new List<GetAllBrandsResponse>();
        private GetAllBrandsResponse brand = new GetAllBrandsResponse();
        private string searchString = "";
        protected override async Task OnInitializedAsync()
        {
            await GetBrandsAsync();
        }
        private async Task GetBrandsAsync()
        {
            var response = await _brandManager.GetAllAsync();
            if (response.Succeeded)
            {
                BrandList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(localizer[message], Severity.Error);
                }
            }
        }

        private async Task Delete(int id)
        {
            string deleteContent = localizer["Delete Content"];
            var parameters = new DialogParameters();
            parameters.Add("ContentText", string.Format(deleteContent, id));
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>("Delete", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await _brandManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    await Reset();
                    _snackBar.Add(localizer[response.Messages[0]], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(localizer[message], Severity.Error);
                    }
                }
            }
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                brand = BrandList.FirstOrDefault(c => c.Id == id);
                parameters.Add("Id", brand.Id);
                parameters.Add("Name", brand.Name);
                parameters.Add("Description", brand.Description);
                parameters.Add("Tax", brand.Tax);
            }
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditBrandModal>("Modal", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            brand = new GetAllBrandsResponse();
            await GetBrandsAsync();
        }

        private bool Search(GetAllBrandsResponse brand)
        {
            if (string.IsNullOrWhiteSpace(searchString)) return true;
            if (brand.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
    }
}