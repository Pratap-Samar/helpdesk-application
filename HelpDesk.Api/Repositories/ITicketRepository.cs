using HelpDesk.Api.Models;

namespace HelpDesk.Api.Repositories;

public interface ITicketRepository
{
    Task<List<Ticket>> GetAllTicketsAsync();
    Task<Ticket?> GetTicketByIdAsync(int id);
    Task<Ticket> CreateTicketAsync(Ticket ticket);
    Task<Ticket?> UpdateTicketAsync(Ticket ticket);
    Task<bool> DeleteTicketAsync(int id);
    Task<List<Ticket>> GetTicketsByStatusAsync(string status);
}