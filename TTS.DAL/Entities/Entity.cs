using System;

namespace TTS.DAL.Entities
{
    public class Entity
    {
        public Guid Id { get; }
        
        public DateTime CreatedDate { get; }

        public Entity()
        {
            Id = new Guid();
            CreatedDate = DateTime.Now;
        }
    }
}