using System;
using System.Collections.Generic;

namespace TTS.Shared.Models.User
{
    public class UserRoleModel
    {
        public Guid UserId { get; set; }
        public List<string> UserRoles { get; set; }
    }
}