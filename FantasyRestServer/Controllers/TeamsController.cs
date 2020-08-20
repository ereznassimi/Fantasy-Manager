using AutoMapper;
using FantasyRestServer.Logic;
using FantasyRestServer.Models.Common;
using FantasyRestServer.Models.Data;
using FantasyRestServer.Models.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace FantasyRestServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : FantasyBaseController
    {
        public TeamsController(
            UserManager<FantasyUser> userManager,
            SignInManager<FantasyUser> signInManager,
            IMapper mapper,
            IOptions<FantasyConfig> fantasySectionConfig,
            IFantasyUserRepository fantasyUserRepository,
            IPositionRepository positionRepository,
            ITeamRepository teamRepository,
            IPlayerRepository playerRepository,
            ITransferRepository transferRepository) :

            base(userManager,
                signInManager,
                mapper,
                fantasySectionConfig,
                fantasyUserRepository,
                positionRepository,
                teamRepository,
                playerRepository,
                transferRepository)
        {
        }


        // POST: api/teams
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Team>> PostTeam([FromBody] TeamIn teamIn)
        {
            Team team = base.Mapper.Map<Team>(teamIn);
            team = await base.TeamRepository.CreateAsync(team);

            return CreatedAtRoute(nameof(GetTeam), new { id = team.ID }, team);
        }


        // GET: api/teams/{id}
        [HttpGet("{id}", Name = "GetTeam")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Team>> GetTeam(int id)
        {
            if (!await base.CurrentUserCanAccessTeam(id))
            {
                return Unauthorized();
            }

            Team team = await base.TeamRepository.ReadAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            return Ok(team);
        }

        // GET: api/teams
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<Team>> GetTeams()
        {
            IEnumerable<Team> teams = base.TeamRepository.Read();

            return Ok(teams);
        }


        // PUT: api/teams/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> PutTeam(int id, [FromBody] TeamIn teamIn)
        {
            if (!await base.CurrentUserCanAccessTeam(id))
            {
                return Unauthorized();
            }

            Team team = await base.TeamRepository.ReadAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            if (HttpContext.User.IsInRole("User"))
            {
                TeamInLimited teamInLimited = new TeamInLimited();
                base.Mapper.Map(teamIn, teamInLimited);
                base.Mapper.Map(teamInLimited, team);
            }
            else // admin
            {
                base.Mapper.Map(teamIn, team);
            }

            await base.TeamRepository.UpdateAsync(team);

            return NoContent();
        }


        // PATCH api/teams/{id}
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PatchTeam(
            int id,
            [FromBody] JsonPatchDocument<TeamIn> patchDoc)
        {
            Team team = await base.TeamRepository.ReadAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            TeamIn teamToPatch = base.Mapper.Map<TeamIn>(team);
            patchDoc.ApplyTo(teamToPatch, ModelState);

            if (!TryValidateModel(teamToPatch))
            {
                return ValidationProblem(ModelState);
            }

            base.Mapper.Map(teamToPatch, team);
            await base.TeamRepository.UpdateAsync(team);

            return NoContent();
        }


        // DELETE: api/teams/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Team>> DeleteTeam(int id)
        {
            Team team = await base.TeamRepository.ReadAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            await base.TeamRepository.DeleteAsync(id);

            return Ok(team);
        }
    }
}
