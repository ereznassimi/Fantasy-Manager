using FantasyRestServer.Models.Common;
using FantasyRestServer.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FantasyRestServer.Models.SqlServer
{
    public class SqlTeamRepository : ITeamRepository
    {
        private readonly FantasyDbContext FantasyDB;


        public SqlTeamRepository(FantasyDbContext FantasyDB)
        {
            this.FantasyDB = FantasyDB;
        }
        

        public async Task<Team> CreateAsync(Team team)
        {
            if (team == null)
            {
                throw new ArgumentNullException(nameof(team));
            }

            await this.FantasyDB.Teams.AddAsync(team);
            await this.FantasyDB.SaveChangesAsync();
            return team;
        }

        public async Task<Team> ReadAsync(int id)
        {
            return await this.FantasyDB.Teams.FindAsync(id);
        }

        public IEnumerable<Team> Read()
        {
            return this.FantasyDB.Teams.ToList();
        }

        public async Task UpdateAsync(Team team)
        {
            await this.FantasyDB.SaveChangesAsync();
        }

        public async Task<Team> DeleteAsync(int id)
        {
            Team team = await this.FantasyDB.Teams.FindAsync(id);
            if (team == null)
            {
                throw new ArgumentNullException(nameof(team));
            }

            this.FantasyDB.Teams.Remove(team);
            await this.FantasyDB.SaveChangesAsync();
            return team;
        }
    }
}
