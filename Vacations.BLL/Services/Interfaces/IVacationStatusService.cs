using System;
using System.Collections.Generic;
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