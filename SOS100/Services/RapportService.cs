using System.Net.Http.Json;
using SOS100.Models;

namespace SOS100.Services;

public class RapportService
{
    private readonly HttpClient _httpClient;

    public RapportService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<RapportStatistik> HamtaRapportStatistik()
    {
        try
        {
            var resultat = await _httpClient.GetFromJsonAsync<RapportStatistik>("api/rapport/statistik");
            return resultat ?? new RapportStatistik();
        }
        catch
        {
            return new RapportStatistik();
        }
    }
}