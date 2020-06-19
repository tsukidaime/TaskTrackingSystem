using System;

namespace TTS.Shared.Models.Job
{
    public class JobChangeStatusDto
    {
        public Guid JobId { get; set; }
        public Guid StatusId { get; set; }
    }
}