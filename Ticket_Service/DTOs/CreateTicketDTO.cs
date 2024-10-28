using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Ticket_Service.Models.Tickets;

namespace Ticket_Service.DTOs;

public class CreateTicketDto
{
    
    [Required(ErrorMessage = "Sender User ID is required")]
    public int SenderUserId { get; set; }
    
    [Required(ErrorMessage = "At least one receiver user ID is required")]
    [MinLength(1, ErrorMessage = "At least one Receiver User ID must be provided.")]
    public required IList<int> ReceiverUserIds { get; set; }
    
    [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
    public required string Title { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TicketStatus Status { get; set; } = TicketStatus.Open;
}