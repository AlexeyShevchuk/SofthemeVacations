using System;
using System.ComponentModel.DataAnnotations;

namespace Vacations.BLL.Models
{
    public class EmployeeDto
    {
        public Guid EmployeeId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string WorkEmail { get; set; }
        public string PersonalEmail { get; set; }
        public string TelephoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public string Skype { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int? Balance { get; set; }
        public string TeamName { get; set; }
        public string TeamLeadName { get; set; }
        public string TeamLeadSurname { get; set; }
        public Guid? EmployeeStatusId { get; set; }
        public Guid? JobTitleId { get; set; }
        public Guid? TeamId { get; set; }
        public Guid? TeamLeadId { get; set; }
        public string ImgUrl { get; set; }

        [Required]
        public string RoleId { get; set; }
    }
}
