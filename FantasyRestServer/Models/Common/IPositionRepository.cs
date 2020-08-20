using FantasyRestServer.Models.Data;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace FantasyRestServer.Models.Common
{
    public interface IPositionRepository
    {
        Task<Position> CreateAsync(Position position);

        Task<Position> ReadAsync(int id);

        IEnumerable<Position> Read();

        Task UpdateAsync(Position position);

        Task<Position> DeleteAsync(int id);
    }
}
