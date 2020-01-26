using System;
using System.ComponentModel.DataAnnotations;
using TTS.Shared.Utils;

namespace TTS.Shared.Models.User
{
    public class UserCreateModel
    {
        [Required(ErrorMessage = "Email is required")]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}