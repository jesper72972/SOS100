using System.Text.Json;
using RapportAPI.Models;

namespace RapportAPI.Services;

public class RapportService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public RapportService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<object> HamtaStatistikAsync()
    {
        var formanerApiUrl = _configuration["ApiUrls:FormanerApi"];
        var applicationsApiUrl = _configuration["ApiUrls:ApplicationsApi"];
        var godkannandenApiUrl = _configuration["ApiUrls:GodkannandenApi"];

        if (string.IsNullOrWhiteSpace(formanerApiUrl) ||
            string.IsNullOrWhiteSpace(applicationsApiUrl) ||
            string.IsNullOrWhiteSpace(godkannandenApiUrl))
        {
            throw new Exception("API-URL:er saknas i appsettings.");
        }

        var formanerResponse = await _httpClient.GetAsync($"{formanerApiUrl}/Formaner");
        var applicationsResponse = await _httpClient.GetAsync($"{applicationsApiUrl}/api/Applications");
        var approvalsResponse = await _httpClient.GetAsync($"{godkannandenApiUrl}/api/Approvals");

        formanerResponse.EnsureSuccessStatusCode();
        applicationsResponse.EnsureSuccessStatusCode();
        approvalsResponse.EnsureSuccessStatusCode();

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var formanerJson = await formanerResponse.Content.ReadAsStringAsync();
        var applicationsJson = await applicationsResponse.Content.ReadAsStringAsync();
        var approvalsJson = await approvalsResponse.Content.ReadAsStringAsync();

        var formaner = JsonSerializer.Deserialize<List<ExternFormanDto>>(formanerJson, jsonOptions) ?? new List<ExternFormanDto>();
        var applications = JsonSerializer.Deserialize<List<ExternAnsokanDto>>(applicationsJson, jsonOptions) ?? new List<ExternAnsokanDto>();
        var approvals = JsonSerializer.Deserialize<List<ExternGodkannandeDto>>(approvalsJson, jsonOptions) ?? new List<ExternGodkannandeDto>();

        var godkandaApplicationIds = approvals
            .Where(a => a.Decision == "Approved")
            .Select(a => a.ApplicationId)
            .ToHashSet();

        var statistikPerForman = formaner
            .Select(f => new
            {
                namn = f.Title,
                kategori = f.categorie,
                antalAnsokningar = applications.Count(a => a.BenefitName == f.Title),
                antalGodkanda = applications.Count(a => a.BenefitName == f.Title && godkandaApplicationIds.Contains(a.Id))
            })
            .ToList();

        var totaltAntalFormaner = formaner.Count;
        var totaltAntalAnsokningar = applications.Count;
        var totaltAntalGodkanda = applications.Count(a => godkandaApplicationIds.Contains(a.Id));

        return new
        {
            totaltAntalFormaner,
            totaltAntalAnsokningar,
            totaltAntalGodkanda,
            statistikPerForman
        };
    }
}