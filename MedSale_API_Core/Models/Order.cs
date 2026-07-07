using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MedSale_API_Core.Models;

public partial class Order
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("orderNumber")]
    public string OrderNumber { get; set; } = null!;

    [Column("recipientId")]
    public int? RecipientId { get; set; }

    [Column("paymentMethod")]
    public string? PaymentMethod { get; set; }

    [Column("price", TypeName = "money")]
    public decimal Price { get; set; }

    [Column("orderStatus")]
    public string? OrderStatus { get; set; }

    [Column("creationDate")]
    public string? CreationDate { get; set; }

    [Column("managerId")]
    public int? ManagerId { get; set; }

    [Column("contractSkan")]
    public byte[]? ContractSkan { get; set; }

    [InverseProperty("ForOrderNavigation")]
    public virtual ICollection<CreateProduct> CreateProducts { get; set; } = new List<CreateProduct>();

    [InverseProperty("Order")]
    public virtual ICollection<ItemsInOrder> ItemsInOrders { get; set; } = new List<ItemsInOrder>();

    [ForeignKey("ManagerId")]
    [InverseProperty("Orders")]
    public virtual Employee? Manager { get; set; }

    [ForeignKey("RecipientId")]
    [InverseProperty("Orders")]
    public virtual Client? Recipient { get; set; }
}
