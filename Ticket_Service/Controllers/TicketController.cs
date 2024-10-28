using Microsoft.AspNetCore.Mvc;
using Ticket_Service.DTOs;
using Ticket_Service.Models.Tickets;
using Ticket_Service.Services;

namespace Ticket_Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketController(ITicketService ticketService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Ticket>> CreateTicket(CreateTicketDto dto)
    {
        var ticket = await ticketService.CreateTicketAsync(dto);
        return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
    }

    // get ticket by id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TicketDto>> GetTicket(int id)
    {
        var ticket = await ticketService.GetTicketByIdAsync(id);

        if (ticket == null)
        {
            return NotFound();
        }

        return ticket;
    }
    
    // get user sent tickets
    [HttpGet("sent/{userId:int}")]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetSentTickets(int userId)
    {
        var tickets = await ticketService.GetSentTickets(userId);
        return Ok(tickets);
    }

    [HttpGet("received/{userId:int}")]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetReceivedTickets(int userId)
    {
        var tickets = await ticketService.GetReceivedTickets(userId);
        return Ok(tickets);
    }
    
    

}

    
    
