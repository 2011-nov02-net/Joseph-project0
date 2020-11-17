using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Project0.Data
{
    public partial class P0Context : DbContext
    {
        public P0Context()
        {
        }

        public P0Context(DbContextOptions<P0Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<StoreCustomer> StoreCustomers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("Item");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__Item__OrderID__17C286CF");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Item__ProductId__19AACF41");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK__Item__StoreID__18B6AB08");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Order__CustomerI__14E61A24");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Order__StoreId__13F1F5EB");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.HasIndex(e => e.Name, "UQ__Product__737584F60A29C6C5")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("Store");

                entity.HasIndex(e => e.Location, "UQ__Store__E55D3B106F424623")
                    .IsUnique();

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<StoreCustomer>(entity =>
            {
                entity.ToTable("StoreCustomer");

                entity.HasIndex(e => e.Name, "UQ__StoreCus__737584F60D0D873E")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
