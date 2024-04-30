using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ReserveServer.ReserveModel;

[Table("Reservation")]
public partial class Reservation
{
    [Key]
    [Column("reservation_id")]
    public int ReservationId { get; set; }

    [Column("customer_id")]
    public int CustomerId { get; set; }

    [Column("reservation_date")]
    public DateOnly ReservationDate { get; set; }

    [Column("reservation_time")]
    public TimeOnly ReservationTime { get; set; }

    [Column("party_size")]
    public int? PartySize { get; set; }

    [Column("special_requests")]
    [StringLength(255)]
    public string? SpecialRequests { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Reservations")]
    public virtual Customer Customer { get; set; } = null!;
}
