using System;
using System.ComponentModel.DataAnnotations;
using TTS.Shared.Utils;

namespace TTS.Shared.Models.User
{
    public class UserEditModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Lastname is required")]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Lastname is required")]
        [MaxLength(50)]
        public string SecondName { get; set; }
        [MaxLength(50)]
        public string Position { get; set; }
        [Required(ErrorMessage = "Birth Date is required")]
        [DisplayFormat(DataFormatString = "{0:MMM/d/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [BirthDateValidation]
        public DateTime BirthDate { get; set; }
    }
}