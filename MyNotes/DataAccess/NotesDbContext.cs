using System;
using Microsoft.EntityFrameworkCore;
using MyNotes.Models;
namespace MyNotes.DataAccess
{
	public class NotesDbContext : DbContext
	{
		private readonly IConfiguration _configuration;

		public NotesDbContext(IConfiguration configuration)
		{
			_configuration = configuration;
		}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
			string? connection = _configuration.GetConnectionString("DefaultConnection");
			Console.WriteLine(connection);
            optionsBuilder.UseNpgsql(connection);
        }
		public DbSet<Note> Notes => Set<Note>();
    }
}

