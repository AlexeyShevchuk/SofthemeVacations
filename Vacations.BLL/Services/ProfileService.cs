using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Vacations.BLL.Models;
using Vacations.DAL.Models;

namespace Vacations.BLL.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IMapper _mapper;
        private readonly VacationsDbContext _context;

        public ProfileService(VacationsDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ProfileDto> GetByIdAsync(Guid idGuid)
        {
            var employee = await _context.Employee
                .Include(e => e.Team)
                .Include(e => e.Team.TeamLead)
                .Include(e => e.EmployeeStatus)
                .Include(e => e.JobTitle)
                .FirstOrDefaultAsync(e => e.EmployeeId == idGuid);

            if (employee == null)
            {
                return null;
            }

            var profile = _mapper.Map<Employee, ProfileDto>(employee);

            return profile;
        }
    }
}
