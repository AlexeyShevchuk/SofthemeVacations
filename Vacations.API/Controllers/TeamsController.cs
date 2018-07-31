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
    public class TeamsController : ControllerBase
    {
        private readonly ITeamsService _teamsService;

        public TeamsController(ITeamsService teamsService)
        {
            _teamsService = teamsService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IEnumerable<TeamDto> GetTeams()
        {
            return _teamsService.Get();
        }

        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "TeamLead")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeam([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var team = await _teamsService.GetByIdAsync(id);

            if (team == null)
            {
                return NotFound();
            }

            return Ok(team);
        }
    }
}