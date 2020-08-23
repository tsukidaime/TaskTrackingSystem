using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TTS.DAL.Entities;
using TTS.Shared.Models.Status;
using TTS.Shared.Models.Todo;
using TTS.Shared.Models.User;
namespace TTS.Shared.Models.Job
{
    public class JobDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM/d/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime StartedTime { get; set; }
        public double DaysBehind => (StartedTime - DateTime.Now).TotalDays;
        public double DaysLeft => (Deadline - DateTime.Now).TotalDays;
        public double DoneByDays => (EndTime - StartedTime).TotalDays;
        [DisplayFormat(DataFormatString = "{0:MMM/d/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime EndTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM/d/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }
        public List<UserDto> Users { get; set; }
        public Guid StatusId { get; set; }
        public StatusDto Status { get; set; }
    }
}