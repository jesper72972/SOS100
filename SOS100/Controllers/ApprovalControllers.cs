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
    public async Task<IActionResult> Approve(int id, string comment)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5130/api/approvals/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }

        var approval = await response.Content.ReadFromJsonAsync<Approval>();

        if (approval == null)
        {
            return RedirectToAction("Index");
        }

        approval.Decision = "Approved";
        approval.Comment = comment;
        approval.DecisionDate = DateTime.UtcNow;

        await _httpClient.PutAsJsonAsync($"http://localhost:5130/api/approvals/{id}", approval);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Reject(int id, string comment)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5130/api/approvals/{id}");

        if (!response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }

        var approval = await response.Content.ReadFromJsonAsync<Approval>();

        if (approval == null)
        {
            return RedirectToAction("Index");
        }

        approval.Decision = "Rejected";
        approval.Comment = comment;
        approval.DecisionDate = DateTime.UtcNow;

        await _httpClient.PutAsJsonAsync($"http://localhost:5130/api/approvals/{id}", approval);

        return RedirectToAction("Index");
    }
}
