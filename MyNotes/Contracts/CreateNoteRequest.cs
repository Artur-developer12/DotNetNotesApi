using System;
using System.ComponentModel.DataAnnotations;

namespace MyNotes.Contracts
{
	public class CreateNoteRequest
	{
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public bool IsChecked { get; set; } = false;
    };

    public record NoteResponce (int Id, string Title, string Description, bool IsChecked, DateTime CreateAt);
}

