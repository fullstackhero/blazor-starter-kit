using Blazored.FluentValidation;
using CleanBlazor.Client.Extensions;
using CleanBlazor.Contracts;
using CleanBlazor.Contracts.Catalog.Brands;
using CleanBlazor.Contracts.Catalog.Products;
using CleanBlazor.Shared.Constants.Application;
using CleanBlazor.Shared.Constants.Routes;
using CleanBlazor.Shared.Enums;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace CleanBlazor.Client.Pages.Catalog;

public partial class AddEditProductModal
{
    private List<GetAllBrandsResponse> _brands = new();

    private IBrowserFile _file;

    private FluentValidationValidator _fluentValidationValidator;

    [Parameter] public AddEditProductRequest AddEditProductModel { get; set; } = new();
    [CascadingParameter] private HubConnection HubConnection { get; set; }
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

    public void Cancel() => MudDialog.Cancel();

    private async Task SaveAsync() =>
        await HttpClient
            .PostAsJsonAsync<AddEditProductRequest, Result<int>>(ProductsEndpoints.Save, AddEditProductModel)
            .Match(async (message, _) =>
                {
                    SnackBar.Success(message);
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    MudDialog.Close();
                },
                errors => SnackBar.Error(errors));

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
        HubConnection = HubConnection.TryInitialize(NavigationManager, LocalStorage);
        if (HubConnection.State == HubConnectionState.Disconnected)
        {
            await HubConnection.StartAsync();
        }
    }

    private async Task LoadDataAsync()
    {
        await LoadImageAsync();
        await LoadBrandsAsync();
    }

    private async Task LoadBrandsAsync() =>
        await HttpClient.GetFromJsonAsync<Result<List<GetAllBrandsResponse>>>(BrandsEndpoints.GetAll)
            .Match((_, brands) => _brands = brands, _ => { });

    private async Task LoadImageAsync() =>
        await HttpClient.GetFromJsonAsync<Result<string>>(ProductsEndpoints.GetProductImage(AddEditProductModel.Id))
                .Match((_, imageData) =>
                {
                    if (!string.IsNullOrEmpty(imageData))
                    {
                        AddEditProductModel.ImageDataUrl = imageData;
                    }
                }, _ => { });

    private void DeleteAsync()
    {
        AddEditProductModel.ImageDataUrl = null;
        AddEditProductModel.UploadRequest = new UploadRequest();
    }

    private async Task UploadFiles(InputFileChangeEventArgs e)
    {
        _file = e.File;
        if (_file != null)
        {
            var extension = Path.GetExtension(_file.Name);
            const string format = "image/png";
            IBrowserFile imageFile = await e.File.RequestImageFileAsync(format, 400, 400);
            var buffer = new byte[imageFile.Size];
            await imageFile.OpenReadStream().ReadAsync(buffer);
            AddEditProductModel.ImageDataUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
            AddEditProductModel.UploadRequest =
                new UploadRequest { Data = buffer, UploadType = UploadType.Product, Extension = extension };
        }
    }

    private async Task<IEnumerable<int>> SearchBrands(string value)
    {
        // In real life use an asynchronous function for fetching data from an api.
        await Task.Delay(5);

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
        {
            return _brands.Select(x => x.Id);
        }

        return _brands.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
            .Select(x => x.Id);
    }
}
