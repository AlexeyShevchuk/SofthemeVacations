using System;
using System.Collections.Generic;

namespace Vacations.Model.Models
{
    public partial class Vacation
    {
        public Guid VacationId { get; set; }
        public DateTime? StartVocationDate { get; set; }
        public DateTime? EndVocationDate { get; set; }
        public Guid? VacationStatusId { get; set; }
        public string Comment { get; set; }
        public Guid? EmployeeId { get; set; }

        public Employee Employee { get; set; }
        public VacationStatus VacationStatus { get; set; }
    }
}
