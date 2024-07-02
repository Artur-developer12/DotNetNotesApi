using System;
namespace MyNotes.Contracts
{
	public record GetNotesResponce(List<NoteDto> Notes);

	public record NoteDto(Guid Id, string Title, string Description, DateTime CreatedAt);
}

