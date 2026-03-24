using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SOS100.Models;

namespace SOS100.Controllers;

public class ApprovalController : Controller
{
    private readonly HttpClient _httpClient;

    public ApprovalController()
    {
        _httpClient = new HttpClient();
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("http://localhost:5130/api/approvals");

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Kunde inte hämta godkännanden från API.";
            return View(new List<Approval>());
        }

        var json = await response.Content.ReadAsStringAsync();

        var approvals = JsonSerializer.Deserialize<List<Approval>>(json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return View(approvals ?? new List<Approval>());
    }

    [HttpPost]
    public async Task<IActionResult> Approve(int applicationId)
    {
        var approval = new Approval
        {
            ApplicationId = applicationId,
            ApproverId = 101,
            Decision = "Approved",
            Comment = "Approved via frontend",
            DecisionDate = DateTime.UtcNow
        };

        var json = JsonSerializer.Serialize(approval);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        await _httpClient.PostAsync("http://localhost:5130/api/approvals", content);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Reject(int applicationId)
    {
        var approval = new Approval
        {
            ApplicationId = applicationId,
            ApproverId = 101,
            Decision = "Rejected",
            Comment = "Rejected via frontend",
            DecisionDate = DateTime.UtcNow
        };

        var json = JsonSerializer.Serialize(approval);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        await _httpClient.PostAsync("http://localhost:5130/api/approvals", content);

        return RedirectToAction("Index");
    }
}
