using System;
using System.ComponentModel.DataAnnotations;
using Faisalman.AgeCalc;
using TTS.Shared.Utils;

namespace TTS.Shared.Models.User
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
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