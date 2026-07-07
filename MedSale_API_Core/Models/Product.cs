using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MedSale_API_Core.Models;

public partial class Product
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("productName")]
    public string ProductName { get; set; } = null!;

    [Column("mainCharacteristics", TypeName = "text")]
    public string? MainCharacteristics { get; set; }

    [Column("countProduct")]
    public int CountProduct { get; set; }

    [Column("price", TypeName = "money")]
    public decimal? Price { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<CreateProduct> CreateProducts { get; set; } = new List<CreateProduct>();

    [InverseProperty("Product")]
    public virtual ICollection<ItemsInOrder> ItemsInOrders { get; set; } = new List<ItemsInOrder>();
}
