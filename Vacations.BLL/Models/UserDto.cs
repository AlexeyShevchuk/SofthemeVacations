using System;

namespace Vacations.BLL.Models
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public Guid EmployeeId { get; set; }
        public string Password { get; set; }
        public string PersonalEmail { get; set; }
        public Guid? RoleId { get; set; }
    }
}
