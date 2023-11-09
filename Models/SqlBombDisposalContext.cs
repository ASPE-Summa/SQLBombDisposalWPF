using System;
using System.Collections.Generic;
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

    public virtual DbSet<Button> Buttons { get; set; }

    public virtual DbSet<ButtonVoltage> ButtonVoltages { get; set; }

    public virtual DbSet<DoctrineMigrationVersion> DoctrineMigrationVersions { get; set; }

    public virtual DbSet<MazePuzzle> MazePuzzles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("Server=127.0.0.1; Port=3308; Database=sqlBombDisposal; Uid=root; Pwd=password;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
