using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace SQLBombDisposal.Models;

public partial class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Adventurer> Adventurers { get; set; }

    public virtual DbSet<Battleship> Battleships { get; set; }

    public virtual DbSet<Button> Buttons { get; set; }

    public virtual DbSet<ButtonVoltage> ButtonVoltages { get; set; }

    public virtual DbSet<MazePuzzle> MazePuzzles { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite(ConfigurationManager.ConnectionStrings["BombsDb"].ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Adventurer>(entity =>
        {
            entity.ToTable("adventurer");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Charisma).HasColumnName("charisma");
            entity.Property(e => e.Class).HasColumnName("class");
            entity.Property(e => e.Constitution).HasColumnName("constitution");
            entity.Property(e => e.Dexterity).HasColumnName("dexterity");
            entity.Property(e => e.Intelligence).HasColumnName("intelligence");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Strength).HasColumnName("strength");
            entity.Property(e => e.Wisdom).HasColumnName("wisdom");
        });

        modelBuilder.Entity<Battleship>(entity =>
        {
            entity.ToTable("battleship");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Coordinates).HasColumnName("coordinates");
            entity.Property(e => e.Description).HasColumnName("description");
        });

        modelBuilder.Entity<Button>(entity =>
        {
            entity.ToTable("button");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Emoji).HasColumnName("emoji");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<ButtonVoltage>(entity =>
        {
            entity.HasKey(e => e.ButtonId);

            entity.ToTable("button_voltage");

            entity.Property(e => e.ButtonId)
                .ValueGeneratedNever()
                .HasColumnName("buttonId");
            entity.Property(e => e.Voltage).HasColumnName("voltage");

            entity.HasOne(d => d.Button).WithOne(p => p.ButtonVoltage)
                .HasForeignKey<ButtonVoltage>(d => d.ButtonId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<MazePuzzle>(entity =>
        {
            entity.ToTable("maze_puzzle");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Contents).HasColumnName("contents");
            entity.Property(e => e.Pattern).HasColumnName("pattern");
            entity.Property(e => e.Sequence).HasColumnName("sequence");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("student");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ArtScore).HasColumnName("art_score");
            entity.Property(e => e.EnglishScore).HasColumnName("english_score");
            entity.Property(e => e.GeographyScore).HasColumnName("geography_score");
            entity.Property(e => e.Grade).HasColumnName("grade");
            entity.Property(e => e.HistoryScore).HasColumnName("history_score");
            entity.Property(e => e.MathScore).HasColumnName("math_score");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.ScienceScore).HasColumnName("science_score");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
