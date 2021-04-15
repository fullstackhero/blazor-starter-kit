using BlazorHero.CleanArchitecture.Application.Features.Documents.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Requests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Misc
{
    public partial class AddEditDocumentModal
    {
        private bool success;
        private string[] errors = { };
        private MudForm form;

        [Parameter]
        public int Id { get; set; }

        [Parameter]
        [Required]
        public string Title { get; set; }

        [Parameter]
        [Required]
        public string URL { get; set; }

        [Parameter]
        public bool IsPublic { get; set; }

        [Parameter]
        [Required]
        public string Description { get; set; }

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            form.Validate();
            if (form.IsValid)
            {
                var request = new AddEditDocumentCommand() { Title = Title, Description = Description, Id = Id, URL = URL, IsPublic = IsPublic, UploadRequest = UploadRequest };
                var response = await _documentManager.SaveAsync(request);
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
            await Task.CompletedTask;
        }

        public IBrowserFile file { get; set; }

        [Parameter]
        public UploadRequest UploadRequest { get; set; }

        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            file = e.File;
            if (file != null)
            {
                var buffer = new byte[file.Size];
                var extension = Path.GetExtension(file.Name);
                var document = await e.File.OpenReadStream().ReadAsync(buffer);
                UploadRequest = new UploadRequest() { Data = buffer, UploadType = Application.Enums.UploadType.Document, Extension = extension };
            }
        }
    }
}