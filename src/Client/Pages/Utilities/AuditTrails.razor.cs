using System.Security.Claims;
using CleanBlazor.Client.Extensions;
using CleanBlazor.Contracts.Audit;
using CleanBlazor.Shared.Constants.Application;
using CleanBlazor.Shared.Constants.Permission;
using CleanBlazor.Shared.Constants.Routes;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using MudBlazor;

namespace CleanBlazor.Client.Pages.Utilities;

public partial class AuditTrails
{
    private bool _bordered;
    private bool _canExportAuditTrails;
    private bool _canSearchAuditTrails;

    private ClaimsPrincipal _currentUser;
    private DateRange _dateRange;
    private MudDateRangePicker _dateRangePicker;
    private bool _dense = true;
    private bool _loaded;
    private bool _searchInNewValues;
    private bool _searchInOldValues;
    private string _searchString = "";
    private bool _striped = true;

    private RelatedAuditTrail _trail = new();

    public List<RelatedAuditTrail> Trails = new();

    private bool Search(AuditResponse response)
    {
        var result = string.IsNullOrWhiteSpace(_searchString);

        // check Search String

        if (!result)
        {
            if (response.TableName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                result = true;
            }

            if (_searchInOldValues &&
                response.OldValues?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                result = true;
            }

            if (_searchInNewValues &&
                response.NewValues?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                result = true;
            }
        }

        // check Date Range
        if (_dateRange?.Start == null && _dateRange?.End == null)
        {
            return result;
        }

        if (_dateRange?.Start != null && response.DateTime < _dateRange.Start)
        {
            result = false;
        }

        if (_dateRange?.End != null && response.DateTime > _dateRange.End + new TimeSpan(0, 11, 59, 59, 999))
        {
            result = false;
        }

        return result;
    }

    protected override async Task OnInitializedAsync()
    {
        _currentUser = await StateProvider.GetCurrentUserAsync();
        _canExportAuditTrails =
            (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.AuditTrails.Export)).Succeeded;
        _canSearchAuditTrails =
            (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.AuditTrails.Search)).Succeeded;

        await GetDataAsync();
        _loaded = true;
    }

    private async Task GetDataAsync() =>
        await HttpClient.GetFromJsonAsync<Result<IEnumerable<AuditResponse>>>(AuditEndpoints.GetCurrentUserTrails)
            .Match((_, auditTrails) =>
                {
                    Trails = auditTrails.Select(x => new RelatedAuditTrail
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
                        LocalTime = x.DateTime.DateTime.ToLocalTime()
                    }).ToList();
                },
                errors => SnackBar.Error(errors));

    private void ShowBtnPress(int id)
    {
        _trail = Trails.First(f => f.Id == id);
        foreach (RelatedAuditTrail trial in Trails.Where(a => a.Id != id))
        {
            trial.ShowDetails = false;
        }

        _trail.ShowDetails = !_trail.ShowDetails;
    }

    private async Task ExportToExcelAsync()
    {
        var endpoint = string.IsNullOrWhiteSpace(_searchString)
            ? AuditEndpoints.DownloadFile
            : AuditEndpoints.DownloadFileFiltered(_searchString, _searchInOldValues, _searchInNewValues);
        await HttpClient.GetFromJsonAsync<Result<string>>(endpoint)
            .Match(async (_, base64Data) =>
                {
                    await JsRuntime.InvokeVoidAsync("Download",
                        new
                        {
                            ByteArray = base64Data,
                            FileName = $"{nameof(AuditTrails).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                            MimeType = ApplicationConstants.MimeTypes.OpenXml
                        });
                    SnackBar.Success(string.IsNullOrWhiteSpace(_searchString)
                        ? Localizer["Audit Trails exported"]
                        : Localizer["Filtered Audit Trails exported"]);
                },
                errors => SnackBar.Error(errors));
    }

    public class RelatedAuditTrail : AuditResponse
    {
        public bool ShowDetails { get; set; }
        public DateTime LocalTime { get; set; }
    }
}
