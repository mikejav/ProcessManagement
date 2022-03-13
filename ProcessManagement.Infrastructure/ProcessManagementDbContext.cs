using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProcessManagement.Core.Entities;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace ProcessManagement.Infrastructure
{
    public class ProcessManagementDbContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<User> Users { get; set; }

        public ProcessManagementDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>(eb => {
                eb.HasKey(p => p.Id);
                eb.Property(p => p.Id)
                    .ValueGeneratedOnAdd();

                eb.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                eb.Property(p => p.Description)
                    .IsRequired(false)
                    .HasColumnType("TEXT");
            });

            modelBuilder.Entity<WorkItem>(eb => {
                eb.HasOne(w => w.Project).WithMany();

                eb.Property(w => w.Id)
                    .ValueGeneratedOnAdd();

                eb.Property(w => w.Status)
                    .IsRequired();
            });

            modelBuilder.Entity<User>(eb => {
                eb.HasKey(u => u.Id);
                eb.Property(p => p.Id).ValueGeneratedOnAdd();
                eb.Property(u => u.Name).IsRequired().IsUnicode();
                eb.Property(u => u.Email).IsRequired().IsUnicode();
                eb.Property(u => u.Password).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
