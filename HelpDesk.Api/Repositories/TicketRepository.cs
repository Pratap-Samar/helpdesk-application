using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Models;
using HelpDesk.Api.Data;

namespace HelpDesk.Api.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly HelpDeskDbContext _context;

    public TicketRepository(HelpDeskDbContext context)
    {
        _context = context;
    }

    public async Task<List<Ticket>> GetAllTicketsAsync()
    {
        return await _context.Tickets
            .OrderByDescending(t => t.CreatedDate)
            .ToListAsync();
    }

    public async Task<Ticket?> GetTicketByIdAsync(int id)
    {
        return await _context.Tickets.FindAsync(id);
    }

    public async Task<Ticket> CreateTicketAsync(Ticket ticket)
    {
        ticket.CreatedDate = DateTime.UtcNow;
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();
        return ticket;
    }

    public async Task<Ticket?> UpdateTicketAsync(Ticket ticket)
    {
        var existingTicket = await _context.Tickets.FindAsync(ticket.Id);
        if (existingTicket == null)
            return null;

        existingTicket.Title = ticket.Title;
        existingTicket.Description = ticket.Description;
        existingTicket.Category = ticket.Category;
        existingTicket.Priority = ticket.Priority;
        existingTicket.Status = ticket.Status;
        existingTicket.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existingTicket;
    }

    public async Task<bool> DeleteTicketAsync(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
            return false;

        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Ticket>> GetTicketsByStatusAsync(string status)
    {
        return await _context.Tickets
            .Where(t => t.Status == status)
            .OrderByDescending(t => t.CreatedDate)
            .ToListAsync();
    }
}