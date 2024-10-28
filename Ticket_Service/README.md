# Ticket Service

A simple CRUD application built with .NET 8 to explore and learn modern .NET development concepts. The project implements real-time ticket updates using WebSocket connections.

## Project Overview

Coming from a React/Node.js background, I built this project to understand .NET's approach to:
- API development
- Real-time updates with SignalR
- Entity Framework Core for handling data
- Clean architecture principles in .NET

## Features

- Basic CRUD operations for tickets
- Real-time updates using SignalR WebSocket connections
- Entity Framework Core with SQL Server
- RESTful API endpoints
- Architecture with services, controllers, and DTOs

## .NET 8** - Latest .NET framework


## Getting Started

1. Clone the repository
2. Ensure you have .NET 8 SDK installed
3. Update the connection string in `appsettings.json`
4. Run migrations:
```bash
dotnet ef database update
```
5. Start the application:
```bash
dotnet run
```

## Project Structure

```
Ticket_Service/
├── Controllers/          # API endpoints
├── Features/
│   └── Tickets/         # Ticket-related models, DTOs, and services
├── Infrastructure/      # Database and migrations
├── WebSocket/          # SignalR hub for real-time updates
└── Program.cs          # Application entry point
```

## Learning Goals

This project serves as a practical introduction to:
- .NET's dependency injection system
- Entity Framework Core operations
- SignalR for WebSocket communication
- Clean architecture in .NET applications

## Future Improvements

- Add authentication
- Implement ticket categories
- Add unit tests
- Enhance real-time features