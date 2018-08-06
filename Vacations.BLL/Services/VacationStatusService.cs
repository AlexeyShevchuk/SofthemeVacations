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
    public class VacationStatusService : IVacationStatusService
    {
        private readonly IMapper _mapper;
        private readonly VacationsDbContext _context;

        public VacationStatusService(VacationsDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public VacationStatusDto GetById(Guid idGuid)
        {
            var vacation = _context.VacationStatus.FirstOrDefault(vs => vs.VacationStatusId == idGuid);
            return _mapper.Map<VacationStatus, VacationStatusDto>(vacation);
        }

        public async Task<VacationStatusDto> GetByIdAsync(Guid idGuid)
        {
            var vacation = await _context.VacationStatus.FirstOrDefaultAsync(vs => vs.VacationStatusId == idGuid);
            return _mapper.Map<VacationStatus, VacationStatusDto>(vacation);
        }

        public IEnumerable<VacationStatusDto> Get()
        {
            var vacations = _context.VacationStatus;
            return _mapper.Map<IEnumerable<VacationStatus>, IEnumerable<VacationStatusDto>>(vacations);
        }
    }
}
