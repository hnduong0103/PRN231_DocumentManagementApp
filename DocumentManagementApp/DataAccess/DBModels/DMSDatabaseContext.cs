using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace DataAccess.DBModels
{
    public partial class DMSDatabaseContext : DbContext
    {
        public DMSDatabaseContext()
        {
        }

        public DMSDatabaseContext(DbContextOptions<DMSDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Data Source=(local);Initial Catalog=DMSDatabase;User ID=sa;Password=1234567890;");
                 var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                IConfigurationRoot configuration = builder.Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("Database"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Document>(entity =>
            {
                entity.ToTable("Document");

                entity.Property(e => e.DocumentName).HasMaxLength(300);

                entity.Property(e => e.DocumentPath).HasMaxLength(300);

                entity.Property(e => e.DocumentTags).HasMaxLength(300);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK__Document__Projec__2B3F6F97");

                entity.HasOne(d => d.UsersUser)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.UsersUserId)
                    .HasConstraintName("FK__Document__UsersU__2C3393D0");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");

                entity.Property(e => e.ProjectName).HasMaxLength(150);

                entity.HasOne(d => d.UsersUser)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.UsersUserId)
                    .HasConstraintName("FK__Project__UsersUs__286302EC");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Password).HasMaxLength(150);

                entity.Property(e => e.UserEmail).HasMaxLength(150);

                entity.Property(e => e.UserName).HasMaxLength(150);

                entity.Property(e => e.UserRole).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
