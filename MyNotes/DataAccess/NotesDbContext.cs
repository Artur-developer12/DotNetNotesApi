using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MyNotes.Models;
namespace MyNotes.DataAccess
{
	public class NotesDbContext : DbContext
	{
        public NotesDbContext(DbContextOptions<NotesDbContext> options): base(options) { }

        public DbSet<Note> Notes { get; set; }
        public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(opt => {
                opt.HasIndex("Email").IsUnique(true);
                opt.Property("RequestAttemts").HasDefaultValue(0);
                opt.HasMany(a => a.Notes).WithOne(a => a.User);
            });

            modelBuilder.Entity<Note>(opt =>
            {
                opt.HasOne(a => a.User).WithMany(a => a.Notes).HasForeignKey(a => a.UserId);
            });
        }
    }
}

