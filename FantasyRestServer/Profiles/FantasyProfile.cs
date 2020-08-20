using AutoMapper;
using FantasyRestServer.Models.Data;
using FantasyRestServer.Models.Mapping;


namespace FantasyRestServer.Profiles
{
    public class FantasyProfile: Profile
    {
        public FantasyProfile()
        {
            CreateMap<PositionIn, Position>();
            CreateMap<Position, PositionIn>();

            CreateMap<TeamIn, Team>();
            CreateMap<Team, TeamIn>();

            CreateMap<TeamInLimited, Team>();
            CreateMap<Team, TeamInLimited>();
            CreateMap<TeamIn, TeamInLimited>();

            CreateMap<PlayerIn, Player>();
            CreateMap<Player, PlayerIn>();

            CreateMap<PlayerInLimited, Player>();
            CreateMap<Player, PlayerInLimited>();
            CreateMap<PlayerIn, PlayerInLimited>();

            CreateMap<TransferIn, Transfer>();
            CreateMap<Transfer, TransferIn>();


            CreateMap<FantasyUserIn, FantasyUser>()
                .ForMember(user => user.UserName,
                    opt => opt.MapFrom(account => account.Email));
        }
    }
}
