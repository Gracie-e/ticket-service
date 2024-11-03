using Microsoft.EntityFrameworkCore;
using Ticket_Service.Infrastructure.Data;
using Ticket_Service.Features.Tickets.DTOs;
using Ticket_Service.Features.Tickets.Models;

namespace Ticket_Service.Services
{
    public class TicketService(ApplicationDbContext context, ILogger<TicketService> logger) : ITicketService
        
    {

        public async Task<TicketDto> CreateTicketAsync(CreateTicketDto createTicketDto)
        {
            try
            {
                // Validate receiver list is not empty
                if (createTicketDto.ReceiverUserIds == null || !createTicketDto.ReceiverUserIds.Any())
                {
                    throw new ArgumentException("At least one receiver is required", nameof(createTicketDto));
                }

                // Validate title
                if (string.IsNullOrWhiteSpace(createTicketDto.Title))
                {
                    throw new ArgumentException("Title is required", nameof(createTicketDto));
                }

                var ticket = new Ticket
                {
                    SenderUserId = createTicketDto.SenderUserId,
                    ReceiverUserIds = createTicketDto.ReceiverUserIds,
                    Title = createTicketDto.Title.Trim(),
                    Status = createTicketDto.Status,
                    CreatedAt = DateTime.UtcNow,
                    Description = createTicketDto.Description.Trim(),
                };

                context.Tickets.Add(ticket);
                await context.SaveChangesAsync();
                
                logger.LogInformation("Created ticket with ID: {TicketId}", ticket.Id);
                return ConvertToTicketDto(ticket);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating ticket");
                throw;
            }
        }

        public async Task<List<TicketDto>> GetAllTicketsAsync()
        {
            try
            {
                return await context.Tickets
                    .AsNoTracking()
                    .OrderByDescending(t => t.CreatedAt)
                    .Select(ticket => ConvertToTicketDto(ticket))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fetching all tickets");
                throw;
            }
        } 
        
      public async Task<TicketDto?> GetTicketByIdAsync(int ticketId)
        {
            try
            {
                var ticket = await context.Tickets
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == ticketId);

                return ticket != null ? ConvertToTicketDto(ticket) : null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving ticket with ID: {TicketId}", ticketId);
                throw;
            }
        }

        public async Task<IEnumerable<TicketDto>> GetSentTickets(int userId)
        {
            try
            {
                return await context.Tickets
                    .AsNoTracking()
                    .Where(t => t.SenderUserId == userId)
                    .OrderByDescending(t => t.CreatedAt)
                    .Select(ticket => ConvertToTicketDto(ticket))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving sent tickets for user: {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<TicketDto>> GetReceivedTickets(int userId)
        {
            try
            {
                return await context.Tickets
                    .AsNoTracking()
                    .Where(t => t.ReceiverUserIds.Contains(userId))
                    .OrderByDescending(t => t.CreatedAt)
                    .Select(ticket => ConvertToTicketDto(ticket))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving received tickets for user: {UserId}", userId);
                throw;
            }
        }

        private static TicketDto ConvertToTicketDto(Ticket ticket)
        {
            return new TicketDto(
                ticket.Id,
                ticket.Title,
                ticket.Description,
                ticket.Status,
                ticket.SenderUserId,
                ticket.ReceiverUserIds,
                ticket.CreatedAt
            );
        }
    }
}