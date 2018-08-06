using System;
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
    public class EmployeesController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IEmployeesService _employeesService;

        public EmployeesController(
            IUsersService usersService,
            IEmployeesService employeesService)
        {
            _usersService = usersService;
            _employeesService = employeesService;
        }

        [Authorize(Roles = "Admin, TeamLead")]
        [HttpGet]
        public IEnumerable<EmployeeDtoList> Get()
        {
            return _employeesService.Get();
        }

        [Authorize]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentEmployee()
        {
            var userDto = await _usersService.GetUserAsync(User);

            if (userDto == null)
            {
                return NotFound("User == null");
            }

            var employeeDto = await _employeesService.GetByIdAsync(userDto.EmployeeId);

            if (employeeDto == null)
            {
                return NotFound("Employee == null");
            }

            return Ok(employeeDto);
        }

        [Authorize]
        [Authorize(Roles = "Admin, TeamLead")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee([FromRoute] Guid id)
        {
            var employeeDto = await _employeesService.GetByIdAsync(id);

            if (employeeDto == null)
            {
                return NotFound("Employee not found");
            }

            return Ok(employeeDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> PutEmployee([FromBody] EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _employeesService.PutAsync(employeeDto, User);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromBody] EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _employeesService.PostAsync(employeeDto, User);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}