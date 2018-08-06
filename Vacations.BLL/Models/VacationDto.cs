using System;

namespace Vacations.BLL.Models
{
    public class VacationDto
    {
        public Guid VacationId { get; set; }
        public DateTime StartVocationDate { get; set; }
        public DateTime EndVocationDate { get; set; }
        public Guid? VacationStatusId { get; set; }
        public string Comment { get; set; }
        public Guid EmployeeId { get; set; }
        public string VacationStatusName { get; set; }
        public string TeamName { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public string EmployeeBalance { get; set; }
        public Guid VacationTypesId { get; set; }
        public string VacationTypeName { get; set; }
        public string TransactionComment { get; set; }
    }
}
