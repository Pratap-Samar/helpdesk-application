using Microsoft.AspNetCore.Mvc;
using HelpDesk.Mvc.Services;

namespace HelpDesk.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly TicketService _ticketService;

    public HomeController(TicketService ticketService)
    {
        _ticketService = ticketService;
    }

    public async Task<IActionResult> Index()
    {
        var totalTickets = await _ticketService.GetTotalTicketsCountAsync();
        var openTickets = await _ticketService.GetOpenTicketsCountAsync();
        var closedTickets = await _ticketService.GetClosedTicketsCountAsync();

        ViewBag.TotalTickets = totalTickets;
        ViewBag.OpenTickets = openTickets;
        ViewBag.ClosedTickets = closedTickets;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}