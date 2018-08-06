using System;

namespace Vacations.BLL.Models
{
    public class EmployeeDtoList
    {
        public Guid EmployeeId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? Birthday { get; set; }
        public string WorkEmail { get; set; }
        public string PersonalEmail { get; set; }
        public string TelephoneNumber { get; set; }
        public string Skype { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? EmployeeStatusId { get; set; }
        public Guid? JobTitleId { get; set; }
        public Guid? TeamId { get; set; }
        public int Balance { get; set; }
    }
}
