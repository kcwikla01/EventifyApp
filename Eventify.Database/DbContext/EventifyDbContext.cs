using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventify.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventify.Database.DbContext
{
    public class EventifyDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventReview> EventReviews { get; set; }
        public virtual DbSet<EventSchedule> EventSchedules { get; set; }
        public virtual DbSet<EventParticipant> EventParticipants { get; set; }

        public EventifyDbContext(){
        }

        public EventifyDbContext(DbContextOptions<EventifyDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
            );

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Owner)
                .WithMany(u => u.OwnedEvents)
                .HasForeignKey(e => e.OwnerId);

            modelBuilder.Entity<EventReview>()
                .HasOne(er => er.Event)
                .WithMany(e => e.EventReviews)
                .HasForeignKey(er => er.EventId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EventReview>()
                .HasOne(er => er.User)
                .WithMany(u => u.EventReviews)
                .HasForeignKey(er => er.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EventSchedule>()
                .HasOne(es => es.Event)
                .WithMany(e => e.EventSchedules)
                .HasForeignKey(es => es.EventId);

            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.Event)
                .WithMany(e => e.EventParticipants)
                .HasForeignKey(ep => ep.EventId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.User)
                .WithMany(u => u.EventParticipants)
                .HasForeignKey(ep => ep.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
