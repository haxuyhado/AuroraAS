using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MedSale_API_Core.Models;

[Table("ItemsInOrder")]
public partial class ItemsInOrder
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("orderId")]
    public int? OrderId { get; set; }

    [Column("productId")]
    public int? ProductId { get; set; }

    [Column("countProduct")]
    public int? CountProduct { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("ItemsInOrders")]
    public virtual Order? Order { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("ItemsInOrders")]
    public virtual Product? Product { get; set; }
}
