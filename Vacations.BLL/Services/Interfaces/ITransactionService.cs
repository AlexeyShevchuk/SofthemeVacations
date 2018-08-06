using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Vacations.BLL.Models;

namespace Vacations.BLL.Services
{
    public interface ITransactionService
    {
        Task CreateTransactionByVacationAsync(VacationDto vacation, ClaimsPrincipal user);
        Task CreateTransactionByAdminAsync(Guid EmployeeId, int balance, ClaimsPrincipal user);
    }
}