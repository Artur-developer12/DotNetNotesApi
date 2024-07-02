using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyNotes.Contracts;
using MyNotes.DataAccess;
using MyNotes.Models;

namespace MyNotes.Controllers;

[ApiController]
[Route("[controller]")]
public class NotesController : ControllerBase
{
	private readonly NotesDbContext _dbContext;
	public NotesController(NotesDbContext dbContext) {
		_dbContext = dbContext;
	}
	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateNoteRequest request)
	{
		var note = new Note(request.Title, request.Description);
		await _dbContext.Notes.AddAsync(note);
		await _dbContext.SaveChangesAsync();

		return Ok();
	}

	[HttpGet]
	public async Task<IActionResult> Get([FromQuery] GetNotesRequest request)
	{
		return Ok(await _dbContext.Notes.ToListAsync());
	}

}


