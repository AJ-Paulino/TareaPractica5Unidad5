using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TareaPractica5Unidad5.Models;

public partial class Practica5Context : DbContext
{
    public Practica5Context()
    {
    }

    public Practica5Context(DbContextOptions<Practica5Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<HistorialRefreshToken> HistorialRefreshTokens { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedor> Proveedors { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server = AJ; Database= Practica5; Trusted_Connection= true; MultipleActiveResultSets= true; Encrypt= false; TrustServerCertificate= false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCategoria);

            entity.HasMany(d => d.ProveedoresIdProveedors).WithMany(p => p.CategoriasIdCategoria)
                .UsingEntity<Dictionary<string, object>>(
                    "CategoriaProveedor",
                    r => r.HasOne<Proveedor>().WithMany().HasForeignKey("ProveedoresIdProveedor"),
                    l => l.HasOne<Categorium>().WithMany().HasForeignKey("CategoriasIdCategoria"),
                    j =>
                    {
                        j.HasKey("CategoriasIdCategoria", "ProveedoresIdProveedor");
                        j.ToTable("CategoriaProveedor");
                        j.HasIndex(new[] { "ProveedoresIdProveedor" }, "IX_CategoriaProveedor_ProveedoresIdProveedor");
                    });
        });

        modelBuilder.Entity<HistorialRefreshToken>(entity =>
        {
            entity.HasKey(e => e.IdHistorialToken).HasName("PK__Historia__03DC48A54B14317C");

            entity.ToTable("HistorialRefreshToken");

            entity.Property(e => e.EsActivo).HasComputedColumnSql("(case when [FechaExpiracion]<getdate() then CONVERT([bit],(0)) else CONVERT([bit],(1)) end)", false);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaExpiracion).HasColumnType("datetime");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Token)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.HistorialRefreshTokens)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Historial__IdUsu__5CD6CB2B");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("Producto");

            entity.HasIndex(e => e.IdCategoria, "IX_Producto_IdCategoria");

            entity.HasIndex(e => e.IdProveedor, "IX_Producto_IdProveedor");

            entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos).HasForeignKey(d => d.IdCategoria);

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Productos).HasForeignKey(d => d.IdProveedor);
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.IdProveedor);

            entity.ToTable("Proveedor");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.Property(e => e.Password).HasDefaultValue("");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
