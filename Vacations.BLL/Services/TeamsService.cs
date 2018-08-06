using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Vacations.BLL.Models;
using Vacations.DAL.Models;

namespace Vacations.BLL.Services
{
    public class TeamsService : ITeamsService
    {
        private readonly IMapper _mapper;
        private readonly VacationsDbContext _context;

        public TeamsService(VacationsDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public TeamDto GetById(Guid idGuid)
        {
            var team = _context.Team
                .Include(t => t.TeamLead)
                .Include(t => t.Employee)
                .FirstOrDefault(e => e.TeamId == idGuid);

            return _mapper.Map<Team, TeamDto>(team);
        }

        public async Task<TeamDto> GetByIdAsync(Guid idGuid)
        {
            var team = await _context.Team
                .Include(t => t.TeamLead)
                .Include(t => t.Employee)
                .FirstOrDefaultAsync(e => e.TeamId == idGuid);

            return _mapper.Map<Team, TeamDto>(team);
        }

        public IEnumerable<TeamDto> Get()
        {
            var teams = _context.Team
                .Include(t => t.TeamLead)
                .Include(t => t.Employee);

            return _mapper.Map<IEnumerable<Team>, IEnumerable<TeamDto>>(teams);
        }
    }
}
