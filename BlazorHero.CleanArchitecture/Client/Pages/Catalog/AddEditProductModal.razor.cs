using BlazorHero.CleanArchitecture.Application.Features.Brands.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.Products.Commands.AddEdit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Catalog
{
    public partial class AddEditProductModal 
    {
        private bool success;
        private string[] errors = { };
        private MudForm form;

        [Parameter]
        public int Id { get; set; }

        [Parameter]
        [Required]
        public string Name { get; set; }

        [Parameter]
        [Required]
        public string Barcode { get; set; }

        [Parameter]
        [Required]
        public string Description { get; set; }

        [Parameter]
        [Required]
        public string Brand { get; set; }

        [Parameter]
        [Required]
        public int BrandId { get; set; }

        [Parameter]
        [Required]
        public decimal Rate { get; set; }

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        private List<GetAllBrandsResponse> Brands = new List<GetAllBrandsResponse>();

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            form.Validate();
            if (form.IsValid)
            {
                //TODO: Try to integrate validation with Mudblazor component - Select
                if (BrandId == 0)
                {
                    _snackBar.Add("Select a Brand.", Severity.Error);
                    return;
                }
                var request = new AddEditProductCommand() { Name = Name, Barcode = Barcode, BrandId = BrandId, Description = Description, ImageDataURL = ImageDataUrl, Rate = Rate, Id = Id };
                var response = await _productManager.SaveAsync(request);
                if (response.Succeeded)
                {
                    _snackBar.Add(localizer[response.Messages[0]], Severity.Success);
                    MudDialog.Close();
                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(localizer[message], Severity.Error);
                    }
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            await LoadImageAsync();
            await LoadBrandsAsync();
        }

        private async Task LoadBrandsAsync()
        {
            var data = await _brandManager.GetAllAsync();
            if (data.Succeeded)
            {
                Brands = data.Data;
            }
        }

        private async Task LoadImageAsync()
        {
            var data = await _productManager.GetProductImageAsync(Id);
            if (data.Succeeded)
            {
                var imageData = data.Data;
                if (!string.IsNullOrEmpty(imageData))
                {
                    ImageDataUrl = imageData;
                }
            }
        }

        private void DeleteAsync()
        {
            ImageDataUrl = null;
        }

        public IBrowserFile file { get; set; }

        [Parameter]
        public string ImageDataUrl { get; set; }

        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            file = e.File;
            if (file != null)
            {
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 500, 500);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                ImageDataUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
            }
        }
    }
}