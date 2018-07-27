using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vacations.API.Models;
using Vacations.BLL.Models;
using Vacations.BLL.Services;
using Vacations.DAL.Models;

namespace Vacations.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUsersService _usersService;
        private readonly IEmployeesService _employeesService;

        public EmployeesController(IMapper mapper, IUsersService usersService, IEmployeesService employeesService)
        {
            _mapper = mapper;
            _usersService = usersService;
            _employeesService = employeesService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            return _employeesService.Get();
        }

        [Authorize]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentEmployee()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState + " IsNotValid");
            }

            var currentUserEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            var userDto = await _usersService.GetByEmailAsync(currentUserEmail);
            var userModel = _mapper.Map<UserDto, UserModel>(userDto);

            var employeeDto = await _employeesService.GetByIdAsync(userModel.EmployeeId);

            return Ok(employeeDto);
        }

        // PUT: api/Employees/5
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> PutEmployee([FromBody] EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState + " IsNotValid");
            }

            try
            {
                await _employeesService.PutAsync(employeeDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(ModelState + " IsNotValid DbUpdateConcurrencyException");
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