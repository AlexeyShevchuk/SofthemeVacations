﻿using System;
using System.Collections.Generic;

namespace Vacations.BLL.Models
{
    public partial class TeamDto
    {
        public Guid TeamId { get; set; }
        public Guid? TeamLeadId { get; set; }
        public string Name { get; set; }
    }
}
