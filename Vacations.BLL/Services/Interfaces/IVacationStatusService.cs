using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vacations.BLL.Models;

namespace Vacations.BLL.Services
{
    public interface IVacationStatusService
    {
        VacationStatusDto GetById(Guid idGuid);

        Task<VacationStatusDto> GetByIdAsync(Guid idGuid);

        IEnumerable<VacationStatusDto> Get();
    }
}