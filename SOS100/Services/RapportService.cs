using System.Text.Json;
using SOS100.Models;

namespace SOS100.Services;

public class RapportService
{
    private readonly HttpClient _httpClient;

    public RapportService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<RapportStatistik?> GetStatistikAsync()
    {
        var response = await _httpClient.GetAsync("api/rapport/statistik");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<RapportStatistik>(json, options);
    }
}