using Microsoft.AspNetCore.Mvc;
using Application_API.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace SOS100.Controllers;

public class ApplicationsController : Controller
{
    private readonly HttpClient _httpClient;

    public ApplicationsController(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient();
    }

    // GET: /Applications
    public async Task<IActionResult> Index()
    {
        var data = await _httpClient.GetFromJsonAsync<List<Application>>(
            "http://localhost:5050/api/applications"
        );

        return View(data);
    }

    // GET: /Applications/Create
    public IActionResult Create(string benefit)
    {
        var model = new Application
        {
            BenefitName = benefit
        };

        return View(model);
    }

    // POST: /Applications/Create
    [HttpPost]
    public async Task<IActionResult> Create(Application application)
    {
        application.Status = "Pending";

        var response = await _httpClient.PostAsJsonAsync(
            "http://localhost:5050/api/applications",
            application
        );

        var createdApp = await response.Content.ReadFromJsonAsync<Application>();
        if (createdApp != null)
        {
            var serviceStatus = new
            {
                ServicID = createdApp.Id,
                Status = "Pending",
                Name = createdApp.EmployeeName
            };

            await _httpClient.PostAsJsonAsync(
                "http://localhost:5030/ServiceStatus",
                serviceStatus
            );
        }

        return RedirectToAction("Index");
    }
}