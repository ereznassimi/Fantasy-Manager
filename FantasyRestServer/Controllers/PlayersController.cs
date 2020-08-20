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
    public class PlayersController : FantasyBaseController
    {
        public PlayersController(
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


        // POST: api/players
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Player>> PostPlayer([FromBody] PlayerIn playerIn)
        {
            Player player = base.Mapper.Map<Player>(playerIn);
            player = await base.PlayerRepository.CreateAsync(player);

            return CreatedAtRoute(nameof(GetPlayer), new { id = player.ID }, player);
        }


        // GET: api/players/{id}
        [HttpGet("{id}", Name = "GetPlayer")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            Player player = await base.PlayerRepository.ReadAsync(id);
            if (!await base.CurrentUserCanAccessPlayer(player))
            {
                return Unauthorized();
            }

            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        // GET: api/players
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<Player>> GetPlayers()
        {
            IEnumerable<Player> players = base.PlayerRepository.Read();

            return Ok(players);
        }


        // PUT: api/players/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> PutPlayer(int id, [FromBody] PlayerIn playerIn)
        {
            Player player = await base.PlayerRepository.ReadAsync(id);
            if (!await base.CurrentUserCanAccessPlayer(player))
            {
                return Unauthorized();
            }

            if (player == null)
            {
                return NotFound();
            }
            
            if (HttpContext.User.IsInRole("User"))
            {
                PlayerInLimited playerInLimited = new PlayerInLimited();
                base.Mapper.Map(playerIn, playerInLimited);
                base.Mapper.Map(playerInLimited, player);
            }
            else // admin
            {
                base.Mapper.Map(playerIn, player);
            }

            await base.PlayerRepository.UpdateAsync(player);

            return NoContent();
        }


        // PATCH api/players/{id}
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PatchPlayer(
            int id,
            [FromBody] JsonPatchDocument<PlayerIn> patchDoc)
        {
            Player player = await base.PlayerRepository.ReadAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            PlayerIn playerToPatch = base.Mapper.Map<PlayerIn>(player);
            patchDoc.ApplyTo(playerToPatch, ModelState);

            if (!TryValidateModel(playerToPatch))
            {
                return ValidationProblem(ModelState);
            }

            base.Mapper.Map(playerToPatch, player);
            await base.PlayerRepository.UpdateAsync(player);

            return NoContent();
        }


        // DELETE: api/players/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Player>> DeletePlayer(int id)
        {
            Player player = await base.PlayerRepository.ReadAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            if (player == null)
            {
                return NotFound();
            }

            await base.PlayerRepository.DeleteAsync(id);

            return Ok(player);
        }
    }
}
