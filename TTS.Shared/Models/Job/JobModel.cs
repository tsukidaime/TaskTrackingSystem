using System;
using System.Collections.Generic;
using TTS.DAL.Entities;

namespace TTS.Shared.Models.Job
{
    public class JobModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ushort Progress { get; set; }
        public DateTime Deadline { get; set; }
        public JobStatus JobStatus { get; set; }
        public List<Guid> Users { get; set; }
    }
}