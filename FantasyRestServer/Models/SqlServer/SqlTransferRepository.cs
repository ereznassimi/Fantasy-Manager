using FantasyRestServer.Models.Common;
using FantasyRestServer.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FantasyRestServer.Models.SqlServer
{
    public class SqlTransferRepository : ITransferRepository
    {
        private readonly FantasyDbContext FantasyDB;


        public SqlTransferRepository(FantasyDbContext FantasyDB)
        {
            this.FantasyDB = FantasyDB;
        }
        

        public async Task<Transfer> CreateAsync(Transfer transfer)
        {
            if (transfer == null)
            {
                throw new ArgumentNullException(nameof(transfer));
            }

            await this.FantasyDB.Transfers.AddAsync(transfer);
            await this.FantasyDB.SaveChangesAsync();
            return transfer;
        }

        public async Task<Transfer> ReadAsync(int id)
        {
            return await this.FantasyDB.Transfers.FindAsync(id);
        }

        public IEnumerable<Transfer> Read()
        {
            return this.FantasyDB.Transfers.ToList();
        }

        public async Task UpdateAsync(Transfer transfer)
        {
            await this.FantasyDB.SaveChangesAsync();
        }

        public async Task<Transfer> DeleteAsync(int id)
        {
            Transfer transfer = await this.FantasyDB.Transfers.FindAsync(id);
            if (transfer == null)
            {
                throw new ArgumentNullException(nameof(transfer));
            }

            this.FantasyDB.Transfers.Remove(transfer);
            await this.FantasyDB.SaveChangesAsync();
            return transfer;
        }
    }
}
