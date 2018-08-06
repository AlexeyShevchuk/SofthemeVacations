using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Vacations.BLL.Models;
using Vacations.DAL.Models;

namespace Vacations.BLL.Services
{
    public interface IUsersService
    {
        Task<string> GenerateJwtTokenAsync(string email, User user);
        Task<TokenDto> GetTokenAsync(string authorizationHeader);
        Task ForgotPassword(string email);
        Task<User> GetUserAsync(ClaimsPrincipal user);
        Task ResetPasswordAsync(string employeeId, string code, string passwordReset);
        Task<string> GetUserRoleId(User user);
        Task<string> GetUserRole(User user);
        Task UpdateUserRole(User user, string role);
        Task SetUserRole(User user, string role);
        Task UpdateUser(User user);
        IEnumerable<RoleDto> GetRoles();
        Task CreateAsync(User user, string password);
        Task<User> FindByEmailAsync(string email);       
    }
}
