using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vacations.BLL.Models;

namespace Vacations.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RolesController(
             RoleManager<IdentityRole> roleManager,
             IMapper mapper
             )
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IEnumerable<RoleDto>> Get()
        {
            return _mapper.Map<IEnumerable<IdentityRole>, IEnumerable<RoleDto>>(await _roleManager.Roles.ToListAsync());
        }
    }
}