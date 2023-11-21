using Blazored.FluentValidation;
using CleanBlazor.Client.Extensions;
using CleanBlazor.Contracts;
using CleanBlazor.Shared.Enums;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Polly.Caching;

namespace CleanBlazor.Client.Shared.Components;

public partial class ImportExcelModal
{
    private IBrowserFile _file;

    private FluentValidationValidator _fluentValidationValidator;
    private bool _uploading;
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    [Parameter] public UploadRequest UploadRequest { get; set; } = new();
    [Parameter] public string ModelName { get; set; }
    [Parameter] public Func<UploadRequest, Task<Result<int>>> OnSaved { get; set; }
    private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

    public void Cancel() => MudDialog.Cancel();

    private async Task SaveAsync()
    {
        if (OnSaved != null)
        {
            _uploading = true;
            
            await OnSaved.Invoke(UploadRequest)
                .Match((message, _) =>
                    {
                        SnackBar.Success(message);
                        MudDialog.Close();
                    },
                    errors => SnackBar.Error(errors));

            _uploading = false;
        }
        else
        {
            MudDialog.Close();
        }

        await Task.CompletedTask;
    }

    private async Task UploadFiles(InputFileChangeEventArgs e)
    {
        _file = e.File;
        if (_file != null)
        {
            var buffer = new byte[_file.Size];
            var extension = Path.GetExtension(_file.Name);
            await _file.OpenReadStream(_file.Size).ReadAsync(buffer);
            UploadRequest = new UploadRequest
            {
                Data = buffer, FileName = _file.Name, UploadType = UploadType.Document, Extension = extension
            };
        }
    }

    protected override async Task OnInitializedAsync() => await LoadDataAsync();

    private static async Task LoadDataAsync() => await Task.CompletedTask;
}
