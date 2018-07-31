using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Vacations.BLL.Models;
using Vacations.BLL.Services;
using Vacations.DAL.Models;

namespace Vacations.BLL.Services
{
    public class VacationsService : IVacationsService
    {
        private readonly IMapper _mapper;
        private readonly VacationsDbContext _context;
        private readonly IUsersService _usersService;

        public VacationsService(
            VacationsDbContext context, 
            IMapper mapper,
            IUsersService usersService
            )
        {
            _mapper = mapper;
            _context = context;
            _usersService = usersService;
        }

        public VacationDto GetById(Guid idGuid)
        {
            var vacation = _context.Vacation.FirstOrDefault(e => e.EmployeeId == idGuid);
            return _mapper.Map<Vacation, VacationDto>(vacation);
        }

        public async Task<VacationDto> GetByIdAsync(Guid idGuid)
        {
            var vacation = await _context.Vacation.FirstOrDefaultAsync(e => e.EmployeeId == idGuid);
            return _mapper.Map<Vacation, VacationDto>(vacation);
        }

        public IEnumerable<VacationDto> Get()
        {
            var vacations = _context.Vacation;
            return _mapper.Map<IEnumerable<Vacation>, IEnumerable<VacationDto>>(vacations);
        }

        public async Task<IEnumerable<VacationDto>> GetByCurrentEmployeeId(ClaimsPrincipal user)
        {
            var currentUser = await _usersService.GetUserAsync(user);

            return GetByEmployeeId(currentUser.EmployeeId);
        }

        public IEnumerable<VacationDto> GetByEmployeeId(Guid idGuid)
        {
            var vacations = _context.Vacation.Where(v => v.EmployeeId == idGuid);

            return _mapper.Map<IEnumerable<Vacation>, IEnumerable<VacationDto>>(vacations);
        }

        public async Task<int> PostAsync(VacationDto vacationDto)
        {
            var vacation = new Vacation
            {
                VacationId = Guid.NewGuid(),
                StartVocationDate = vacationDto.StartVocationDate,
                EndVocationDate = vacationDto.EndVocationDate,
                VacationStatusId = vacationDto.VacationStatusId,
                Comment = vacationDto.Comment,
                EmployeeId = vacationDto.EmployeeId,
            };

            await _context.Vacation.AddAsync(vacation);

            return await _context.SaveChangesAsync();
        }
    }
}