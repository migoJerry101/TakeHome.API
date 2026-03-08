using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TakeHome.API.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Packaging> Packagings { get; set; }

    public virtual DbSet<PackagingItem> PackagingItems { get; set; }

    public virtual DbSet<PackagingType> PackagingTypes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=PackagingDb;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ItemName).HasMaxLength(200);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Packaging>(entity =>
        {
            entity.ToTable("Packaging");

            entity.HasIndex(e => e.ParentPackagingId, "IX_Packaging_ParentId");

            entity.HasIndex(e => e.ProductId, "IX_Packaging_ProductId");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.PackagingType).WithMany(p => p.Packagings)
                .HasForeignKey(d => d.PackagingTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Packaging_Type");

            entity.HasOne(d => d.ParentPackaging).WithMany(p => p.InverseParentPackaging)
                .HasForeignKey(d => d.ParentPackagingId)
                .HasConstraintName("FK_Packaging_Parent");

            entity.HasOne(d => d.Product).WithMany(p => p.Packagings)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Packaging_Product");
        });

        modelBuilder.Entity<PackagingItem>(entity =>
        {
            entity.HasIndex(e => e.ItemId, "IX_PackagingItems_ItemId");

            entity.HasIndex(e => e.PackagingId, "IX_PackagingItems_PackagingId");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Item).WithMany(p => p.PackagingItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagingItems_Item");

            entity.HasOne(d => d.Packaging).WithMany(p => p.PackagingItems)
                .HasForeignKey(d => d.PackagingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagingItems_Packaging");
        });

        modelBuilder.Entity<PackagingType>(entity =>
        {
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TypeName).HasMaxLength(100);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasIndex(e => e.Sku, "UQ__Products__CA1ECF0D0088C12D").IsUnique();

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProductName).HasMaxLength(200);
            entity.Property(e => e.Sku)
                .HasMaxLength(100)
                .HasColumnName("SKU");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
