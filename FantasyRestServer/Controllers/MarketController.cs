using AutoMapper;
using FantasyRestServer.Logic;
using FantasyRestServer.Models.Common;
using FantasyRestServer.Models.Data;
using FantasyRestServer.Models.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FantasyRestServer.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class MarketController : FantasyBaseController
    {
        private Random Rand = new Random();


        public MarketController(
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


        // POST: api/market/buy
        [HttpPost]
        [Route("api/[controller]/buy")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> Buy([FromBody] MarketIn marketIn)
        {
            // make sure player exists and he is in transfer list
            Player player = await base.PlayerRepository.ReadAsync(marketIn.PlayerID);
            if (player == null)
            {
                return NotFound();
            }

            Transfer transfer = base.TransferRepository.Read()
                .FirstOrDefault(xfer => xfer.PlayerRefID == marketIn.PlayerID);

            if (transfer == null)
            {
                return NotFound();
            }

            // make sure both selling & buying teams do exist
            int sellingTeamID = player.TeamRefID;
            Team sellingTeam = await base.TeamRepository.ReadAsync(sellingTeamID);
            if (sellingTeam == null)
            {
                return NotFound();
            }

            FantasyUser fantasyUser =
                await this.FantasyUserRepository.ReadAsync(HttpContext.User.Identity.Name);

            int buyingTeamID = fantasyUser.TeamRefID;
            Team buyingTeam = await base.TeamRepository.ReadAsync(buyingTeamID);
            if (buyingTeam == null)
            {
                return NotFound();
            }

            if (buyingTeamID == sellingTeamID)
            {
                return UnprocessableEntity(
                    new ErrorOut()
                    {
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Title = "Unprocessable Entity",
                        Errors = new List<string>() { "Manager cannot buy players from his own team!" }
                    });
            }

            // make sure buying teams has enough budget
            if (buyingTeam.AvailableBudget < transfer.AskingPrice)
            {
                return UnprocessableEntity(
                    new ErrorOut()
                    {
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Title = "Unprocessable Entity",
                        Errors = new List<string>() { "Not enough budget!" }
                    });
            }

            // complete transactions in selling team
            sellingTeam.TotalValue -= player.MarketValue;
            sellingTeam.AvailableBudget += transfer.AskingPrice;
            await base.TeamRepository.UpdateAsync(sellingTeam);

            // update player's value
            int minValueIncrease = base.FantasySectionConfig.Value.MinValueIncrease;
            int maxValueIncrease = base.FantasySectionConfig.Value.MaxValueIncrease;

            int increasePercent = Math.Max(-100, this.Rand.Next(minValueIncrease, maxValueIncrease));
            uint realIncrease = (uint)(player.MarketValue * increasePercent / 100);

            player.MarketValue += realIncrease;
            player.TeamRefID = buyingTeamID;
            await base.PlayerRepository.UpdateAsync(player);

            // complete transactions in buying team
            buyingTeam.AvailableBudget -= transfer.AskingPrice;
            buyingTeam.TotalValue += player.MarketValue;
            await base.TeamRepository.UpdateAsync(buyingTeam);

            // finally remove the transfer record
            Transfer toDelete = await base.TransferRepository.DeleteAsync(transfer.ID);

            return Ok(toDelete);
        }
    }
}
