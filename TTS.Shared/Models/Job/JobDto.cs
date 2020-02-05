using System;
using System.Collections.Generic;
using TTS.Shared.Models.Status;

namespace TTS.Shared.Models.Job
{
    public class JobDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ushort Progress { get; set; }
        public DateTime Deadline { get; set; }
        public StatusDto Status { get; set; }
        public List<Guid> Users { get; set; }
    }
}