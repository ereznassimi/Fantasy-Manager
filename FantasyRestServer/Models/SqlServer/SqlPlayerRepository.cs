using FantasyRestServer.Models.Common;
using FantasyRestServer.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FantasyRestServer.Models.SqlServer
{
    public class SqlPlayerRepository : IPlayerRepository
    {
        private readonly FantasyDbContext FantasyDB;


        public SqlPlayerRepository(FantasyDbContext fantasyDbContext)
        {
            this.FantasyDB = fantasyDbContext;
        }


        public async Task<Player> CreateAsync(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            await this.FantasyDB.Players.AddAsync(player);
            await this.FantasyDB.SaveChangesAsync();
            return player;
        }

        public async Task<Player> ReadAsync(int id)
        {
            return await this.FantasyDB.Players.FindAsync(id);
        }

        public IEnumerable<Player> Read()
        {
            return this.FantasyDB.Players.ToList();
        }

        public async Task UpdateAsync(Player player)
        {
            await this.FantasyDB.SaveChangesAsync();
        }

        public async Task<Player> DeleteAsync(int id)
        {
            Player player = await this.FantasyDB.Players.FindAsync(id);
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            this.FantasyDB.Players.Remove(player);
            await this.FantasyDB.SaveChangesAsync();
            return player;
        }
    }
}
