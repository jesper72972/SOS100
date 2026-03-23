using Microsoft.AspNetCore.Mvc;
using SOS100.Models;
using SOS100.Services;

namespace SOS100.Controllers;

public class FormanerStatusController : Controller
{
    private readonly FormanerStatusService _statusService;
    private readonly FormanService _formanService;

    public FormanerStatusController(FormanerStatusService statusService, FormanService formanService)
    {
        _statusService = statusService;
        _formanService = formanService;
    }

    public async Task<IActionResult> FormanerStatus(string? name)
    {
        var allStatuses = await _statusService.GetStatuses();
        var availableNames = allStatuses.Select(s => s.Name).Distinct().OrderBy(n => n).ToList();

        var filteredStatuses = string.IsNullOrEmpty(name)
            ? allStatuses
            : allStatuses.Where(s => s.Name == name).ToArray();

        var vm = new FormanerStatusViewModel
        {
            Statuses       = filteredStatuses,
            Comments       = await _statusService.GetComments(),
            Formaners      = await _formanService.GetFormans(),
            SelectedName   = name,
            AvailableNames = availableNames, 
        };
        return View(vm);
    }
    
       [HttpPost]
    public async Task<IActionResult> CreateUserComment(int statusOBJ, string userComment)
    {
        await _statusService.CreateComment(new Comment { StatusOBJ = statusOBJ, UserCommemt = userComment });
        return RedirectToAction("FormanerStatus");
    }

    [HttpPost]
    public async Task<IActionResult> CreateAdminComment(int statusOBJ, string adminComment)
    {
        await _statusService.CreateComment(new Comment { StatusOBJ = statusOBJ, AdminComment = adminComment });
        return RedirectToAction("FormanerStatus");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        await _statusService.UpdateStatus(id, status);
        return RedirectToAction("FormanerStatus");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteStatus(int id)
    {
        await _statusService.DeleteStatus(id);
        return RedirectToAction("FormanerStatus");
    }
}
