using System;
using System.ComponentModel.DataAnnotations;

namespace TTS.Shared.Models.User
{
    public class UserDetailsDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondName { get; set; }
        public string Position { get; set; }
        public string PhoneNumber { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM/d/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        public UserDto Manager { get; set; }
        
    }
}