using Microsoft.AspNetCore.Mvc;
using HelpDesk.Api.Models;
using HelpDesk.Api.Repositories;

namespace HelpDesk.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    private readonly ITicketRepository _repository;

    public TicketController(ITicketRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Ticket>>> GetAllTickets()
    {
        var tickets = await _repository.GetAllTicketsAsync();
        return Ok(tickets);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Ticket>> GetTicketById(int id)
    {
        var ticket = await _repository.GetTicketByIdAsync(id);
        if (ticket == null)
            return NotFound();

        return Ok(ticket);
    }

    [HttpPost]
    public async Task<ActionResult<Ticket>> CreateTicket(Ticket ticket)
    {
        var createdTicket = await _repository.CreateTicketAsync(ticket);
        return CreatedAtAction(nameof(GetTicketById), new { id = createdTicket.Id }, createdTicket);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Ticket>> UpdateTicket(int id, Ticket ticket)
    {
        if (id != ticket.Id)
            return BadRequest();

        var updatedTicket = await _repository.UpdateTicketAsync(ticket);
        if (updatedTicket == null)
            return NotFound();

        return Ok(updatedTicket);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTicket(int id)
    {
        var result = await _repository.DeleteTicketAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<List<Ticket>>> GetTicketsByStatus(string status)
    {
        var tickets = await _repository.GetTicketsByStatusAsync(status);
        return Ok(tickets);
    }
}