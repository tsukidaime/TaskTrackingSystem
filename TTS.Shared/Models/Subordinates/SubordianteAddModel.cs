using System;
using System.Collections.Generic;
using TTS.Shared.Models.User;

namespace TTS.Shared.Models.Subordinates
{
    public class SubordinateAddModel
    {
        public List<Guid> Subordinates { get; set; }
        public List<UserModel> AllUsers { get; set; }
        public List<UserModel> UserSubs { get; set; }
    }
}