using System.Collections.Generic;

namespace TTS.DAL.Entities
{
    public class JobStatus : Entity
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public virtual List<Job> Jobs { get; }

        public JobStatus()
        {
            Jobs = new List<Job>();
        }
    }
}