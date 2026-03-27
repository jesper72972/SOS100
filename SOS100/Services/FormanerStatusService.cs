using SOS100.Models;
using System.Net.Http.Json;

namespace SOS100.Services;

public class FormanerStatusService
{
    private readonly HttpClient _httpClient;

    public FormanerStatusService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ServiceStatus[]> GetStatuses()
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<ServiceStatus[]>("ServiceStatus");
            return result ?? Array.Empty<ServiceStatus>();
        }
        catch (HttpRequestException)
        {
            return Array.Empty<ServiceStatus>();
        }
    }

    public async Task<Comment[]> GetComments()
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<Comment[]>("Comments");
            return result ?? Array.Empty<Comment>();
        }
        catch (HttpRequestException)
        {
            return Array.Empty<Comment>();
        }
    }

    public async Task CreateComment(Comment comment)
    {
        await _httpClient.PostAsJsonAsync("Comments", comment);
    }

    public async Task UpdateComment(Comment comment)
    {
        await _httpClient.PutAsJsonAsync($"Comments/{comment.ID}", comment);
    }

    public async Task UpdateStatus(int id, string newStatus)
    {
        await _httpClient.PutAsJsonAsync($"ServiceStatus/{id}/status", newStatus);
    }

    public async Task DeleteStatus(int id)
    {
        await _httpClient.DeleteAsync($"Comments/byStatus/{id}");
        await _httpClient.DeleteAsync($"ServiceStatus/{id}");
    }
}
