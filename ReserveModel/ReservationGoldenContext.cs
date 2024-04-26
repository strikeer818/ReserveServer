using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ReserveServer.ReserveModel;

public partial class ReservationGoldenContext : DbContext
{
    public ReservationGoldenContext()
    {
    }

    public ReservationGoldenContext(DbContextOptions<ReservationGoldenContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }
        IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
        var config = builder.Build();
        optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.CustomerId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.Property(e => e.ReservationId).ValueGeneratedOnAdd();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
