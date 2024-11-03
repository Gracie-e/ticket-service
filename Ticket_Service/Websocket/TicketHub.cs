using Microsoft.AspNetCore.SignalR;
using Ticket_Service.Features.Tickets.Models;
using Ticket_Service.Services;
using System.Collections.Concurrent;

namespace Ticket_Service.Websocket;

public class TicketHub(ITicketService ticketService) : Hub
{
    private static readonly ConcurrentDictionary<string, int> UserConnections = new();

    // Register user connection with their userId
    public async Task RegisterUser(int userId)
    {
        UserConnections.TryAdd(Context.ConnectionId, userId);
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
    }

    // Send ticket to specific receivers
    public async Task SendTicket(Ticket ticket)
    {
        // Send to all receivers
        foreach (var receiverId in ticket.ReceiverUserIds)
        {
            await Clients.Group($"user_{receiverId}").SendAsync("ReceiveTicket", ticket);
        }

        // Also send to sender
        await Clients.Group($"user_{ticket.SenderUserId}").SendAsync("TicketSent", ticket);
    }

    // Notify about ticket status changes
    public async Task UpdateTicketStatus(int ticketId, TicketStatus newStatus)
    {
        var ticket = await ticketService.GetTicketByIdAsync(ticketId);
        if (ticket != null)
        {
            // Notify all involved parties about the status change
            var involvedUsers = new List<int> { ticket.SenderUserId };
            involvedUsers.AddRange(ticket.ReceiverUserIds);

            foreach (var userId in involvedUsers)
            {
                await Clients.Group($"user_{userId}").SendAsync("TicketStatusUpdated", ticketId, newStatus);
            }
        }
    }

    // Handle disconnection
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        UserConnections.TryRemove(Context.ConnectionId, out _);
        return base.OnDisconnectedAsync(exception);
    }
}