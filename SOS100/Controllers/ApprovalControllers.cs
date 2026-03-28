using System.Net.Http.Json;
using System.Text.Json;
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
        var statusBase     = config["ApiUrls:StatusFormanerApi"];
        var applicationBase = config["ApiUrls:ApplicationApi"];

        _approvalsApiUrl    = $"{godkannadeBase}/api/approvals";
        _commentsApiUrl     = $"{statusBase}/Comments";
        _serviceStatusApiUrl = $"{statusBase}/ServiceStatus";
        _applicationsApiUrl  = $"{applicationBase}/api/applications";
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var comments = await _httpClient.GetFromJsonAsync<List<CommentApiItem>>(_commentsApiUrl)
                           ?? new List<CommentApiItem>();

            var statuses = await _httpClient.GetFromJsonAsync<List<ServiceStatusApiItem>>(_serviceStatusApiUrl)
                           ?? new List<ServiceStatusApiItem>();

            var applications = await _httpClient.GetFromJsonAsync<List<ApplicationApiItem>>(_applicationsApiUrl)
                               ?? new List<ApplicationApiItem>();

            var items = statuses.Select(status =>
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

            return View(items);
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Kunde inte hämta data från de andra tjänsterna: {ex.Message}";
            return View(new List<ManagerApprovalItem>());
        }
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
        await _httpClient.PutAsJsonAsync(
            $"{_serviceStatusApiUrl}/{statusId}/status",
            newStatus
        );

        if (!string.IsNullOrWhiteSpace(comment))
        {
            var commentPayload = new CommentApiItem
            {
                StatusOBJ = statusId,
                AdminComment = comment,
                UserCommemt = string.Empty
            };

            await _httpClient.PostAsJsonAsync(_commentsApiUrl, commentPayload);
        }

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

            await _httpClient.PostAsJsonAsync(_approvalsApiUrl, newApproval);
        }
        else
        {
            existingApproval.Decision = newStatus;
            existingApproval.Comment = comment ?? string.Empty;
            existingApproval.DecisionDate = DateTime.UtcNow;

            await _httpClient.PutAsJsonAsync(
                $"{_approvalsApiUrl}/{existingApproval.Id}",
                existingApproval
            );
        }
    }
}
