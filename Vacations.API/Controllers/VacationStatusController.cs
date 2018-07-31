using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vacations.BLL.Models;
using Vacations.BLL.Services;

namespace Vacations.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationStatusController : ControllerBase
    {
        private readonly IVacationStatusService _vacationStatusService;

        public VacationStatusController(
            IVacationStatusService vacationStatusService
            )
        {
            _vacationStatusService = vacationStatusService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IEnumerable<VacationStatusDto> GetVacationStatus()
        {
            return _vacationStatusService.Get();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVacationStatus([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vacationStatus = await _vacationStatusService.GetByIdAsync(id);

            if (vacationStatus == null)
            {
                return NotFound();
            }

            return Ok(vacationStatus);
        }
    }
}