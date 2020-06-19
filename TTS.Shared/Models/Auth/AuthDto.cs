using System.ComponentModel.DataAnnotations;

namespace TTS.Shared.Models.Auth
{
    public class AuthDto
    {
        [Required] public string Username { get; set; }

        [Required] public string Password { get; set; }

    }
}