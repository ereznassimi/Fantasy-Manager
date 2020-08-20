using FantasyRestServer.Models.Common;
using FantasyRestServer.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FantasyRestServer.Models.SqlServer
{
    public class SqlPositionRepository : IPositionRepository
    {
        private readonly FantasyDbContext FantasyDB;


        public SqlPositionRepository(FantasyDbContext FantasyDB)
        {
            this.FantasyDB = FantasyDB;
        }
        

        public async Task<Position> CreateAsync(Position position)
        {
            if (position == null)
            {
                throw new ArgumentNullException(nameof(position));
            }

            await this.FantasyDB.Positions.AddAsync(position);
            await this.FantasyDB.SaveChangesAsync();
            return position;
        }

        public async Task<Position> ReadAsync(int id)
        {
            return await this.FantasyDB.Positions.FindAsync(id);
        }

        public IEnumerable<Position> Read()
        {
            return this.FantasyDB.Positions.ToList();
        }

        public async Task UpdateAsync(Position position)
        {
            await this.FantasyDB.SaveChangesAsync();
        }

        public async Task<Position> DeleteAsync(int id)
        {
            Position position = await this.FantasyDB.Positions.FindAsync(id);
            if (position == null)
            {
                throw new ArgumentNullException(nameof(position));
            }

            this.FantasyDB.Positions.Remove(position);
            await this.FantasyDB.SaveChangesAsync();
            return position;
        }
    }
}
