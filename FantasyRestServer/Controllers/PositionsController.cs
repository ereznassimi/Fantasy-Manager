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
    [Authorize(Roles = "Admin")]
    public class PositionsController : FantasyBaseController
    {
        public PositionsController(
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


        // POST: api/positions
        [HttpPost]
        public async Task<ActionResult<Position>> PostPosition([FromBody] PositionIn positionIn)
        {
            Position position = base.Mapper.Map<Position>(positionIn);
            position = await base.PositionRepository.CreateAsync(position);

            return CreatedAtRoute(nameof(GetPosition), new { id = position.ID }, position);
        }


        // GET: api/positions/{id}
        [HttpGet("{id}", Name = "GetPosition")]
        public async Task<ActionResult<Position>> GetPosition(int id)
        {
            Position position = await base.PositionRepository.ReadAsync(id);
            if (position == null)
            {
                return NotFound();
            }

            return Ok(position);
        }

        // GET: api/positions
        [HttpGet]
        public ActionResult<IEnumerable<Position>> GetPositions()
        {
            IEnumerable<Position> positions = base.PositionRepository.Read();

            return Ok(positions);
        }


        // PUT: api/positions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPosition(int id, [FromBody] PositionIn positionIn)
        {
            Position position = await base.PositionRepository.ReadAsync(id);
            if (position == null)
            {
                return NotFound();
            }

            base.Mapper.Map(positionIn, position);
            await base.PositionRepository.UpdateAsync(position);

            return NoContent();
        }


        // PATCH api/positions/{id}
        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchPosition(
            int id,
            [FromBody] JsonPatchDocument<PositionIn> patchDoc)
        {
            Position position = await base.PositionRepository.ReadAsync(id);
            if (position == null)
            {
                return NotFound();
            }

            PositionIn positionToPatch = base.Mapper.Map<PositionIn>(position);
            patchDoc.ApplyTo(positionToPatch, ModelState);

            if (!TryValidateModel(positionToPatch))
            {
                return ValidationProblem(ModelState);
            }

            base.Mapper.Map(positionToPatch, position);
            await base.PositionRepository.UpdateAsync(position);

            return NoContent();
        }


        // DELETE: api/positions/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Position>> DeletePosition(int id)
        {
            Position position = await base.PositionRepository.ReadAsync(id);
            if (position == null)
            {
                return NotFound();
            }

            await base.PositionRepository.DeleteAsync(id);

            return Ok(position);
        }
    }
}
