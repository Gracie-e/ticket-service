using Microsoft.EntityFrameworkCore;
using Ticket_Service.Infrastructure.Data;
using Ticket_Service.Services;
using Ticket_Service.Websocket;
using Ticket_Service.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// register middleware
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteApp", corsPolicyBuilder =>
    {
        corsPolicyBuilder.WithOrigins("http://localhost:5173", "http://localhost:5174")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowViteApp");

app.UseAuthorization();

app.MapControllers();

app.MapHub<TicketHub>("/hubs/tickets");

app.Run();