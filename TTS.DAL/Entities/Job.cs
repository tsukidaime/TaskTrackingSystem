using System;
using System.Collections.Generic;

namespace TTS.DAL.Entities
{
    public class Job : Entity
    {
        public string Name { get; set; }
        public ushort Progress { get; set; }
        public string Description { get; set; }
        public DateTime StartedTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Deadline { get; set; }
        public virtual List<UserJob> UserJobs { get; }
        
        public Guid JobStatusId { get; set; }
        
        public virtual JobStatus JobStatus { get; set; }

        public Job()
        {
            UserJobs = new List<UserJob>();
        }
    }
}