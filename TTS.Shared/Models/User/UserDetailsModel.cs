using System;

namespace TTS.Shared.Models.User
{
    public class UserDetailsModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondName { get; set; }
        public string Position { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public UserModel Manager { get; set; }
        
    }
}