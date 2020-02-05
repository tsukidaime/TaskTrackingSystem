using System;
using System.Collections.Generic;
using TTS.Shared.Models.User;

namespace TTS.Shared.Models.Employee
{
    public class EmployeeDto
    {
        public Guid UserId { get; set; }
        public Guid EmployeeId { get; set; }
        public List<UserDto> Employees { get; set; }
    }
}