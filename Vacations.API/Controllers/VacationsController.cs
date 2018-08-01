﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vacations.BLL.Models;
using Vacations.BLL.Services;

namespace Vacations.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationsController : ControllerBase
    {
        private readonly IVacationsService _vacationsService;

        public VacationsController(
            IVacationsService vacationsService
            )
        {
            _vacationsService = vacationsService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IEnumerable<VacationDto> GetVacation()
        {
            return _vacationsService.Get();
        }

        [Authorize]
        [HttpGet("employee")]
        public async Task<IEnumerable<VacationDto>> GetVacationByCurrentEmployeeAsync()
        {
            return await _vacationsService.GetByCurrentEmployeeId(User);
        }

        [Authorize]
        [HttpGet("employee/{id}")]
        public IEnumerable<VacationDto> GetVacationByEmployeeId([FromRoute] Guid id)
        {
            var vacations = _vacationsService.GetByEmployeeId(id);

            return vacations;
        }

        [Authorize]
        [HttpPost("employee")]
        public async Task<IActionResult> PostVacarion([FromBody] VacationDto vacationsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _vacationsService.PostCurrentAsync(User, vacationsDto);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "TeamLead")]
        [HttpPost("employee/{id}")]
        public async Task<IActionResult> PostVacarion([FromRoute] Guid employeeId, [FromBody] VacationDto vacationsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _vacationsService.PostAsync(vacationsDto);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}