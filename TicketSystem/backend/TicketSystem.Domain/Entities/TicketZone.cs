using TicketSystem.Domain.Enums;

namespace TicketSystem.Domain.Entities;

public class TicketZone
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public ZoneType ZoneType { get; set; }
    public decimal Price { get; set; }
    public int TotalCapacity { get; set; }
    public int AvailableCapacity { get; set; }

    public Event Event { get; set; } = null!;
}
