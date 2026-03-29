using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOS100.Models;

namespace SOS100.Controllers;

[Authorize]
public class ApprovalController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _approvalsApiUrl;
    private readonly string _commentsApiUrl;
    private readonly string _serviceStatusApiUrl;
    private readonly string _applicationsApiUrl;

    public ApprovalController(IConfiguration config)
    {
        _httpClient = new HttpClient();

        var godkannadeBase = config["ApiUrls:GodkannadeApi"];
        var statusBase = config["ApiUrls:StatusFormanerApi"];
        var applicationBase = config["ApiUrls:ApplicationApi"];

        _approvalsApiUrl = $"{godkannadeBase}/api/approvals";
        _commentsApiUrl = $"{statusBase}/Comments";
        _serviceStatusApiUrl = $"{statusBase}/ServiceStatus";
        _applicationsApiUrl = $"{applicationBase}/api/applications";
    }

    public async Task<IActionResult> Index()
    {
        var errors = new List<string>();

        List<CommentApiItem> comments = new();
        List<ServiceStatusApiItem> statuses = new();
        List<ApplicationApiItem> applications = new();

        try
        {
            comments = await _httpClient.GetFromJsonAsync<List<CommentApiItem>>(_commentsApiUrl)
                       ?? new List<CommentApiItem>();
        }
        catch
        {
            errors.Add("Comments API fungerade inte.");
        }

        try
        {
            statuses = await _httpClient.GetFromJsonAsync<List<ServiceStatusApiItem>>(_serviceStatusApiUrl)
                       ?? new List<ServiceStatusApiItem>();
        }
        catch
        {
            errors.Add("Status API fungerade inte.");
        }

        try
        {
            applications = await _httpClient.GetFromJsonAsync<List<ApplicationApiItem>>(_applicationsApiUrl)
                           ?? new List<ApplicationApiItem>();
        }
        catch
        {
            errors.Add("Applications API fungerade inte.");
        }

        var items = new List<ManagerApprovalItem>();

        if (statuses.Any())
        {
            items = statuses.Select(status =>
            {
                var statusComments = comments
                    .Where(c => c.StatusOBJ == status.ID)
                    .OrderBy(c => c.ID)
                    .ToList();

                var latestComment = statusComments.LastOrDefault();

                var hasUnread = latestComment != null
                    && !string.IsNullOrWhiteSpace(latestComment.UserCommemt)
                    && string.IsNullOrWhiteSpace(latestComment.AdminComment);

                var application = applications.FirstOrDefault(a => a.Id == status.ServicID);

                return new ManagerApprovalItem
                {
                    StatusId = status.ID,
                    ApplicationId = status.ServicID,
                    EmployeeName = application?.EmployeeName ?? status.Name,
                    BenefitName = application?.BenefitName ?? "Förmån",
                    ApplicationMessage = application?.Message ?? string.Empty,
                    Status = string.IsNullOrWhiteSpace(status.Status) ? "Pending" : status.Status,
                    AdminComment = latestComment?.AdminComment ?? string.Empty,
                    UserComment = latestComment?.UserCommemt ?? string.Empty,
                    AllComments = statusComments,
                    HasUnreadUserComment = hasUnread
                };
            }).ToList();
        }
        else if (applications.Any())
        {
            items = applications.Select(app => new ManagerApprovalItem
            {
                StatusId = app.Id,
                ApplicationId = app.Id,
                EmployeeName = app.EmployeeName,
                BenefitName = app.BenefitName,
                ApplicationMessage = app.Message ?? string.Empty,
                Status = string.IsNullOrWhiteSpace(app.Status) ? "Pending" : app.Status,
                AdminComment = string.Empty,
                UserComment = string.Empty,
                AllComments = new List<CommentApiItem>(),
                HasUnreadUserComment = false
            }).ToList();
        }

        ViewBag.Errors = errors;

        return View(items);
    }

    [HttpPost]
    public async Task<IActionResult> Approve(int statusId, int applicationId, string comment)
    {
        await UpdateStatusAndComment(statusId, applicationId, "Approved", comment);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Reject(int statusId, int applicationId, string comment)
    {
        await UpdateStatusAndComment(statusId, applicationId, "Rejected", comment);
        return RedirectToAction("Index");
    }

    private async Task UpdateStatusAndComment(int statusId, int applicationId, string newStatus, string comment)
    {
        var warnings = new List<string>();

        // 1. Spara alltid approval först
        try
        {
            var approvals = await _httpClient.GetFromJsonAsync<List<Approval>>(_approvalsApiUrl)
                            ?? new List<Approval>();

            var existingApproval = approvals.FirstOrDefault(a => a.ApplicationId == applicationId);

            if (existingApproval == null)
            {
                var newApproval = new Approval
                {
                    ApplicationId = applicationId,
                    ApproverId = 101,
                    Decision = newStatus,
                    Comment = comment ?? string.Empty,
                    DecisionDate = DateTime.UtcNow
                };

                var approvalResponse = await _httpClient.PostAsJsonAsync(_approvalsApiUrl, newApproval);

                if (!approvalResponse.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Kunde inte spara beslutet i Godkännande API.";
                    return;
                }
            }
            else
            {
                existingApproval.Decision = newStatus;
                existingApproval.Comment = comment ?? string.Empty;
                existingApproval.DecisionDate = DateTime.UtcNow;

                var approvalResponse = await _httpClient.PutAsJsonAsync(
                    $"{_approvalsApiUrl}/{existingApproval.Id}",
                    existingApproval
                );

                if (!approvalResponse.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Kunde inte uppdatera beslutet i Godkännande API.";
                    return;
                }
            }
        }
        catch
        {
            TempData["Error"] = "Kunde inte spara beslutet i Godkännande API.";
            return;
        }

        // 2. Försök uppdatera status
        try
        {
            var statusResponse = await _httpClient.PutAsJsonAsync(
                $"{_serviceStatusApiUrl}/{statusId}/status",
                newStatus
            );

            if (!statusResponse.IsSuccessStatusCode)
            {
                warnings.Add("Status API fungerade inte.");
            }
        }
        catch
        {
            warnings.Add("Status API fungerade inte.");
        }

        // 3. Försök spara kommentar
        if (!string.IsNullOrWhiteSpace(comment))
        {
            try
            {
                var commentPayload = new CommentApiItem
                {
                    StatusOBJ = statusId,
                    AdminComment = comment,
                    UserCommemt = string.Empty
                };

                var commentResponse = await _httpClient.PostAsJsonAsync(_commentsApiUrl, commentPayload);

                if (!commentResponse.IsSuccessStatusCode)
                {
                    warnings.Add("Comments API fungerade inte.");
                }
            }
            catch
            {
                warnings.Add("Comments API fungerade inte.");
            }
        }

        if (warnings.Any())
        {
            TempData["Warning"] = $"Beslutet sparades, men: {string.Join(" ", warnings)}";
        }
        else
        {
            TempData["Success"] = "Beslutet sparades.";
        }
    }
}
