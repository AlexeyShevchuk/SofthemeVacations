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
    public class JobTitlesService: IJobTitlesService
    {
        private readonly IMapper _mapper;
        private readonly VacationsDbContext _context;

        public JobTitlesService(VacationsDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public JobTitleDto GetById(Guid idGuid)
        {
            var jobtitle = _context.JobTitle.FirstOrDefault(e => e.JobTitleId == idGuid);
            return _mapper.Map<JobTitle, JobTitleDto>(jobtitle);
        }

        public async Task<JobTitleDto> GetByIdAsync(Guid idGuid)
        {
            var jobtitle = await _context.JobTitle.FirstOrDefaultAsync(e => e.JobTitleId == idGuid);
            return _mapper.Map<JobTitle, JobTitleDto>(jobtitle);
        }

        public IEnumerable<JobTitleDto> Get()
        {
            var jobtitle = _context.JobTitle;
            return _mapper.Map<IEnumerable<JobTitle>, IEnumerable<JobTitleDto>>(jobtitle);
        }
    }
}
