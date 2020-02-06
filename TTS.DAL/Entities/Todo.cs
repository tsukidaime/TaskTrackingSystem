using System;

namespace TTS.DAL.Entities
{
    public class Todo : Entity
    {
        public string Content { get; set; }
        public bool Done { get; set; }
        public Guid JobId { get; set; }
        public virtual Job Job { get; set; }
        
    }
}