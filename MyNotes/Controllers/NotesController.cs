using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyNotes.Contracts;
using MyNotes.DataAccess;
using MyNotes.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MyNotes.Controllers;

[Authorize]
[ApiController]
[Route("notes")]
public class NotesController : ControllerBase
{
	private readonly NotesDbContext _dbContext;

	public NotesController(NotesDbContext dbContext) {
		_dbContext = dbContext;
	}

	[HttpPost]
	public IActionResult Create([FromBody] CreateNoteRequest request)
	{
        Users user = GetUser();

        Note note = new() {
            Title = request.Title,
            Description = request.Description,
            IsChecked = request.IsChecked,
            CreatedAt = DateTime.UtcNow,
            UserId = user.Id
        };

        _dbContext.Notes.Add(note);
        _dbContext.SaveChanges();
        var noteResponce = new NoteResponce(note.Id, note.Title, note.Description, note.IsChecked, note.CreatedAt);
        return new JsonResult(noteResponce);
	}

    [HttpGet]
	public IActionResult Get()
	{
        Users user = GetUser();

        return new JsonResult(_dbContext.Notes.Where(item => item.UserId == user.Id).ToArray());
	}
    private Users GetUser()
    {
        var Identity = (ClaimsIdentity?)User.Identity;
        var userData = Identity?.Claims.FirstOrDefault(item => item.Type == ClaimTypes.UserData)?.Value;
        if (userData != null)
        {
            var userId = int.Parse(userData);
            var user = _dbContext.Users.FirstOrDefault(item => item.Id == userId);
            if (user != null)
            {
                return user;
            }
            throw new Exception();
        }
        throw new Exception();
    }

}


