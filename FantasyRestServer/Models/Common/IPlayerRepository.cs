using FantasyRestServer.Models.Data;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace FantasyRestServer.Models.Common
{
    public interface IPlayerRepository
    {
        Task<Player> CreateAsync(Player player);

        Task<Player> ReadAsync(int id);

        IEnumerable<Player> Read();

        Task UpdateAsync(Player player);

        Task<Player> DeleteAsync(int id);
    }
}
