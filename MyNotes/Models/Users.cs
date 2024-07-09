using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MyNotes.Models
{
	public class Users
	{
		public int Id { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
		[Required]
		public string Password { get; set; } = string.Empty;
		public int RequestAttemts { get; set; } = 0;
		public DateTime? LastRequestAt { get; set; }
		public List<Note> Notes { get; set; } = new ();
	}
}

