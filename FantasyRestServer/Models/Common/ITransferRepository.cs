using FantasyRestServer.Models.Data;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace FantasyRestServer.Models.Common
{
    public interface ITransferRepository
    {
        Task<Transfer> CreateAsync(Transfer transfer);

        Task<Transfer> ReadAsync(int id);

        IEnumerable<Transfer> Read();

        Task UpdateAsync(Transfer transfer);

        Task<Transfer> DeleteAsync(int id);
    }
}
