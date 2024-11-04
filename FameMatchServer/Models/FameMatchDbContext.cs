﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FameMatchServer.Models;

public partial class FameMatchDbContext : DbContext
{
    public FameMatchDbContext()
    {
    }

    public FameMatchDbContext(DbContextOptions<FameMatchDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Audition> Auditions { get; set; }

    public virtual DbSet<Casted> Casteds { get; set; }

    public virtual DbSet<Castor> Castors { get; set; }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Picture> Pictures { get; set; }

    public virtual DbSet<Reporet> Reporets { get; set; }

    public virtual DbSet<Tip> Tips { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Initial Catalog=FameMatchDB;User ID=FameMatchAdminLogin;Password=ori1geva2;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Audition>(entity =>
        {
            entity.HasKey(e => e.AudId).HasName("PK__Audition__D2F73E1507785EFD");

            entity.HasOne(d => d.User).WithMany(p => p.Auditions).HasConstraintName("FK__Auditions__UserI__33D4B598");
        });

        modelBuilder.Entity<Casted>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Casted__1788CC4C8F9FAE05");

            entity.Property(e => e.UserId).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithOne(p => p.Casted)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Casted__UserId__2D27B809");
        });

        modelBuilder.Entity<Castor>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Castor__1788CC4CB377E852");

            entity.Property(e => e.UserId).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithOne(p => p.Castor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Castor__UserId__2A4B4B5E");
        });

        modelBuilder.Entity<File>(entity =>
        {
            entity.HasKey(e => e.FileId).HasName("PK__Files__6F0F98BFC87333DA");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Message__C87C0C9C524FB190");

            entity.HasOne(d => d.Reciver).WithMany(p => p.MessageRecivers).HasConstraintName("FK__Message__Reciver__38996AB5");

            entity.HasOne(d => d.Sender).WithMany(p => p.MessageSenders).HasConstraintName("FK__Message__SenderI__37A5467C");
        });

        modelBuilder.Entity<Picture>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Pictures__1788CC4CC1344F8F");

            entity.Property(e => e.UserId).ValueGeneratedNever();

            entity.HasOne(d => d.File).WithMany(p => p.Pictures).HasConstraintName("FK__Pictures__FileId__412EB0B6");

            entity.HasOne(d => d.User).WithOne(p => p.Picture)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pictures__UserId__403A8C7D");
        });

        modelBuilder.Entity<Reporet>(entity =>
        {
            entity.HasKey(e => e.ReporetId).HasName("PK__Reporet__B69BBCE8F44F04A2");

            entity.HasOne(d => d.Reported).WithMany(p => p.ReporetReporteds).HasConstraintName("FK__Reporet__Reporte__30F848ED");

            entity.HasOne(d => d.User).WithMany(p => p.ReporetUsers).HasConstraintName("FK__Reporet__UserId__300424B4");
        });

        modelBuilder.Entity<Tip>(entity =>
        {
            entity.HasKey(e => e.TipId).HasName("PK__Tip__2DB1A1C88E91D98F");

            entity.HasOne(d => d.User).WithMany(p => p.Tips).HasConstraintName("FK__Tip__UserId__3B75D760");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CD6C546D1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
