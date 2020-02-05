using System;
using System.ComponentModel.DataAnnotations;

namespace TTS.Shared.Models.Role
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}