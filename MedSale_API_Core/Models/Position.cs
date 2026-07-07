using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MedSale_API_Core.Models;

public partial class Position
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("position")]
    public string Position1 { get; set; } = null!;

    [InverseProperty("Position")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
