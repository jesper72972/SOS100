using Microsoft.AspNetCore.Mvc;
using SOS100.Models;
using System.Net.Http.Json;

namespace SOS100.Controllers;

public class ApplicationsController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _applicationApiUrl;
    private readonly string _statusFormanerApiUrl;

    public ApplicationsController(IHttpClientFactory factory, IConfiguration config)
    {
        _httpClient = factory.CreateClient();
        _applicationApiUrl   = $"{config["ApiUrls:ApplicationApi"]}/api/applications";
        _statusFormanerApiUrl = $"{config["ApiUrls:StatusFormanerApi"]}/ServiceStatus";
    }

    // GET: /Applications
    public async Task<IActionResult> Index()
    {
        var data = await _httpClient.GetFromJsonAsync<List<Application>>(_applicationApiUrl);
        return View(data);
    }

    // GET: /Applications/Create
    public IActionResult Create(string benefit)
    {
        var model = new Application { BenefitName = benefit };
        return View(model);
    }

    // POST: /Applications/Create
    [HttpPost]
    public async Task<IActionResult> Create(Application application)
    {
        application.Status = "Pending";

        var response = await _httpClient.PostAsJsonAsync(_applicationApiUrl, application);

        var createdApp = await response.Content.ReadFromJsonAsync<Application>();
        if (createdApp != null)
        {
            var serviceStatus = new
            {
                ServicID = createdApp.Id,
                Status = "Pending",
                Name = createdApp.EmployeeName
            };

            await _httpClient.PostAsJsonAsync(_statusFormanerApiUrl, serviceStatus);
        }

        return RedirectToAction("Forman", "Forman");
    }
}