using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly IMapper _mapper;
        private readonly IEmployeesService _employeesService;

        public EmployeesController(
            IMapper mapper,
            IEmployeesService employeesService)
        {
            _mapper = mapper;
            _employeesService = employeesService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IEnumerable<EmployeeDtoList> Get()
        {
            return _employeesService.Get();
        }

        [Authorize]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentEmployee()
        {
            var userDto = await _userManager.GetUserAsync(User);

            if (userDto == null)
            {
                return BadRequest("User == null");
            }
            
            var employeeDto = await _employeesService.GetByIdAsync(userDto.EmployeeId);

            return Ok(employeeDto);
        }

        // PUT: api/Employees/5
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> PutEmployee([FromBody] EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _employeesService.PutAsync(employeeDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/Employees
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
                await _employeesService.PostAsync(employeeDto);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}