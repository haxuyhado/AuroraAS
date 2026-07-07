using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MedSale_API_Core.Models;

public partial class AuroraContext : DbContext
{
    public AuroraContext()
    {
    }

    public AuroraContext(DbContextOptions<AuroraContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<CreateProduct> CreateProducts { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<ItemsInOrder> ItemsInOrders { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-49VRJ07\\MSSQL2019;Database=AuroraData;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clients__3213E83FB5FC65CA");
        });

        modelBuilder.Entity<CreateProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CreatePr__3213E83F8AD220F9");

            entity.HasOne(d => d.ForOrderNavigation).WithMany(p => p.CreateProducts).HasConstraintName("FK__CreatePro__forOr__4222D4EF");

            entity.HasOne(d => d.Product).WithMany(p => p.CreateProducts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CreatePro__produ__412EB0B6");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F77C0588B");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employees__posit__267ABA7A");
        });

        modelBuilder.Entity<ItemsInOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ItemsInO__3213E83FB1B8E4F1");

            entity.HasOne(d => d.Order).WithMany(p => p.ItemsInOrders).HasConstraintName("FK__ItemsInOr__order__3D5E1FD2");

            entity.HasOne(d => d.Product).WithMany(p => p.ItemsInOrders).HasConstraintName("FK__ItemsInOr__produ__3E52440B");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3213E83F8F9ABDE4");

            entity.HasOne(d => d.Manager).WithMany(p => p.Orders).HasConstraintName("FK__Orders__managerI__3A81B327");

            entity.HasOne(d => d.Recipient).WithMany(p => p.Orders).HasConstraintName("FK__Orders__recipien__398D8EEE");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Position__3213E83FAE725077");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3213E83FF8706E8E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
