﻿using Ticket_Service.Models.Tickets;

namespace Ticket_Service.DTOs;

public class TicketDto(
    int id,
    string title,
    TicketStatus status,
    int senderUserId,
    IList<int> receiverUserIds,
    DateTime createdAt)
{
    public int Id { get; set; } = id;
    public string Title { get; set; } = title;
    public TicketStatus Status { get; set; } = status;
    public int SenderUserId { get; set; } = senderUserId;
    public IList<int> ReceiverUserIds { get; set; } = receiverUserIds;
    public DateTime CreatedAt { get; set; } = createdAt;
}