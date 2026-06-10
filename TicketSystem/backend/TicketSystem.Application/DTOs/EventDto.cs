namespace TicketSystem.Application.DTOs;

public record EventDto(
    int Id,
    string Name,
    string Description,
    DateTime Date,
    string Venue,
    string Status,
    DateTime CreatedAt,
    List<ZoneDto> Zones);
