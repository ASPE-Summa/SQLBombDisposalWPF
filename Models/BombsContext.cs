using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SQLBombDisposal.Models;

public partial class BombsContext : DbContext
{
    public BombsContext()
    {
    }

    public BombsContext(DbContextOptions<BombsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bomb> Bombs { get; set; }

    public virtual DbSet<MazePuzzle> MazePuzzles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("DataSource=.\\Database\\bombs.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bomb>(entity =>
        {
            entity.ToTable("bombs");

            entity.Property(e => e.BombId).HasColumnName("bombId");
            entity.Property(e => e.Complexity)
                .HasDefaultValueSql("1")
                .HasColumnName("complexity");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<MazePuzzle>(entity =>
        {
            entity.HasKey(e => new { e.Pattern, e.Sequence });

            entity.ToTable("mazePuzzle");

            entity.Property(e => e.Pattern)
                .HasColumnType("INTEGER (11)")
                .HasColumnName("pattern");
            entity.Property(e => e.Sequence)
                .HasColumnType("INTEGER (11)")
                .HasColumnName("sequence");
            entity.Property(e => e.Contents)
                .HasColumnType("TEXT (5)")
                .HasColumnName("contents");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
