using SOS100.Models;
using Microsoft.AspNetCore.Http.Features;
using System.Net.Http.Json;

namespace SOS100.Services;

public class FormanService
{ 
    private HttpClient _httpClient;
    
    public FormanService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Formaner[]> GetFormans()
    {
        var result = await _httpClient.GetFromJsonAsync<Formaner[]>("Formaner");
        if (result == null)
        { 
            return result ?? Array.Empty<Formaner>(); 
        } 
        
        return result;
    }
    
    public async Task CreateForman(Formaner formaner)
    {
        await _httpClient.PostAsJsonAsync("Formaner", formaner);
    }

    public async Task EditForman(Formaner formaner)
    {
        await _httpClient.PostAsJsonAsync("Formaner/Edit", formaner);
    }

    public async Task DeleteForman(int ID)
    {
        await _httpClient.DeleteAsync($"Formaner/{ID}");
    }
    
}