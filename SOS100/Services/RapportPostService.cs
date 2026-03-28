using System.Text;
using System.Text.Json;
using SOS100.Models;

namespace SOS100.Services;

public class RapportPostService
{
    private readonly HttpClient _httpClient;

    public RapportPostService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<RapportPostDto>> GetAllAsync()
    {
        var response = await _httpClient.GetAsync("api/RapportPost");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<RapportPostDto>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<RapportPostDto>();
    }

    public async Task<RapportPostDto> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/RapportPost/{id}");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<RapportPostDto>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task CreateAsync(RapportPostDto dto)
    {
        var json = JsonSerializer.Serialize(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("api/RapportPost", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAsync(RapportPostDto dto)
    {
        var json = JsonSerializer.Serialize(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"api/RapportPost/{dto.Id}", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/RapportPost/{id}");
        response.EnsureSuccessStatusCode();
    }
}