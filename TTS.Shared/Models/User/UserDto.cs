using System;
using System.ComponentModel.DataAnnotations;
using Faisalman.AgeCalc;
using TTS.Shared.Utils;

namespace TTS.Shared.Models.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM/d/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public int Age
        {
            get
            {
                var calc = new Age(BirthDate, DateTime.Now);
                return calc.Years;
            }
        }
    }
}