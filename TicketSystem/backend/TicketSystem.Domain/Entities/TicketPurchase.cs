namespace TicketSystem.Domain.Entities;

public class TicketPurchase
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int ZoneId { get; set; }
    public string BuyerName { get; set; } = string.Empty;
    public string BuyerEmail { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;

    public Event Event { get; set; } = null!;
    public TicketZone Zone { get; set; } = null!;
}
