using System;
using System.ComponentModel.DataAnnotations;

namespace TTS.Shared.Models
{
    public class RoleViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}