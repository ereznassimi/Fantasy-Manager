using FantasyRestServer.Models.Data;
using FantasyRestServer.Models.Mapping;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace FantasyRestServer.Models.Common
{
    public interface IFantasyUserRepository
    {
        Task<IdentityResult> CreateAsync(FantasyUserIn fantasyUserIn);

        Task<FantasyUser> ReadAsync(string email);

        IEnumerable<FantasyUser> Read();

        Task UpdateAsync(FantasyUser fantasyUser);

        Task<FantasyUser> DeleteAsync(string email);
    }
}
