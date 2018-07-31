using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Vacations.BLL.Models;

namespace Vacations.BLL.Services
{
    public interface IVacationsService
    {
        VacationDto GetById(Guid idGuid);

        Task<VacationDto> GetByIdAsync(Guid idGuid);

        IEnumerable<VacationDto> Get();

        IEnumerable<VacationDto> GetByEmployeeId(Guid idGuid);

        Task<IEnumerable<VacationDto>> GetByCurrentEmployeeId(ClaimsPrincipal user);

        Task<int> PostAsync(VacationDto employeeDto);
    }
}
