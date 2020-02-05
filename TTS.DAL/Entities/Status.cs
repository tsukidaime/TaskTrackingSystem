using System.Collections.Generic;

namespace TTS.DAL.Entities
{
    public class Status : Entity
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public virtual List<Job> Jobs { get; }

        public Status()
        {
            Jobs = new List<Job>();
        }
    }
}