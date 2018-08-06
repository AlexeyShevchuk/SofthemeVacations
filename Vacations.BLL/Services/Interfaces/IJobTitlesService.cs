using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vacations.BLL.Models;

namespace Vacations.BLL.Services
{
    public interface IJobTitlesService
    {
        JobTitleDto GetById(Guid idGuid);
        Task<JobTitleDto> GetByIdAsync(Guid idGuid);

        IEnumerable<JobTitleDto> Get();
    }
}