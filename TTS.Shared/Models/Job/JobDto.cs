using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TTS.Shared.Models.Status;

namespace TTS.Shared.Models.Job
{
    public class JobDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM/d/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }
        public Guid StatusId { get; set; }
        public StatusDto Status { get; set; }
    }
}