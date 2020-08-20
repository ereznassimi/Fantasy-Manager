using AutoMapper;
using FantasyRestServer.Logic;
using FantasyRestServer.Models.Common;
using FantasyRestServer.Models.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace FantasyRestServer.Controllers
{
    public class FantasyBaseController : ControllerBase
    {
        protected readonly UserManager<FantasyUser> UserManager;
        protected readonly SignInManager<FantasyUser> SignInManager;

        protected readonly IMapper Mapper;

        protected readonly IOptions<FantasyConfig> FantasySectionConfig;

        protected readonly IFantasyUserRepository FantasyUserRepository;

        protected readonly IPositionRepository PositionRepository;
        protected readonly ITeamRepository TeamRepository;
        protected readonly IPlayerRepository PlayerRepository;
        protected readonly ITransferRepository TransferRepository;


        public FantasyBaseController(
            UserManager<FantasyUser> userManager,
            SignInManager<FantasyUser> signInManager,
            IMapper mapper,
            IOptions<FantasyConfig> fantasySectionConfig,
            IFantasyUserRepository fantasyUserRepository,
            IPositionRepository positionRepository,
            ITeamRepository teamRepository,
            IPlayerRepository playerRepository,
            ITransferRepository transferRepository)
        {
            this.UserManager = userManager;
            this.SignInManager = signInManager;
            this.Mapper = mapper;
            this.FantasySectionConfig = fantasySectionConfig;
            this.FantasyUserRepository = fantasyUserRepository;
            this.PositionRepository = positionRepository;
            this.TeamRepository = teamRepository;
            this.PlayerRepository = playerRepository;
            this.TransferRepository = transferRepository;
        }


        protected bool CanAccessTeam(FantasyUser fantasyUser, int teamID)
        {
            if (HttpContext.User.IsInRole("Admin"))
            {
                return true;
            }

            if (HttpContext.User.IsInRole("User"))
            {
                return (fantasyUser.TeamRefID == teamID);
            }

            return false;
        }

        protected async Task<bool> CurrentUserCanAccessTeam(int id)
        {
            if (HttpContext.User.IsInRole("Admin"))
            {
                return true;
            }

            if (HttpContext.User.IsInRole("User"))
            {
                FantasyUser fantasyUser =
                    await this.FantasyUserRepository.ReadAsync(HttpContext.User.Identity.Name);

                return (fantasyUser.TeamRefID == id);
            }

            return false;
        }

        protected async Task<bool> CurrentUserCanAccessTeam(Team team)
        {
            return await this.CurrentUserCanAccessTeam(team.ID);
        }

        protected bool CanAccessPlayer(FantasyUser fantasyUser, Player player)
        {
            if (HttpContext.User.IsInRole("Admin"))
            {
                return true;
            }

            if (HttpContext.User.IsInRole("User"))
            {
                return (fantasyUser.TeamRefID == player.TeamRefID);
            }

            return false;
        }

        protected async Task<bool> CurrentUserCanAccessPlayer(Player player)
        {
            if (HttpContext.User.IsInRole("Admin"))
            {
                return true;
            }

            if (HttpContext.User.IsInRole("User"))
            {
                FantasyUser fantasyUser =
                    await this.FantasyUserRepository.ReadAsync(HttpContext.User.Identity.Name);

                return (fantasyUser.TeamRefID == player.TeamRefID);
            }

            return false;
        }

        protected async Task<bool> CurrentUserCanAccessPlayer(int id)
        {
            if (HttpContext.User.IsInRole("Admin"))
            {
                return true;
            }

            if (HttpContext.User.IsInRole("User"))
            {
                FantasyUser fantasyUser =
                    await this.FantasyUserRepository.ReadAsync(HttpContext.User.Identity.Name);

                Player player = await this.PlayerRepository.ReadAsync(id);

                return (fantasyUser.TeamRefID == player.TeamRefID);
            }

            return false;
        }
    }
}
