using BlazorHero.CleanArchitecture.Application.Responses.Audit;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Utilities
{
    public partial class AuditTrails
    {
        public List<RelatedAuditTrail> Trails = new List<RelatedAuditTrail>();
        private RelatedAuditTrail Trail = new RelatedAuditTrail();
        private string searchString = "";

        private bool Search(AuditResponse response)
        {
            if (string.IsNullOrWhiteSpace(searchString)) return true;
            if (response.TableName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        protected override async Task OnInitializedAsync()
        {
            await GetDataAsync();
        }

        private async Task GetDataAsync()
        {
            var response = await _auditManager.GetCurrentUserTrailsAsync();
            if (response.Succeeded)
            {
                Trails = response.Data
                    .Select(x => new RelatedAuditTrail
                    {
                        AffectedColumns = x.AffectedColumns,
                        DateTime = x.DateTime,
                        Id = x.Id,
                        NewValues = x.NewValues,
                        OldValues = x.OldValues,
                        PrimaryKey = x.PrimaryKey,
                        TableName = x.TableName,
                        Type = x.Type,
                        UserId = x.UserId,
                        LocalTime = DateTime.SpecifyKind(x.DateTime, DateTimeKind.Utc).ToLocalTime()
                    }).ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(localizer[message], Severity.Error);
                }
            }
        }

        private void ShowBtnPress(int id)
        {
            Trail = Trails.First(f => f.Id == id);
            foreach (var trial in Trails.Where(a => a.Id != id))
            {
                trial.ShowDetails = false;
            }
            Trail.ShowDetails = !Trail.ShowDetails;
        }

        private async Task ExportToExcelAsync()
        {
            var base64 = await _auditManager.DownloadFileAsync();
            await _jsRuntime.InvokeVoidAsync("Download", new
            {
                ByteArray = base64,
                FileName = $"audit_trails_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            });
        }

        public class RelatedAuditTrail : AuditResponse
        {
            public bool ShowDetails { get; set; } = false;
            public DateTime LocalTime { get; set; }
        }
    }
}