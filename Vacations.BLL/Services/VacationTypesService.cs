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
    public class VacationTypesService : IVacationTypesService
    {
        private readonly IMapper _mapper;
        private readonly VacationsDbContext _context;

        public VacationTypesService(VacationsDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public VacationTypesDto GetById(Guid idGuid)
        {
            var vacationTypes = _context.VacationTypes.FirstOrDefault(vt => vt.VacationTypesId == idGuid);

            return _mapper.Map<VacationTypes, VacationTypesDto>(vacationTypes);
        }

        public async Task<VacationTypesDto> GetByIdAsync(Guid idGuid)
        {
            var vacationTypes = await _context.VacationTypes.FirstOrDefaultAsync(vt => vt.VacationTypesId == idGuid);

            return _mapper.Map<VacationTypes, VacationTypesDto>(vacationTypes);
        }

        public IEnumerable<VacationTypesDto> Get()
        {
            var vacationTypes = _context.VacationTypes;

            return _mapper.Map<IEnumerable<VacationTypes>, IEnumerable<VacationTypesDto>>(vacationTypes);
        }
    }
}
