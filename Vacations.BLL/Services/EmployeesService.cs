using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Vacations.BLL.Models;
using Vacations.DAL.Models;

namespace Vacations.BLL.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IMapper _mapper;
        private readonly VacationsDbContext _context;
        private readonly IUsersService _usersService;

        public EmployeesService(
            VacationsDbContext context,
            IMapper mapper,
             IUsersService usersService
            )
        {
            _mapper = mapper;
            _context = context;
            _usersService = usersService;
        }

        public IEnumerable<EmployeeDtoList> Get()
        {
            var employees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeDtoList>>(_context.Employee);
            return employees;
        }

        public async Task<EmployeeDto> GetByCurrentUserAsync(ClaimsPrincipal user)
        {
            var currentUser = await _usersService.GetUserAsync(user);

            return await GetByIdAsync(currentUser.EmployeeId);
        }

        public async Task<EmployeeDto> GetByIdAsync(Guid id)
        {
            var employee = await _context.Employee
                .Include(e => e.Team.TeamLead)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee != null)
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
                    TeamName = employee.Team.Name,
                    TeamLeadName = employee.Team.TeamLead.Name,
                    TeamLeadSurname = employee.Team.TeamLead.Surname,
                    Balance = employee.Balance,
                    EmployeeStatusId = employee.EmployeeStatusId,
                    JobTitleId = employee.JobTitleId,
                    TeamId = employee.TeamId,
                    TeamLeadId = employee.Team.TeamLead.EmployeeId
                };
            throw new NullReferenceException("Employee not found!");
        }

        public async Task<int> PutAsync(EmployeeDto employeeDto)
        {
            var employee = await _context.Employee.FindAsync(employeeDto.EmployeeId);

            var user = await _userManager.FindByEmailAsync(employee.WorkEmail);
            
            employee.Name = employeeDto.Name;
            employee.Surname = employeeDto.Surname;
            employee.PersonalEmail = employeeDto.PersonalEmail;
            employee.WorkEmail = employeeDto.WorkEmail;
            employee.TelephoneNumber = employeeDto.TelephoneNumber;
            employee.Birthday = employeeDto.Birthday;
            employee.Skype = employeeDto.Skype;
            employee.StartDate = employeeDto.StartDate;
            employee.EmployeeStatusId = employeeDto.EmployeeStatusId;
            employee.EndDate = employeeDto.EndDate;
            employee.JobTitleId = employeeDto.JobTitleId;
            employee.Balance = employeeDto.Balance;

            _context.Employee.Update(employee);

            user.UserName = employeeDto.WorkEmail;

            user.Email = employeeDto.WorkEmail;

            var result1 = await _context.SaveChangesAsync();

            var result2 = await _userManager.UpdateAsync(user);

            //foreach (var role in await _userManager.GetRolesAsync(user))
            //{
            //    var result3 = await _userManager.RemoveFromRoleAsync(user, role);
            //}

            //var result4 = await _userManager.AddToRoleAsync(user, employeeDto.Role);

            return result1;
        }

        public async Task<int> PostAsync(EmployeeDto employeeDto)
        {
            var employee = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                Name = employeeDto.Name,
                Surname = employeeDto.Surname,
                WorkEmail = employeeDto.WorkEmail,
                TelephoneNumber = employeeDto.TelephoneNumber,
                Birthday = employeeDto.Birthday,
                Skype = employeeDto.Skype,
                StartDate = employeeDto.StartDate,
                EmployeeStatusId = employeeDto.EmployeeStatusId,
                EndDate = employeeDto.EndDate,
                JobTitleId = employeeDto.JobTitleId,
                Balance = employeeDto.Balance
            };

            await _context.Employee.AddAsync(employee);

            var user = new User
            {
                UserName = employeeDto.WorkEmail,
                Email = employeeDto.WorkEmail,
                EmployeeId = employee.EmployeeId
            };

            var result1 = await _context.SaveChangesAsync();
            var result2 = await _userManager.CreateAsync(user, "asd123Q!");

            //TODO: Add role
            var result3 = await _userManager.AddToRoleAsync(user, "Admin");

            return result1;
        }
    }
}