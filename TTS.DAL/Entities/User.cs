using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TTS.DAL.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Position { get; set; }
        public virtual List<UserJob> UserJobs { get; }
        public Guid? ManagerId { get; set; }
        public virtual User Manager { get; set; }
        public virtual List<User> Subordinates { get; }
        
        public User()
        {
            UserJobs = new List<UserJob>();
            Subordinates = new List<User>();
        }
    }
}