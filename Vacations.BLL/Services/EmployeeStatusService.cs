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
    public class EmployeeStatusService : IEmployeeStatusService
    {
        private readonly IMapper _mapper;
        private readonly VacationsDbContext _context;

        public EmployeeStatusService(VacationsDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public EmployeeStatusDto GetById(Guid idGuid)
        {
            var employeeStatus = _context.EmployeeStatus.FirstOrDefault(e => e.EmployeeStatusId == idGuid);
            return _mapper.Map<EmployeeStatus, EmployeeStatusDto>(employeeStatus);
        }

        public async Task<EmployeeStatusDto> GetByIdAsync(Guid idGuid)
        {
            var employeeStatus = await _context.EmployeeStatus.FirstOrDefaultAsync(e => e.EmployeeStatusId == idGuid);
            return _mapper.Map<EmployeeStatus, EmployeeStatusDto>(employeeStatus);
        }

        public IEnumerable<EmployeeStatusDto> Get()
        {
            var employeeStatus = _context.EmployeeStatus;
            return _mapper.Map<IEnumerable<EmployeeStatus>, IEnumerable<EmployeeStatusDto>>(employeeStatus);
        }
    }
}
