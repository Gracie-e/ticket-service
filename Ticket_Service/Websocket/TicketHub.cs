using Ticket_Service.Features.Tickets.DTOs;
using Ticket_Service.Services;

namespace Ticket_Service.Websocket;
using Microsoft.AspNetCore.SignalR;

public class TicketHub(ITicketService ticketService) : Hub
{
    public async Task SendTicket(CreateTicketDto createTicketDto)
    {
        try
        {
            var createdTicket = await ticketService.CreateTicketAsync(createTicketDto);
            var receiverIds = createTicketDto.ReceiverUserIds.Select(id => id.ToString()).ToList();
            await Clients.Users(receiverIds).SendAsync("ReceiveTicket", createdTicket);
            await Clients.User(createTicketDto.SenderUserId.ToString()).SendAsync("TicketSent", createdTicket.Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await Clients.Caller.SendAsync("TicketCreationFailed", ex.Message);
        }
    }

    public async Task ReceiveUserTickets(int userId)
    {
        var userTickets = await ticketService.GetReceivedTickets(userId);
        await Clients.Caller.SendAsync("UserTicketsReceived", userTickets);
    }
}

