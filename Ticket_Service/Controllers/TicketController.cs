using Microsoft.AspNetCore.Mvc;
using Ticket_Service.Common.Api;
using Ticket_Service.Features.Tickets.DTOs;
using Ticket_Service.Services;

namespace Ticket_Service.Controllers;

[ApiController]
[Route("api/tickets")]
public class TicketController(ITicketService ticketService, ILogger<TicketController> logger) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<TicketDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TicketDto>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TicketDto>> CreateTicket(CreateTicketDto dto)
    {
        try
        {
            var ticket = await ticketService.CreateTicketAsync(dto);
            var response = ApiResponse<TicketDto>.Success(ticket);
            logger.LogInformation("Ticket created: {Ticket}", ticket);
            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, response);
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "An error occured while creating a ticket");
            return BadRequest(ApiResponse<TicketDto>.Failure(ex.Message, StatusCodes.Status400BadRequest));
        }
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TicketDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<TicketDto>>>> GetAllTickets()
    {
        try
        {
            var tickets = await ticketService.GetAllTicketsAsync();
            return Ok(ApiResponse<IEnumerable<TicketDto>>.Success(tickets));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving all tickets");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                ApiResponse<IEnumerable<TicketDto>>.Failure("An error occurred while retrieving tickets"));
        }
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<TicketDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TicketDto>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TicketDto>>> GetTicket(int id)
    {
        try
        {
            var ticket = await ticketService.GetTicketByIdAsync(id);

            if (ticket == null)
            {
                return NotFound(ApiResponse<TicketDto>.Failure(
                    $"Ticket with ID {id} not found", 
                    StatusCodes.Status404NotFound));
            }

            return Ok(ApiResponse<TicketDto>.Success(ticket));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving ticket {TicketId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, 
                ApiResponse<TicketDto>.Failure("An error occurred while retrieving the ticket"));
        }
    }

    [HttpGet("sent/{userId:int}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TicketDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<TicketDto>>>> GetSentTickets(int userId)
    {
        try
        {
            var tickets = await ticketService.GetSentTickets(userId);
            return Ok(ApiResponse<IEnumerable<TicketDto>>.Success(tickets));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving sent tickets for user {UserId}", userId);
            return StatusCode(StatusCodes.Status500InternalServerError, 
                ApiResponse<IEnumerable<TicketDto>>.Failure("An error occurred while retrieving sent tickets"));
        }
    }
    [HttpGet("received/{userId:int}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TicketDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<TicketDto>>>> GetReceivedTickets(int userId)
    {
        try
        {
            var tickets = await ticketService.GetReceivedTickets(userId);
            return Ok(ApiResponse<IEnumerable<TicketDto>>.Success(tickets));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving received tickets for user {UserId}", userId);
            return StatusCode(StatusCodes.Status500InternalServerError, 
                ApiResponse<IEnumerable<TicketDto>>.Failure("An error occurred while retrieving received tickets"));
        }
    }
}