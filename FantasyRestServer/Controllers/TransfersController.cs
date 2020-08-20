using AutoMapper;
using FantasyRestServer.Logic;
using FantasyRestServer.Models.Common;
using FantasyRestServer.Models.Data;
using FantasyRestServer.Models.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace FantasyRestServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransfersController : FantasyBaseController
    {
        public TransfersController(
            UserManager<FantasyUser> userManager,
            SignInManager<FantasyUser> signInManager,
            IMapper mapper,
            IOptions<FantasyConfig> fantasyConfig,
            IFantasyUserRepository fantasyUserRepository,
            IPositionRepository positionRepository,
            ITeamRepository teamRepository,
            IPlayerRepository playerRepository,
            ITransferRepository transferRepository) :

            base(userManager,
                signInManager,
                mapper,
                fantasyConfig,
                fantasyUserRepository,
                positionRepository,
                teamRepository,
                playerRepository,
                transferRepository)
        {
        }


        // POST: api/transfers
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Transfer>> PostTransfer([FromBody] TransferIn transferIn)
        {
            Player player = await base.PlayerRepository.ReadAsync(transferIn.PlayerRefID);
            if (!await base.CurrentUserCanAccessPlayer(player))
            {
                return Unauthorized();
            }

            if (player == null)
            {
                return NotFound();
            }

            Transfer transfer = base.TransferRepository.Read()
                .FirstOrDefault(xfer => xfer.PlayerRefID == transferIn.PlayerRefID);

            if (transfer != null)
            {
                return Conflict(
                    new ErrorOut()
                    {
                        Status = StatusCodes.Status409Conflict,
                        Title = "Conflict",
                        Errors = new List<string>() { "Player is already in Transfer List!" }
                    });
            }
            
            transfer = base.Mapper.Map<Transfer>(transferIn);
            transfer = await base.TransferRepository.CreateAsync(transfer);

            return CreatedAtRoute(nameof(GetTransfer), new { id = transfer.ID }, transfer);
        }


        // GET: api/transfers/{id}
        [HttpGet("{id}", Name = "GetTransfer")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Transfer>> GetTransfer(int id)
        {
            Transfer transfer = await base.TransferRepository.ReadAsync(id);
            if (transfer == null)
            {
                return NotFound();
            }

            return Ok(transfer);
        }

        // GET: api/transfers
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public ActionResult<IEnumerable<TransferOut>> GetTransfers()
        {
            IEnumerable<Team> teams = base.TeamRepository.Read();
            IEnumerable<Player> players = base.PlayerRepository.Read();
            IEnumerable<Transfer> transfers = base.TransferRepository.Read();

            IEnumerable<TransferOut> result =
                from transfer in transfers
                join player in players on transfer.PlayerRefID equals player.ID
                join team in teams on player.TeamRefID equals team.ID
                select new TransferOut()
                {
                    ID = player.ID,
                    FirstName = player.FirstName,
                    LastName = player.LastName,
                    Country = player.Country,
                    Age = player.Age,
                    PositionRefID = player.PositionRefID,
                    Team = team.Name,
                    Value = player.MarketValue,
                    Price = transfer.AskingPrice
                };

            bool parseFailed = false;
            List<string> errors = new List<string>();
            foreach (KeyValuePair<string, StringValues> query in HttpContext.Request.Query)
            {
                uint parsedValue = 0;

                switch (query.Key.ToLower())
                {
                    case "country":
                        result = result.Where(record =>
                            record.Country.Equals(WebUtility.UrlEncode(query.Value)));
                        break;

                    case "team":
                        result = result.Where(record =>
                            record.Team.Equals(WebUtility.UrlEncode(query.Value)));
                        break;

                    case "firstname":
                        result = result.Where(record =>
                            record.FirstName.Equals(WebUtility.UrlEncode(query.Value)));
                        break;

                    case "lastname":
                        result = result.Where(record =>
                            record.LastName.Equals(WebUtility.UrlEncode(query.Value)));
                        break;

                    case "value":
                        if (uint.TryParse(query.Value, out parsedValue))
                        {
                            result = result.Where(record => record.Value == parsedValue);
                        }
                        else
                        {
                            parseFailed = true;
                            errors.Add($"Bad numeric in value field: {query.Value}");
                        }
                        
                        break;

                    case "price":
                        if (uint.TryParse(query.Value, out parsedValue))
                        {
                            result = result.Where(record => record.Price == parsedValue);
                        }
                        else
                        {
                            parseFailed = true;
                            errors.Add($"Bad numeric in price field: {query.Value}");
                        }

                        break;

                    default:
                        break;
                }
            }

            if (parseFailed)
            {
                return UnprocessableEntity(
                    new ErrorOut()
                    {
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Title = "Unprocessable Entity",
                        Errors = errors
                    });
            }

            if ((result == null) || (result.Count() == 0))
            {
                return NotFound();
            }

            return Ok(result);
        }


        // PUT: api/transfers/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> PutTransfer(int id, [FromBody] TransferIn transferIn)
        {
            Transfer transfer = await base.TransferRepository.ReadAsync(id);
            if (!await base.CurrentUserCanAccessPlayer(transfer.PlayerRefID))
            {
                return Unauthorized();
            }

            if (transfer == null)
            {
                return NotFound();
            }

            base.Mapper.Map(transferIn, transfer);
            await base.TransferRepository.UpdateAsync(transfer);

            return NoContent();
        }


        // PATCH api/transfers/{id}
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PatchTransfer(
            int id,
            [FromBody] JsonPatchDocument<TransferIn> patchDoc)
        {
            Transfer transfer = await base.TransferRepository.ReadAsync(id);
            if (transfer == null)
            {
                return NotFound();
            }

            TransferIn transferToPatch = base.Mapper.Map<TransferIn>(transfer);
            patchDoc.ApplyTo(transferToPatch, ModelState);

            if (!TryValidateModel(transferToPatch))
            {
                return ValidationProblem(ModelState);
            }

            base.Mapper.Map(transferToPatch, transfer);
            await base.TransferRepository.UpdateAsync(transfer);

            return NoContent();
        }


        // DELETE: api/transfers/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Transfer>> DeleteTransfer(int id)
        {
            Transfer transfer = await base.TransferRepository.ReadAsync(id);
            if (!await base.CurrentUserCanAccessPlayer(transfer.PlayerRefID))
            {
                return Unauthorized();
            }

            if (transfer == null)
            {
                return NotFound();
            }

            await base.TransferRepository.DeleteAsync(id);

            return Ok(transfer);
        }
    }
}
