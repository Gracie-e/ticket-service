using Microsoft.EntityFrameworkCore;
using Ticket_Service.Data;
using Ticket_Service.DTOs;
using Ticket_Service.Models.Tickets;

namespace Ticket_Service.Services
{
    public class TicketService(ApplicationDbContext context) : ITicketService
    {

        public async Task<TicketDto> CreateTicketAsync(CreateTicketDto createTicketDto)
        // validate the input data
        {
            var ticket = new Ticket
            {
                SenderUserId = createTicketDto.SenderUserId,
                ReceiverUserIds = createTicketDto.ReceiverUserIds,
                Title = createTicketDto.Title,
                Status = createTicketDto.Status,
                CreatedAt = DateTime.UtcNow
            };

            context.Tickets.Add(ticket);
            await context.SaveChangesAsync();
            return ConvertToTicketDto(ticket);                                                                                                       
        }


       public async Task<TicketDto?> GetTicketByIdAsync(int ticketId)
       {
           var ticket = await context.Tickets.FindAsync(ticketId);
           return ticket != null ? ConvertToTicketDto(ticket) : null;
       }

       public async Task<IEnumerable<TicketDto>> GetSentTickets(int userId)
       {
            return await context.Tickets
                .Where(t => t.SenderUserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .Select(ticket => ConvertToTicketDto(ticket))
                .ToListAsync();
       }

       public async Task<IEnumerable<TicketDto>> GetReceivedTickets(int userId)
       {
           
           return await context.Tickets
               .Where(t => t.ReceiverUserIds.Contains(userId))
               .Select(ticket => ConvertToTicketDto(ticket))
               .ToListAsync();
       }

       private static TicketDto ConvertToTicketDto(Ticket ticket)
        {
            return new TicketDto(
                ticket.Id,
                ticket.Title,
                ticket.Status,
                ticket.SenderUserId,
                ticket.ReceiverUserIds,
                ticket.CreatedAt
                );
        }

    }
}
