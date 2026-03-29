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
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync("api/rapport/statistik");
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Kunde inte nå Rapport-API:et: {ex.Message}", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new InvalidOperationException("Tidsgräns överskriden vid anrop till Rapport-API.", ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            var felmeddelande = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Rapport-API svarade med fel ({(int)response.StatusCode}): {felmeddelande}");
        }

        var json = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<RapportStatistik>(json, options);
    }
}