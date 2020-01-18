using System;
using System.ComponentModel.DataAnnotations;
using Faisalman.AgeCalc;
using TTS.Shared.Utils;

namespace TTS.Shared.Models
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50)]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50)]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Second name is required")]
        [MaxLength(50)]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Birth Date is required")]
        [DisplayFormat(DataFormatString = "{0:MMM/d/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [BirthDateValidation]
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