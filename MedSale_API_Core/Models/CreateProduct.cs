using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MedSale_API_Core.Models;

[Table("CreateProduct")]
public partial class CreateProduct
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("productId")]
    public int ProductId { get; set; }

    [Column("countProduct")]
    public int CountProduct { get; set; }

    [Column("forOrder")]
    public int? ForOrder { get; set; }

    [Column("status")]
    public string Status { get; set; } = null!;

    [ForeignKey("ForOrder")]
    [InverseProperty("CreateProducts")]
    public virtual Order? ForOrderNavigation { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("CreateProducts")]
    public virtual Product Product { get; set; } = null!;
}
