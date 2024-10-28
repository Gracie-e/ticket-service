using Microsoft.EntityFrameworkCore;
using Ticket_Service.Models.Tickets;

namespace Ticket_Service.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Ticket> Tickets { get; init; }
    }
}