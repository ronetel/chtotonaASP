using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace chtotonaASP.Models;

public partial class AspnetContext : DbContext
{
    public AspnetContext()
    {
    }

    public AspnetContext(DbContextOptions<AspnetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CatList> CatLists { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<ProductsInOrder> ProductsInOrders { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=ronetel\\SQLEXPRESS;Initial Catalog=ASPnet;Integrated Security=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CatList>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__CatList__9833FF922AF77E69");

            entity.ToTable("CatList");

            entity.Property(e => e.ProductId).HasColumnName("Product_id");
            entity.Property(e => e.DescProduct)
                .HasColumnType("text")
                .HasColumnName("Desc_product");
            entity.Property(e => e.IdproductType).HasColumnName("IDProductType");
            entity.Property(e => e.Img).HasColumnType("text");
            entity.Property(e => e.NameProduct)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Name_product");

            entity.HasOne(d => d.IdproductTypeNavigation).WithMany(p => p.CatLists)
                .HasForeignKey(d => d.IdproductType)
                .HasConstraintName("FK__CatList__IDProdu__4222D4EF");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewId).HasName("PK__News__AA103E5E984E45B7");

            entity.Property(e => e.NewId).HasColumnName("New_ID");
            entity.Property(e => e.Caption).HasColumnType("text");
            entity.Property(e => e.Link).HasColumnType("text");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.ProductTypeId).HasName("PK__ProductT__A1312F6E2F161FD1");

            entity.ToTable("ProductType");

            entity.Property(e => e.ProductType1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ProductType");
        });

        modelBuilder.Entity<ProductsInOrder>(entity =>
        {
            entity.HasKey(e => e.PinorId).HasName("PK__Products__D24B2BC6E269B5AC");

            entity.ToTable("ProductsInOrder");

            entity.Property(e => e.PinorId).HasColumnName("Pinor_id");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductsInOrders)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__ProductsI__Produ__45F365D3");

            entity.HasOne(d => d.User).WithMany(p => p.ProductsInOrders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ProductsI__UserI__44FF419A");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Review__F803F2C320C55B8F");

            entity.ToTable("Review");

            entity.Property(e => e.ReviewId).HasColumnName("Review_id");
            entity.Property(e => e.Comment).HasMaxLength(255);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Review__ProductI__48CFD27E");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Review__UserID__49C3F6B7");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PK__Roles__45DFFBDB33910FCA");

            entity.HasIndex(e => e.NameRole, "UQ__Roles__28A576BDD89E84BC").IsUnique();

            entity.Property(e => e.IdRole).HasColumnName("ID_role");
            entity.Property(e => e.NameRole)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Name_role");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__Users__ED4DE442D5BA6572");

            entity.HasIndex(e => e.Phone, "UQ__Users__5C7E359EBC03765E").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053465AC8425").IsUnique();

            entity.Property(e => e.IdUser).HasColumnName("ID_User");
            entity.Property(e => e.DateJoined)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Users__RoleID__3C69FB99");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
