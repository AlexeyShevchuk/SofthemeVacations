using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Vacations.BLL.Models;
using Vacations.DAL.Models;

namespace Vacations.BLL.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IMapper _mapper;
        private readonly VacationsDbContext _context;
        private readonly IUsersService _usersService;
        private readonly IImagesService _imagesService;
        private readonly ITransactionService _transactionService;

        public EmployeesService(
             VacationsDbContext context,
             IMapper mapper,
             IUsersService usersService,
             IImagesService imagesService,
             ITransactionService transactionService
             )
        {
            _mapper = mapper;
            _context = context;
            _usersService = usersService;
            _imagesService = imagesService;
            _transactionService = transactionService;
        }

        public IEnumerable<EmployeeDtoList> Get()
        {
            var employees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeDtoList>>(_context.Employee);
            return employees;
        }

        public async Task<EmployeeDto> GetByIdAsync(Guid id)
        {
            var employee = await _context.Employee
                .Include(e => e.Team.TeamLead)
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
            {
                return null;
            }

            return new EmployeeDto()
            {
                EmployeeId = employee.EmployeeId,
                Name = employee.Name,
                Surname = employee.Surname,
                Birthday = employee.Birthday,
                PersonalEmail = employee.PersonalEmail,
                WorkEmail = employee.WorkEmail,
                TelephoneNumber = employee.TelephoneNumber,
                Skype = employee.Skype,
                StartDate = employee.StartDate,
                EndDate = employee.EndDate,
                TeamName = employee.Team?.Name,
                TeamLeadName = employee.Team?.TeamLead?.Name,
                TeamLeadSurname = employee.Team?.TeamLead?.Surname,
                Balance = employee.Balance,
                EmployeeStatusId = employee.EmployeeStatusId,
                JobTitleId = employee.JobTitleId,
                TeamId = employee.TeamId,
                TeamLeadId = employee.Team?.TeamLead?.EmployeeId,
                RoleId = await _usersService.GetUserRoleId(employee.User),
                ImgUrl = employee.ImgUrl
            };
        }

        public async Task PutAsync(EmployeeDto employeeDto, ClaimsPrincipal admin)
        {
            var employee = await _context.Employee.FindAsync(employeeDto.EmployeeId);

            if (employee == null)
            {
                throw new InvalidOperationException();
            }

            var user = await _usersService.FindByEmailAsync(employee.WorkEmail);

            if (user == null)
            {
                throw new InvalidOperationException();
            }

            _context.Entry(employee).State = EntityState.Detached;
            
            var newBalance = (employeeDto.Balance ?? CulcBalance()) - employee.Balance;

            var newEmployee = _mapper.Map<EmployeeDto, Employee>(employeeDto);

            newEmployee.Balance = employeeDto.Balance ?? CulcBalance();

            newEmployee.ImgUrl = await _imagesService.GetUrlAsync(employeeDto.ImgUrl);

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                _context.Employee.Update(newEmployee);

                user.UserName = employeeDto.WorkEmail;

                user.Email = employeeDto.WorkEmail;

                await _transactionService.CreateTransactionByAdminAsync(newEmployee.EmployeeId, newBalance, admin);

                await _usersService.UpdateUser(user);

                await _usersService.UpdateUserRole(user, employeeDto.RoleId);

                await _context.SaveChangesAsync();

                scope.Complete();
            }
        }

        private const float _startBalance = 28;

        private int CulcBalance()
        {
            DateTime dateTimeNow = DateTime.Now;

            var days = DateTime.IsLeapYear(dateTimeNow.Year) ? 366 : 365;

            var vacationPerDay = _startBalance / (days);

            var daysLeft = days - dateTimeNow.DayOfYear;

            return (int)(vacationPerDay * daysLeft);
        }

        public async Task PostAsync(EmployeeDto employeeDto, ClaimsPrincipal admin)
        {
            employeeDto.EmployeeId = Guid.NewGuid();

            employeeDto.Balance = employeeDto.Balance ?? CulcBalance();

            employeeDto.ImgUrl = await _imagesService.GetUrlAsync(employeeDto.ImgUrl);

            var employee = _mapper.Map<EmployeeDto, Employee>(employeeDto);

            var user = new User
            {
                UserName = employeeDto.WorkEmail,
                Email = employeeDto.WorkEmail,
                EmployeeId = employee.EmployeeId
            };

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await _context.Employee.AddAsync(employee);

                await _usersService.CreateAsync(user, "asd123Q!");

                await _usersService.SetUserRole(user, employeeDto.RoleId);

                await _transactionService.CreateTransactionByAdminAsync(employee.EmployeeId, employee.Balance, admin);

                await _usersService.ForgotPassword(user.Email);

                await _context.SaveChangesAsync();

                scope.Complete();
            }
        }
    }
}