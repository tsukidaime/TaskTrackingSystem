using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TTS.DAL.Entities;

namespace TTS.DAL
{
    public class AppDbContext : IdentityDbContext<User,IdentityRole<Guid>,Guid>
    {
        public override DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<UserJob> UserJobs { get; set; }
        public DbSet<Todo> Todos { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            if (Database.EnsureCreated())
            {
                Database.Migrate();
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Status>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<UserJob>()
                .HasKey(t => new {t.UserId, t.JobId});
            modelBuilder.Entity<Todo>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Job>()
                .Property(x => x.Progress)
                .HasDefaultValue(0);
            modelBuilder.Entity<Todo>()
                .Property(x => x.Done)
                .HasDefaultValue(false);

            modelBuilder.Entity<Status>()
                .HasMany(x => x.Jobs)
                .WithOne(x => x.Status)
                .HasForeignKey(x => x.StatusId);

            modelBuilder.Entity<User>()
                .HasOne(x => x.Manager)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.ManagerId);
            
            modelBuilder.Entity<Job>()
                .HasMany(x => x.Todos)
                .WithOne(x => x.Job)
                .HasForeignKey(x => x.JobId);
            
            modelBuilder.Entity<UserJob>()
                .HasOne(t => t.User)
                .WithMany(p => p.UserJobs)
                .HasForeignKey(pc=>pc.UserId);
            
            modelBuilder.Entity<UserJob>()
                .HasOne(t => t.Job)
                .WithMany(p => p.UserJobs)
                .HasForeignKey(pc=>pc.JobId);

            base.OnModelCreating(modelBuilder);
        }
    }
}