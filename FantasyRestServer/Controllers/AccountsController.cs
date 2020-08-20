using AutoMapper;
using FantasyRestServer.Logic;
using FantasyRestServer.Models.Common;
using FantasyRestServer.Models.Data;
using FantasyRestServer.Models.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;


namespace FantasyRestServer.Controllers
{
    [ApiController]
    [AllowAnonymous]
    public class AccountsController : FantasyBaseController
    {
        public AccountsController(
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


        // POST: api/accounts/signup
        [HttpPost]
        [Route("api/[controller]/signup")]
        public async Task<ActionResult<FantasyUserOut>> SignUp([FromBody] FantasyUserIn fantasyUserIn)
        {
            // take care of the account management
            IdentityResult result = await base.FantasyUserRepository.CreateAsync(fantasyUserIn);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            FantasyUser fantasyUser = await base.FantasyUserRepository.ReadAsync(fantasyUserIn.Email);
            await base.SignInManager.SignInAsync(fantasyUser, isPersistent: false);
            await base.UserManager.AddToRoleAsync(fantasyUser, "User");

            // build a new team
            IEnumerable<Position> positions = base.PositionRepository.Read();
            TeamIn teamIn = Agency.BuildNewTeam(base.FantasySectionConfig, positions);
            Team team = base.Mapper.Map<Team>(teamIn);
            team = await base.TeamRepository.CreateAsync(team);

            // generate players
            IEnumerable<PlayerIn> playersIn = Agency.GeneratePlayers(
                base.FantasySectionConfig,
                positions,
                team.ID);

            foreach (PlayerIn playerIn in playersIn)
            {
                Player player = base.Mapper.Map<Player>(playerIn);
                player = await base.PlayerRepository.CreateAsync(player);
            }

            // update team id in user
            fantasyUser.TeamRefID = team.ID;
            await base.FantasyUserRepository.UpdateAsync(fantasyUser);

            // prepare response
            IEnumerable<Player> players = base.PlayerRepository.Read()
                .Where(player => player.TeamRefID == team.ID);

            FantasyUserOut fantasyUserOut = new FantasyUserOut()
            {
                Email = fantasyUser.Email,
                Team = team,
                Players = players
            };

            return Ok(fantasyUserOut);
        }


        // POST: api/accounts/login
        [Route("api/[controller]/login")]
        [HttpPost]
        public async Task<ActionResult<FantasyUserOut>> Login([FromBody] FantasyUserIn fantasyUserIn)
        {
            SignInResult result = await base.SignInManager.PasswordSignInAsync(
                fantasyUserIn.Email,
                fantasyUserIn.Password,
                isPersistent: true,
                lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            // prepare response
            FantasyUser fantasyUser = await base.FantasyUserRepository.ReadAsync(fantasyUserIn.Email);
            Team team = await base.TeamRepository.ReadAsync(fantasyUser.TeamRefID);

            IEnumerable<Player> players = base.PlayerRepository.Read()
                .Where(player => player.TeamRefID == fantasyUser.TeamRefID);

            FantasyUserOut fantasyUserOut = new FantasyUserOut()
            {
                Email = fantasyUser.Email,
                Team = team,
                Players = players
            };

            return Ok(fantasyUserOut);
        }


        // POST: api/accounts/logout
        [Route("api/[controller]/logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await base.SignInManager.SignOutAsync();
            return Ok();
        }
    }
}
