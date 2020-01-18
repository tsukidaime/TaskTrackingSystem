using System;
using System.Collections.Generic;

namespace TTS.Shared.Models
{
    public class ChangeUserRoleViewModel
    {
        public Guid UserId { get; set; }
        public List<string> UserRoles { get; set; }
        public List<RoleViewModel> AllRoles { get; set; }
    }
}