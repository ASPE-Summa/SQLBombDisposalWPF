using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace SQLBombDisposal.Models;

public partial class SqlBombDisposalContext : DbContext
{
    public SqlBombDisposalContext()
    {
    }

    public SqlBombDisposalContext(DbContextOptions<SqlBombDisposalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Battleship> Battleships { get; set; }

    public virtual DbSet<Button> Buttons { get; set; }

    public virtual DbSet<ButtonVoltage> ButtonVoltages { get; set; }

    public virtual DbSet<DoctrineMigrationVersion> DoctrineMigrationVersions { get; set; }

    public virtual DbSet<MazePuzzle> MazePuzzles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL(ConfigurationManager.ConnectionStrings["BombsDb"].ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Battleship>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("battleship");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Coordinates)
                .HasMaxLength(255)
                .HasColumnName("coordinates");
            entity.Property(e => e.Description).HasColumnName("description");
        });

        modelBuilder.Entity<Button>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("button");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Emoji)
                .HasMaxLength(200)
                .HasColumnName("emoji");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ButtonVoltage>(entity =>
        {
            entity.HasKey(e => e.ButtonId).HasName("PRIMARY");

            entity.ToTable("button_voltage");

            entity.Property(e => e.ButtonId).HasColumnName("buttonId");
            entity.Property(e => e.Voltage).HasColumnName("voltage");

            entity.HasOne(d => d.Button).WithOne(p => p.ButtonVoltage)
                .HasForeignKey<ButtonVoltage>(d => d.ButtonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DADBBE369F35DE89");
        });

        modelBuilder.Entity<DoctrineMigrationVersion>(entity =>
        {
            entity.HasKey(e => e.Version).HasName("PRIMARY");

            entity.ToTable("doctrine_migration_versions");

            entity.Property(e => e.Version)
                .HasMaxLength(191)
                .HasColumnName("version");
            entity.Property(e => e.ExecutedAt)
                .HasColumnType("datetime")
                .HasColumnName("executed_at");
            entity.Property(e => e.ExecutionTime).HasColumnName("execution_time");
        });

        modelBuilder.Entity<MazePuzzle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("maze_puzzle");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Contents)
                .HasMaxLength(5)
                .HasColumnName("contents");
            entity.Property(e => e.Pattern).HasColumnName("pattern");
            entity.Property(e => e.Sequence).HasColumnName("sequence");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
