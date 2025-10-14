using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using TDRevision.Models;

namespace TDRevision.Models.EntityFramework;

public partial class AppDbContext : DbContext
{
    public DbSet<Utilisateur> Utilisateurs { get; set; } = null!;
    public DbSet<Commande> Commandes { get; set; } = null!;

    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Server=localhost;port=5432;Database=TDREVISION;uid=postgres;password=postgres;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.HasDefaultSchema("public");

        modelBuilder.Entity<Commande>(entity =>
        {
            entity.HasKey(e => e.IdCommande).HasName("COM_ID");

            entity.HasOne(entity => entity.CommandeNav)
                    .WithMany(m => m.Commandes)
                    .HasConstraintName("UTI_ID");
        });

        modelBuilder.Entity<Utilisateur>(entity =>
        {
            entity.HasKey(e => e.IdUtilisateur).HasName("UTI_ID");

            entity.HasMany(entity => entity.Commandes)
            .WithOne(m => m.CommandeNav)
            .HasConstraintName("UTI_ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
