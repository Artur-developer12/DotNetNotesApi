using System;
namespace MyNotes.Models
{
	public class Note
	{
		public Note(string Title, string Description)
		{
			this.Title = Title;
			this.Description = Description;
			CreatedAt = DateTime.Now;
		}
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}

