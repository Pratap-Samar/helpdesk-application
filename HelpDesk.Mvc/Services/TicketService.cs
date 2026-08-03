using System.Text.Json;
using HelpDesk.Mvc.Models;

namespace HelpDesk.Mvc.Services;

public class TicketService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;
    private readonly JsonSerializerOptions _jsonOptions;

    public TicketService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiBaseUrl = configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5210";
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<List<Ticket>> GetAllTicketsAsync()
    {
        var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/ticket");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Ticket>>(json, _jsonOptions) ?? new List<Ticket>();
    }

    public async Task<Ticket?> GetTicketByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/ticket/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
        
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Ticket>(json, _jsonOptions);
    }

    public async Task<Ticket?> CreateTicketAsync(Ticket ticket)
    {
        var json = JsonSerializer.Serialize(ticket, _jsonOptions);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/ticket", content);
        response.EnsureSuccessStatusCode();
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Ticket>(responseJson, _jsonOptions);
    }

    public async Task<Ticket?> UpdateTicketAsync(int id, Ticket ticket)
    {
        var json = JsonSerializer.Serialize(ticket, _jsonOptions);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{_apiBaseUrl}/api/ticket/{id}", content);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
        
        response.EnsureSuccessStatusCode();
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Ticket>(responseJson, _jsonOptions);
    }

    public async Task<bool> DeleteTicketAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/api/ticket/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return false;
        
        response.EnsureSuccessStatusCode();
        return true;
    }

    public async Task<List<Ticket>> GetTicketsByStatusAsync(string status)
    {
        var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/ticket/status/{Uri.EscapeDataString(status)}");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Ticket>>(json, _jsonOptions) ?? new List<Ticket>();
    }

    public async Task<int> GetTotalTicketsCountAsync()
    {
        var tickets = await GetAllTicketsAsync();
        return tickets.Count;
    }

    public async Task<int> GetOpenTicketsCountAsync()
    {
        var tickets = await GetTicketsByStatusAsync("Open");
        return tickets.Count;
    }

    public async Task<int> GetClosedTicketsCountAsync()
    {
        var tickets = await GetTicketsByStatusAsync("Closed");
        return tickets.Count;
    }
}