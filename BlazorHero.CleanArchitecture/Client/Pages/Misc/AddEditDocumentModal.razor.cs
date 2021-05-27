using System;
using BlazorHero.CleanArchitecture.Application.Features.Documents.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Requests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.IO;
using System.Threading.Tasks;
using Blazored.FluentValidation;

namespace BlazorHero.CleanArchitecture.Client.Pages.Misc
{
    public partial class AddEditDocumentModal
    {
        [Inject] private Microsoft.Extensions.Localization.IStringLocalizer<AddEditDocumentModal> localizer { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        [Parameter]
        public AddEditDocumentCommand AddEditDocumentModel { get; set; } = new();

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await _documentManager.SaveAsync(AddEditDocumentModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            await Task.CompletedTask;
        }

        public IBrowserFile file { get; set; }

        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            file = e.File;
            if (file != null)
            {
                var buffer = new byte[file.Size];
                var extension = Path.GetExtension(file.Name);
                var format = "application/octet-stream";
                await file.OpenReadStream(file.Size).ReadAsync(buffer);
                AddEditDocumentModel.URL = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditDocumentModel.UploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Document, Extension = extension };
            }
        }
    }
}