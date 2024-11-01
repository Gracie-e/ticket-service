using Microsoft.AspNetCore.Mvc;
using Ticket_Service.Features.Tickets.DTOs;
using Ticket_Service.Features.Tickets.Models;
using Ticket_Service.Services;

namespace Ticket_Service.Controllers;

[ApiController]
[Route("api/tickets")]
public class TicketController(ITicketService ticketService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(TicketDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TicketDto>> CreateTicket(CreateTicketDto dto)
    {
        try
        {
            var ticket = await ticketService.CreateTicketAsync(dto);
            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TicketDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetAllTickets()
    {
        var tickets = await ticketService.GetAllTicketsAsync();
        return Ok(tickets);
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TicketDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TicketDto>> GetTicket(int id)
    {
        var ticket = await ticketService.GetTicketByIdAsync(id);

        if (ticket == null)
        {
            return NotFound(new { message = $"Ticket with ID {id} not found" });
        }

        return Ok(ticket);
    }
    
    [HttpGet("sent/{userId:int}")]
    [ProducesResponseType(typeof(IEnumerable<TicketDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetSentTickets(int userId)
    {
        var tickets = await ticketService.GetSentTickets(userId);
        return Ok(tickets);
    }

    [HttpGet("received/{userId:int}")]
    [ProducesResponseType(typeof(IEnumerable<TicketDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetReceivedTickets(int userId)
    {
        var tickets = await ticketService.GetReceivedTickets(userId);
        return Ok(tickets);
    }
}