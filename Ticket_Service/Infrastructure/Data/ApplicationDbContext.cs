using Microsoft.EntityFrameworkCore;
using Ticket_Service.Features.Tickets.Models;

namespace Ticket_Service.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Ticket> Tickets { get; init; }
    }
}