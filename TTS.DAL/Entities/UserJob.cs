using System;

namespace TTS.DAL.Entities
{
    public class UserJob
    {
        public Guid UserId { get; set; }
        public Guid JobId { get; set; }
        public virtual User User { get; set; }
        public virtual Job Job { get; set; }
    }
}