using System;
namespace MyNotes.Models
{
	public class Note
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
        public bool IsChecked { get; set; }
		public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public Users? User { get; set; }
    }
}

