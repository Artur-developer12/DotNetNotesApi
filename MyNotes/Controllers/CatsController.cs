using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyNotes.Services;
using MyNotes.DataAccess;
using System.Security.Claims;
using MyNotes.Models;


namespace MyNotes.Controllers
{
    [Authorize]
    [Route("cats")]
    public class CatsController : ControllerBase
    {
        private readonly CatApiService _catApi;
        private readonly NotesDbContext _dbContext;
        public CatsController(CatApiService catsApiService, NotesDbContext dbContext)
        {
            _catApi = catsApiService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetFact() {
            try
            {
                var fact = await _catApi.GetFact();
                var user = GetUser();

                if (user.LastRequestAt == null)
                {

                    user.LastRequestAt = DateTime.UtcNow;
                    user.RequestAttemts++;
                    _dbContext.SaveChanges();
                    return new JsonResult(new { fact });
                }

                var lastTime = user.LastRequestAt.Value;
                user.LastRequestAt = DateTime.UtcNow;
                var now = DateTime.Now;
                var timeSpent = now.Subtract(lastTime);

                if (timeSpent.Minutes >= 3)
                {
                    user.RequestAttemts = 0;
                    _dbContext.SaveChanges();
                    return new JsonResult(new { fact });

                }

                if (user.RequestAttemts >= 3)
                {
                    return BadRequest("Превышено количество попыток");
                }

                user.RequestAttemts++;
                _dbContext.SaveChanges();
                return new JsonResult(new { fact });


            } catch
            {
                return BadRequest("api not available");
            }
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
}

