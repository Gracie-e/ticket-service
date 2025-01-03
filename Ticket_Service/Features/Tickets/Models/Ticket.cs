﻿using System.ComponentModel.DataAnnotations;

namespace Ticket_Service.Features.Tickets.Models
{
    public class Ticket
    {
        public int Id { get; init; }
        
        [Required]
        public required int SenderUserId { get; init; }
        
        [Required]
        public required IList<int> ReceiverUserIds { get; init; }
        
        [Required]
        [StringLength(100)]
        public required string Title { get; init; } =string.Empty;
        
        [Required]
        [StringLength(300)]
        public string Description { get; init; } = string.Empty;
        
        [Required]
        public DateTime CreatedAt { get; init; }
        
        [Required]
        public TicketStatus Status { get; init; }
        
    }

    public enum TicketStatus
    {
        Open,
        InProgress,
        Resolved,
        Closed
    }
}