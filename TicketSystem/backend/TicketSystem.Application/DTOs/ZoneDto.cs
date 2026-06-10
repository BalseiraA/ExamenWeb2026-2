namespace TicketSystem.Application.DTOs;

public record ZoneDto(
    int Id,
    string ZoneType,
    decimal Price,
    int TotalCapacity,
    int AvailableCapacity);
