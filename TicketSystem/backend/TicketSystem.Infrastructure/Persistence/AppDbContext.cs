using Microsoft.EntityFrameworkCore;
using TicketSystem.Domain.Entities;

namespace TicketSystem.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Event> Events => Set<Event>();
    public DbSet<TicketZone> TicketZones => Set<TicketZone>();
    public DbSet<TicketPurchase> TicketPurchases => Set<TicketPurchase>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Venue).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);

            entity.HasMany(e => e.Zones)
                  .WithOne(z => z.Event)
                  .HasForeignKey(z => z.EventId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Purchases)
                  .WithOne(p => p.Event)
                  .HasForeignKey(p => p.EventId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TicketZone>(entity =>
        {
            entity.HasKey(z => z.Id);
            entity.Property(z => z.ZoneType).HasConversion<string>().HasMaxLength(20);
            entity.Property(z => z.Price).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<TicketPurchase>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.BuyerName).IsRequired().HasMaxLength(150);
            entity.Property(p => p.BuyerEmail).IsRequired().HasMaxLength(200);
            entity.Property(p => p.TotalAmount).HasColumnType("decimal(18,2)");

            entity.HasOne(p => p.Zone)
                  .WithMany()
                  .HasForeignKey(p => p.ZoneId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
