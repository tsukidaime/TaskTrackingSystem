using System;
using System.Collections.Generic;

namespace TTS.Shared.Models.Role
{
    public class RoleAssignDto
    {
        public Guid UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}