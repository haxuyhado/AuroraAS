using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MedSale_API_Core.Models;

public partial class Employee
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("fullName")]
    public string FullName { get; set; } = null!;

    [Column("positionId")]
    public int PositionId { get; set; }

    [Column("address")]
    public string Address { get; set; } = null!;

    [Column("phone")]
    public string Phone { get; set; } = null!;

    [Column("email")]
    public string Email { get; set; } = null!;

    [Column("myPassword")]
    [StringLength(16)]
    public string MyPassword { get; set; } = null!;

    [InverseProperty("Manager")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [ForeignKey("PositionId")]
    [InverseProperty("Employees")]
    public virtual Position Position { get; set; } = null!;
}
