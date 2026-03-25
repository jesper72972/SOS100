using Microsoft.AspNetCore.Mvc;
using RapportFrontend.Models;
using System.Text;
using System.Text.Json;

namespace RapportFrontend.Controllers
{
    public class RapportController : Controller
    {
        private readonly HttpClient _httpClient;

        public RapportController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("http://localhost:5160/api/rapport");

            var json = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<List<Rapport>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Rapport rapport)
        {
            var json = JsonSerializer.Serialize(rapport);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _httpClient.PostAsync("http://localhost:5160/api/rapport", content);

            return RedirectToAction("Index");
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Rapport>> GetById(int id)
        {
            var rapport = await _context.Rapporter.FindAsync(id);

            if (rapport == null)
                return NotFound();

            return rapport;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var rapport = await _context.Rapporter.FindAsync(id);

            if (rapport == null)
                return NotFound();

            _context.Rapporter.Remove(rapport);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}