using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ReserveServer.ReserveModel;

[Table("Customer")]
public partial class Customer
{
    [Key]
    [Column("customer_id")]
    public int CustomerId { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("email")]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [Column("phone")]
    [StringLength(20)]
    public string Phone { get; set; } = null!;

    [InverseProperty("Customer")]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
