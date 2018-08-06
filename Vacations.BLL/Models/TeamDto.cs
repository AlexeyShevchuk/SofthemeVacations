using System;

namespace Vacations.BLL.Models
{
    public partial class TeamDto
    {
        public Guid TeamId { get; set; }
        public Guid? TeamLeadId { get; set; }
        public string TeamLeadName { get; set; }
        public string TeamLeadSurname { get; set; }
        public string Name { get; set; }
        public int MembersNumber { get; set; }
    }
}
