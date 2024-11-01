using Ticket_Service.Features.Tickets.DTOs;
using Ticket_Service.Features.Tickets.Models;

namespace Ticket_Service.Services
{
    public interface ITicketService
    {
        public Task<TicketDto> CreateTicketAsync(CreateTicketDto createTicketDto);
        public Task<TicketDto?> GetTicketByIdAsync(int id);

        public Task<IEnumerable<TicketDto>> GetSentTickets(int userId);
        
        public Task<IEnumerable<TicketDto>> GetReceivedTickets(int userId);
    }
}