using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using HelpDesk.Api.Controllers;
using HelpDesk.Api.Models;
using HelpDesk.Api.Repositories;

namespace HelpDesk.Tests;

public class TicketControllerTests
{
    private readonly Mock<ITicketRepository> _mockRepository;
    private readonly TicketController _controller;

    public TicketControllerTests()
    {
        _mockRepository = new Mock<ITicketRepository>();
        _controller = new TicketController(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAllTickets_ReturnsOkResult_WithListOfTickets()
    {
        // Arrange
        var tickets = new List<Ticket>
        {
            new Ticket { Id = 1, Title = "Ticket 1", Description = "Desc 1", Category = "Software", Priority = "High", Status = "Open", CreatedDate = System.DateTime.UtcNow },
            new Ticket { Id = 2, Title = "Ticket 2", Description = "Desc 2", Category = "Hardware", Priority = "Medium", Status = "Closed", CreatedDate = System.DateTime.UtcNow }
        };
        _mockRepository.Setup(repo => repo.GetAllTicketsAsync()).ReturnsAsync(tickets);

        // Act
        var result = await _controller.GetAllTickets();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTickets = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
        Assert.Equal(2, returnedTickets.Count);
    }

    [Fact]
    public async Task GetAllTickets_ReturnsEmptyList_WhenNoTicketsExist()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetAllTicketsAsync()).ReturnsAsync(new List<Ticket>());

        // Act
        var result = await _controller.GetAllTickets();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTickets = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
        Assert.Empty(returnedTickets);
    }

    [Fact]
    public async Task GetTicketById_ReturnsOkResult_WithTicket()
    {
        // Arrange
        var ticket = new Ticket { Id = 1, Title = "Test Ticket", Description = "Description", Category = "Software", Priority = "High", Status = "Open", CreatedDate = System.DateTime.UtcNow };
        _mockRepository.Setup(repo => repo.GetTicketByIdAsync(1)).ReturnsAsync(ticket);

        // Act
        var result = await _controller.GetTicketById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTicket = Assert.IsType<Ticket>(okResult.Value);
        Assert.Equal(1, returnedTicket.Id);
        Assert.Equal("Test Ticket", returnedTicket.Title);
    }

    [Fact]
    public async Task GetTicketById_ReturnsNotFound_WhenTicketDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetTicketByIdAsync(999)).ReturnsAsync((Ticket?)null);

        // Act
        var result = await _controller.GetTicketById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateTicket_ReturnsCreatedAtAction_WithTicket()
    {
        // Arrange
        var newTicket = new Ticket { Title = "New Ticket", Description = "New Description", Category = "Network", Priority = "Critical", Status = "Open" };
        var createdTicket = new Ticket { Id = 1, Title = "New Ticket", Description = "New Description", Category = "Network", Priority = "Critical", Status = "Open", CreatedDate = System.DateTime.UtcNow };
        _mockRepository.Setup(repo => repo.CreateTicketAsync(It.IsAny<Ticket>())).ReturnsAsync(createdTicket);

        // Act
        var result = await _controller.CreateTicket(newTicket);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(_controller.GetTicketById), createdAtActionResult.ActionName);
        var returnedTicket = Assert.IsType<Ticket>(createdAtActionResult.Value);
        Assert.Equal(1, returnedTicket.Id);
        Assert.Equal("New Ticket", returnedTicket.Title);
    }

    [Fact]
    public async Task UpdateTicket_ReturnsOkResult_WhenUpdateIsSuccessful()
    {
        // Arrange
        var existingTicket = new Ticket { Id = 1, Title = "Original", Description = "Desc", Category = "Software", Priority = "High", Status = "Open", CreatedDate = System.DateTime.UtcNow };
        var updatedTicket = new Ticket { Id = 1, Title = "Updated", Description = "Updated Desc", Category = "Hardware", Priority = "Critical", Status = "In Progress", CreatedDate = System.DateTime.UtcNow, UpdatedDate = System.DateTime.UtcNow };
        _mockRepository.Setup(repo => repo.UpdateTicketAsync(It.IsAny<Ticket>())).ReturnsAsync(updatedTicket);

        // Act
        var result = await _controller.UpdateTicket(1, updatedTicket);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTicket = Assert.IsType<Ticket>(okResult.Value);
        Assert.Equal("Updated", returnedTicket.Title);
        Assert.Equal("In Progress", returnedTicket.Status);
        Assert.Equal("Critical", returnedTicket.Priority);
    }

    [Fact]
    public async Task UpdateTicket_ReturnsNotFound_WhenTicketDoesNotExist()
    {
        // Arrange
        var updatedTicket = new Ticket { Id = 999, Title = "Updated", Description = "Desc", Category = "Software", Priority = "High", Status = "Open" };
        _mockRepository.Setup(repo => repo.UpdateTicketAsync(It.IsAny<Ticket>())).ReturnsAsync((Ticket?)null);

        // Act
        var result = await _controller.UpdateTicket(999, updatedTicket);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task UpdateTicket_ReturnsBadRequest_WhenIdMismatch()
    {
        // Arrange
        var ticket = new Ticket { Id = 2, Title = "Test", Description = "Desc", Category = "Software", Priority = "High", Status = "Open" };

        // Act
        var result = await _controller.UpdateTicket(1, ticket);

        // Assert
        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public async Task DeleteTicket_ReturnsOkResult_WhenTicketIsDeletedSuccessfully()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.DeleteTicketAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteTicket(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteTicket_ReturnsNotFound_WhenTicketDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.DeleteTicketAsync(999)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteTicket(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetTicketsByStatus_ReturnsOkResult_WithFilteredTickets()
    {
        // Arrange
        var tickets = new List<Ticket>
        {
            new Ticket { Id = 1, Title = "Open Ticket", Description = "Desc", Category = "Software", Priority = "High", Status = "Open", CreatedDate = System.DateTime.UtcNow },
            new Ticket { Id = 2, Title = "Another Open", Description = "Desc", Category = "Hardware", Priority = "Medium", Status = "Open", CreatedDate = System.DateTime.UtcNow }
        };
        _mockRepository.Setup(repo => repo.GetTicketsByStatusAsync("Open")).ReturnsAsync(tickets);

        // Act
        var result = await _controller.GetTicketsByStatus("Open");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTickets = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
        Assert.Equal(2, returnedTickets.Count);
        Assert.All(returnedTickets, t => Assert.Equal("Open", t.Status));
    }

    [Fact]
    public async Task GetTicketsByStatus_ReturnsEmptyList_WhenNoMatchingTicketsExist()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetTicketsByStatusAsync("Closed")).ReturnsAsync(new List<Ticket>());

        // Act
        var result = await _controller.GetTicketsByStatus("Closed");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTickets = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
        Assert.Empty(returnedTickets);
    }
}