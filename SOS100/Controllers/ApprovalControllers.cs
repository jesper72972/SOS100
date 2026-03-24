

using System.Net.Http.Json;
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
    public async Task<IActionResult> Approve(int id)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5130/api/approvals/{id}");

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Kunde inte hämta godkännandet för uppdatering.";
            return RedirectToAction("Index");
        }

        var approval = await response.Content.ReadFromJsonAsync<Approval>();

        if (approval == null)
        {
            ViewBag.Error = "Godkännandet kunde inte hittas.";
            return RedirectToAction("Index");
        }

        approval.Decision = "Approved";
        approval.Comment = "Approved via frontend";
        approval.DecisionDate = DateTime.UtcNow;

        var putResponse = await _httpClient.PutAsJsonAsync(
            $"http://localhost:5130/api/approvals/{id}",
            approval
        );

        if (!putResponse.IsSuccessStatusCode)
        {
            ViewBag.Error = "Kunde inte uppdatera godkännandet.";
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Reject(int id)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5130/api/approvals/{id}");

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Kunde inte hämta godkännandet för uppdatering.";
            return RedirectToAction("Index");
        }

        var approval = await response.Content.ReadFromJsonAsync<Approval>();

        if (approval == null)
        {
            ViewBag.Error = "Godkännandet kunde inte hittas.";
            return RedirectToAction("Index");
        }

        approval.Decision = "Rejected";
        approval.Comment = "Rejected via frontend";
        approval.DecisionDate = DateTime.UtcNow;

        var putResponse = await _httpClient.PutAsJsonAsync(
            $"http://localhost:5130/api/approvals/{id}",
            approval
        );

        if (!putResponse.IsSuccessStatusCode)
        {
            ViewBag.Error = "Kunde inte uppdatera godkännandet.";
        }

        return RedirectToAction("Index");
    }
}
