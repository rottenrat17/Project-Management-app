using Microsoft.EntityFrameworkCore;
using Comp2139Lab1.Areas.ProjectManagement.Models;
using System;

namespace Comp2139Lab1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure Project entity
            modelBuilder.Entity<Project>()
                .Property(p => p.StartDate)
                .HasConversion(
                    v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
                
            modelBuilder.Entity<Project>()
                .Property(p => p.EndDate)
                .HasConversion(
                    v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        }
    }
}
