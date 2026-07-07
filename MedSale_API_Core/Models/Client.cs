using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MedSale_API_Core.Models;

public partial class Client
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("recipientCompany")]
    public string? RecipientCompany { get; set; }

    [Column("recipientFullName")]
    public string RecipientFullName { get; set; } = null!;

    [Column("address")]
    public string Address { get; set; } = null!;

    [Column("phone")]
    public string Phone { get; set; } = null!;

    [Column("email")]
    public string Email { get; set; } = null!;

    [Column("INN")]
    [StringLength(13)]
    public string Inn { get; set; } = null!;

    [Column("OGRN")]
    [StringLength(13)]
    public string Ogrn { get; set; } = null!;

    [Column("acceptScan")]
    public byte[]? AcceptScan { get; set; }

    [InverseProperty("Recipient")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
