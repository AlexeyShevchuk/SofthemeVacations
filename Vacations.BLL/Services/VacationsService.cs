using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Vacations.BLL.Models;
using Vacations.DAL.Models;

namespace Vacations.BLL.Services
{
    public class VacationsService : IVacationsService
    {
        private readonly IMapper _mapper;
        private readonly VacationsDbContext _context;
        private readonly IUsersService _usersService;
        private readonly IVacationStatusService _vacationStatusService;
        private readonly UserManager<User> _userManager;
        private readonly IEmployeesService _employeesService;
        private readonly ITransactionService _transactionService;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public VacationsService(
            VacationsDbContext context,
            IMapper mapper,
            IUsersService usersService,
            IVacationStatusService vacationStatusService,
            UserManager<User> userManager,
            IEmployeesService employeesService,
            ITransactionService transactionService,
            IEmailSender emailSender,
            IConfiguration configuration
            )
        {
            _mapper = mapper;
            _context = context;
            _usersService = usersService;
            _vacationStatusService = vacationStatusService;
            _userManager = userManager;
            _employeesService = employeesService;
            _transactionService = transactionService;
            _emailSender = emailSender;
            _configuration = configuration;
        }


        public VacationDto GetById(Guid idGuid)
        {
            var vacation = _context.Vacation
                .Include(v => v.VacationStatus)
                .Include(v => v.VacationTypes)
                .Include(v => v.Employee.Team)
                .Include(v => v.Employee)
                .FirstOrDefault(v => v.VacationId == idGuid);

            return _mapper.Map<Vacation, VacationDto>(vacation);
        }

        public async Task<VacationDto> GetByIdAsync(Guid idGuid)
        {
            var vacation = await _context.Vacation
                .Include(v => v.VacationStatus)
                .Include(v => v.VacationTypes)
                .Include(v => v.Employee.Team)
                .Include(v => v.Employee)
                .FirstOrDefaultAsync(v => v.VacationId == idGuid);

            return _mapper.Map<Vacation, VacationDto>(vacation);
        }

        public IEnumerable<VacationDto> Get()
        {
            var vacations = _context.Vacation
                .Include(v => v.VacationStatus)
                .Include(v => v.VacationTypes)
                .Include(v => v.Employee.Team)
                .Include(v => v.Employee);

            return _mapper.Map<IEnumerable<Vacation>, IEnumerable<VacationDto>>(vacations);
        }

        private IEnumerable<VacationDto> GetTeamleadVacationRequests(User currentUser)
        {
            var employees = _context.Employee
                .Include(t => t.Team)
                .Where(e => e.Team.TeamLeadId == currentUser.EmployeeId);

            var vacations = _context.Vacation
                .Include(v => v.VacationStatus)
                .Include(v => v.VacationTypes)
                .Include(v => v.Employee.Team)
                .Include(v => v.Employee)
                .Join(employees, v => v.EmployeeId, e => e.EmployeeId,
                    (v, e) => v);

            return _mapper.Map<IEnumerable<Vacation>, IEnumerable<VacationDto>>(vacations);
        }

        private IEnumerable<VacationDto> GetAdminVacationRequests(User currentUser)
        {
            var employees = _context.Employee
                .Include(t => t.Team)
                .Where(e => e.Team.TeamLeadId == currentUser.EmployeeId
                            || e.TeamId == null
                            || e.EmployeeId == e.Team.TeamLeadId);

            var vacations = _context.Vacation
                .Include(v => v.VacationStatus)
                .Include(v => v.VacationTypes)
                .Include(v => v.Employee.Team)
                .Include(v => v.Employee)
                .Join(employees, v => v.EmployeeId, e => e.EmployeeId,
                    (v, e) => v);

            return _mapper.Map<IEnumerable<Vacation>, IEnumerable<VacationDto>>(vacations);
        }

        public async Task<IEnumerable<VacationDto>> GetByCurrentEmployeeIdAsync(ClaimsPrincipal user)
        {
            var currentUser = await _usersService.GetUserAsync(user);

            return GetByEmployeeId(currentUser.EmployeeId);
        }

        public IEnumerable<VacationDto> GetByEmployeeId(Guid idGuid)
        {
            var vacations = _context.Vacation
                .Include(v => v.VacationStatus)
                .Include(v => v.Employee.Team)
                .Include(v => v.Employee)
                .Where(v => v.EmployeeId == idGuid);

            return _mapper.Map<IEnumerable<Vacation>, IEnumerable<VacationDto>>(vacations);
        }

        public async Task PutAsync(VacationDto vacationDto, ClaimsPrincipal user)
        {
            if (vacationDto.VacationStatusId == null)
            {
                throw new ArgumentException("");
            }

            var vacation = new Vacation
            {
                VacationId = vacationDto.VacationId,
                StartVocationDate = vacationDto.StartVocationDate,
                EndVocationDate = vacationDto.EndVocationDate,
                VacationStatusId = vacationDto.VacationStatusId ?? new Guid(),
                VacationTypesId = vacationDto.VacationTypesId,
                Comment = vacationDto.Comment,
                EmployeeId = vacationDto.EmployeeId,
            };

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                _context.Vacation.Update(vacation);

                if (vacation.VacationStatusId == _vacationStatusService.Get().FirstOrDefault(vs => vs.Name == "Approved").VacationStatusId)
                {
                    await _transactionService.CreateTransactionByVacationAsync(vacationDto, user);
                }

                await _context.SaveChangesAsync();

                scope.Complete();
            }

            await VacationStatusSendEmail(vacationDto, (await _context.Employee.FirstOrDefaultAsync(e => e.EmployeeId == vacation.EmployeeId)).WorkEmail);
        }

        public async Task PostCurrentAsync(ClaimsPrincipal user, VacationDto vacationDto)
        {
            var currentUser = await _usersService.GetUserAsync(user);

            vacationDto.EmployeeId = currentUser.EmployeeId;

            vacationDto.VacationId = new Guid();

            if (_vacationStatusService != null)
                vacationDto.VacationStatusId = _vacationStatusService.Get().FirstOrDefault(vs => vs.Name == "InProcess")
                    .VacationStatusId;

            if (vacationDto.VacationStatusId == null)
            {
                throw new ArgumentException("");
            }

            _context.Vacation.Add(_mapper.Map<VacationDto, Vacation>(vacationDto));

            await _context.SaveChangesAsync();

            var workEmail = (await _context.Employee
                .Include(t => t.Team.TeamLead)
                .Select(
                    e => new
                    {
                        e.Team.TeamLead.WorkEmail,
                        e.EmployeeId
                    }
                ).FirstOrDefaultAsync(e => e.EmployeeId == vacationDto.EmployeeId)).WorkEmail;

            if (workEmail == null)
            {
                foreach (var item in await _userManager.GetUsersInRoleAsync("Admin"))
                {
                    await VacationRequestSendEmail(vacationDto, item.Email);
                }
            }
            else
            {
                await VacationRequestSendEmail(vacationDto, workEmail);
            }
        }

        private async Task VacationStatusSendEmail(VacationDto vacation, string email)
        {
            var callbackUrl =
                $"{_configuration["Domain:RequestScheme"]}://{_configuration["Domain:DomainName"]}/profile";
            await _emailSender.SendEmailAsync(email, "Vacation Status Updated",
                $"Your vacation with {vacation.StartVocationDate} - {vacation.EndVocationDate} has been {vacation.VacationStatusName}." +
                $"<a href='{callbackUrl}'>https://btangular.azurewebsites.net/profile</a>");
        }

        private async Task VacationRequestSendEmail(VacationDto vacation, string email)
        {
            var callbackUrl =
                $"{_configuration["Domain:RequestScheme"]}://{_configuration["Domain:DomainName"]}/vacation-requests";
            await _emailSender.SendEmailAsync(email, "New Vacation Request",
                $"You have new vacation request." +
                $"<a href='{callbackUrl}'>https://btangular.azurewebsites.net/vacation-requests</a>");
        }

        public async Task<IEnumerable<VacationDto>> GetVacationRequestsAsync(ClaimsPrincipal user)
        {
            var currentUser = await _usersService.GetUserAsync(user);

            foreach (var role in await _userManager.GetRolesAsync(currentUser))
            {
                if (role == "Admin")
                {
                    return GetAdminVacationRequests(currentUser);
                }
                if (role == "TeamLead")
                {
                    return GetTeamleadVacationRequests(currentUser);
                }
            }

            return null;
        }
    }
}