using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SOS100.Models;



namespace SOS100.Controllers;

public class ApprovalController : Controller
{
    private readonly HttpClient _httpClient;

    
    private const string ApprovalsApiUrl = "http://localhost:5130/api/approvals";
    private const string ApplicationsApiUrl = "http://localhost:5050/api/Applications";
    private const string CommentsApiUrl = "http://localhost:5030/Comments";
    private const string ServiceStatusApiUrl = "http://localhost:5030/ServiceStatus";

    public ApprovalController()
    {
        _httpClient = new HttpClient();
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var applications = await _httpClient.GetFromJsonAsync<List<ApplicationApiItem>>(ApplicationsApiUrl)
                               ?? new List<ApplicationApiItem>();

            var comments = await _httpClient.GetFromJsonAsync<List<CommentApiItem>>(CommentsApiUrl)
                           ?? new List<CommentApiItem>();

            var statuses = await _httpClient.GetFromJsonAsync<List<ServiceStatusApiItem>>(ServiceStatusApiUrl)
                           ?? new List<ServiceStatusApiItem>();

        
            var items = applications.Select(app =>
            {
                var status = statuses.FirstOrDefault(s => s.ID == app.Id);
                var latestComment = comments
                    .Where(c => c.StatusOBJ == (status?.ID ?? app.Id))
                    .OrderByDescending(c => c.ID)
                    .FirstOrDefault();

                return new ManagerApprovalItem
                {
                    StatusId = status?.ID ?? app.Id,
                    ApplicationId = app.Id,
                    EmployeeName = app.EmployeeName,
                    BenefitName = app.BenefitName,
                    ApplicationMessage = app.Message,
                    Status = status?.Status ?? app.Status ?? "Pending",
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
        // 1. Uppdatera Axel's status-API
        var statusResponse = await _httpClient.PutAsJsonAsync(
            $"{ServiceStatusApiUrl}/{statusId}/status",
            newStatus
        );

        // 2. Skapa admin-kommentar i Axel's kommentar-API
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
