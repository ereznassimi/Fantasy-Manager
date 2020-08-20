using FantasyRestServer.Models.Data;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace FantasyRestServer.Models.Common
{
    public interface ITeamRepository
    {
        Task<Team> CreateAsync(Team team);

        Task<Team> ReadAsync(int id);

        IEnumerable<Team> Read();

        Task UpdateAsync(Team team);

        Task<Team> DeleteAsync(int id);
    }
}
