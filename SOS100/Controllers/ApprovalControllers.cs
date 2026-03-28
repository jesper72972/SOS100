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

    private const string ApprovalsApiUrl = "http://localhost:5130/api/approvals";
    private const string CommentsApiUrl = "http://localhost:5030/Comments";
    private const string ServiceStatusApiUrl = "http://localhost:5030/ServiceStatus";
    private const string ApplicationsApiUrl = "http://localhost:5050/api/applications";

    public ApprovalController()
    {
        _httpClient = new HttpClient();
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var comments = await _httpClient.GetFromJsonAsync<List<CommentApiItem>>(CommentsApiUrl)
                           ?? new List<CommentApiItem>();

            var statuses = await _httpClient.GetFromJsonAsync<List<ServiceStatusApiItem>>(ServiceStatusApiUrl)
                           ?? new List<ServiceStatusApiItem>();

            var applications = await _httpClient.GetFromJsonAsync<List<ApplicationApiItem>>(ApplicationsApiUrl)
                               ?? new List<ApplicationApiItem>();

            var items = statuses.Select(status =>
            {
                var latestComment = comments
                    .Where(c => c.StatusOBJ == status.ID)
                    .OrderByDescending(c => c.ID)
                    .FirstOrDefault();

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
                    UserComment = latestComment?.UserCommemt ?? string.Empty
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
            $"{ServiceStatusApiUrl}/{statusId}/status",
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

            await _httpClient.PostAsJsonAsync(CommentsApiUrl, commentPayload);
        }

        var approvals = await _httpClient.GetFromJsonAsync<List<Approval>>(ApprovalsApiUrl)
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

            await _httpClient.PostAsJsonAsync(ApprovalsApiUrl, newApproval);
        }
        else
        {
            existingApproval.Decision = newStatus;
            existingApproval.Comment = comment ?? string.Empty;
            existingApproval.DecisionDate = DateTime.UtcNow;

            await _httpClient.PutAsJsonAsync(
                $"{ApprovalsApiUrl}/{existingApproval.Id}",
                existingApproval
            );
        }
    }
}
