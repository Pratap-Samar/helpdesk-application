using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HelpDesk.Mvc.Models;
using HelpDesk.Mvc.Services;

namespace HelpDesk.Mvc.Controllers;

public class TicketsController : Controller
{
    private readonly TicketService _ticketService;

    public TicketsController(TicketService ticketService)
    {
        _ticketService = ticketService;
    }

    // GET: Tickets
    public async Task<IActionResult> Index()
    {
        var tickets = await _ticketService.GetAllTicketsAsync();
        return View(tickets);
    }

    // GET: Tickets/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var ticket = await _ticketService.GetTicketByIdAsync(id);
        if (ticket == null)
            return NotFound();

        return View(ticket);
    }

    // GET: Tickets/Create
    public IActionResult Create()
    {
        ViewBag.Priorities = GetPriorityList();
        return View();
    }

    // POST: Tickets/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Ticket ticket)
    {
        if (ModelState.IsValid)
        {
            ticket.Status = "Open"; // Hardcoded as per requirements
            await _ticketService.CreateTicketAsync(ticket);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Priorities = GetPriorityList();
        return View(ticket);
    }

    // GET: Tickets/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var ticket = await _ticketService.GetTicketByIdAsync(id);
        if (ticket == null)
            return NotFound();

        ViewBag.Priorities = GetPriorityList();
        ViewBag.Statuses = GetStatusList();
        return View(ticket);
    }

    // POST: Tickets/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Ticket ticket)
    {
        if (id != ticket.Id)
            return BadRequest();

        if (ModelState.IsValid)
        {
            await _ticketService.UpdateTicketAsync(id, ticket);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Priorities = GetPriorityList();
        ViewBag.Statuses = GetStatusList();
        return View(ticket);
    }

    // GET: Tickets/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var ticket = await _ticketService.GetTicketByIdAsync(id);
        if (ticket == null)
            return NotFound();

        return View(ticket);
    }

    // POST: Tickets/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _ticketService.DeleteTicketAsync(id);
        return RedirectToAction(nameof(Index));
    }

    // GET: Tickets/Filter
    public async Task<IActionResult> Filter(string status)
    {
        ViewBag.Statuses = GetStatusList();
        ViewBag.SelectedStatus = status;

        if (string.IsNullOrEmpty(status))
        {
            return View(new List<Ticket>());
        }

        var tickets = await _ticketService.GetTicketsByStatusAsync(status);
        return View(tickets);
    }

    private List<SelectListItem> GetPriorityList()
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Value = "Low", Text = "Low" },
            new SelectListItem { Value = "Medium", Text = "Medium" },
            new SelectListItem { Value = "High", Text = "High" },
            new SelectListItem { Value = "Critical", Text = "Critical" }
        };
    }

    private List<SelectListItem> GetStatusList()
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Value = "Open", Text = "Open" },
            new SelectListItem { Value = "In Progress", Text = "In Progress" },
            new SelectListItem { Value = "Closed", Text = "Closed" }
        };
    }
}