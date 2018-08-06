using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Vacations.BLL.Models;

namespace Vacations.BLL.Services
{
    public interface IEmployeesService
    {
        IEnumerable<EmployeeDtoList> Get();
        Task<EmployeeDto> GetByIdAsync(Guid id);
        Task PutAsync(EmployeeDto employeeDto, ClaimsPrincipal admin);
        Task PostAsync(EmployeeDto employeeDto, ClaimsPrincipal admin);
    }
}
