
using Ticket_Service.Features.Tickets.Models;
namespace Ticket_Service.Features.Tickets.DTOs;

public class TicketDto(
    int id,
    string title,
    string description,
    TicketStatus status,
    int senderUserId,
    IList<int> receiverUserIds,
    DateTime createdAt)
{
    public int Id { get; set; } = id;
    public string Title { get; set; } = title;
    public string Description { get; set; } = description;
    public TicketStatus Status { get; set; } = status;
    public int SenderUserId { get; set; } = senderUserId;
    public IList<int> ReceiverUserIds { get; set; } = receiverUserIds;
    public DateTime CreatedAt { get; set; } = createdAt;
}