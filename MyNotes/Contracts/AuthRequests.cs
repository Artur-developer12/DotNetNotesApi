using System.ComponentModel.DataAnnotations;
namespace MyNotes.Contracts
{
	public class AuthRequest
	{
		[Required, EmailAddress]
		public string Email { get; set; } = string.Empty;
		[Required]
		public string Password { get; set; } = string.Empty;
    };
	public record AuthResponceDto(string AccessToken, string RefreshToken);
}

