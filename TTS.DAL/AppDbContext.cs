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
        public DbSet<JobStatus> JobStatuses { get; set; }
        public DbSet<UserJob> UserJobs { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<JobStatus>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<UserJob>()
                .HasKey(t => new {t.UserId, t.JobId});

            modelBuilder.Entity<JobStatus>()
                .HasMany(x => x.Jobs)
                .WithOne(x => x.JobStatus)
                .HasForeignKey(x => x.JobStatusId);

            modelBuilder.Entity<User>()
                .HasOne(x => x.Manager)
                .WithMany(x => x.Subordinates)
                .HasForeignKey(x => x.ManagerId);
            
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