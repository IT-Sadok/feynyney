using LogisticsApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogisticsApp.Configuration;

public class PackageConfiguration : IEntityTypeConfiguration<Package>
{
    public void Configure(EntityTypeBuilder<Package> builder)
    {
        builder.ToTable("Packages");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.Property(x => x.Weight)
            .HasPrecision(10, 2)
            .IsRequired();
        
        builder.Property(x => x.TrackingNumber)
            .HasMaxLength(64)
            .IsRequired();
        
        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.SentAt)
            .IsRequired();
        
        builder.Property(x => x.DeliveredAt)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(x => x.TrackingNumber).IsUnique();
        builder.HasIndex(x => x.SenderUserId);
        builder.HasIndex(x => x.RecipientUserId);
        builder.HasIndex(x => x.Status);
        
        // Relationships
        
        //Sender
        builder.HasOne(x => x.SenderUser)
            .WithMany(u => u.SentPackages)
            .HasForeignKey(x =>  x.SenderUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Recipient
        builder.HasOne(x => x.RecipientUser)
            .WithMany(u => u.ReceivedPackages)
            .HasForeignKey(x =>  x.RecipientUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Origin Terminal
        builder.HasOne(x => x.OriginTerminal)
            .WithMany(t => t.OriginPackages)
            .HasForeignKey(x => x.OriginTerminalId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Destination Terminal
        builder.HasOne(x => x.DestinationTerminal)
            .WithMany(t => t.DestinationPackages)
            .HasForeignKey(x => x.DestinationTerminalId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Transport
        builder.HasOne(x => x.Transport)
            .WithMany(t => t.Packages)
            .HasForeignKey(x => x.TransportId)
            .OnDelete(DeleteBehavior.SetNull);

    }
}