using AutoMapper;
using FantasyRestServer.Models.Common;
using FantasyRestServer.Models.Data;
using FantasyRestServer.Models.Mapping;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace FantasyRestServer.Models.SqlServer
{
    public class SqlFantasyUserRepository : IFantasyUserRepository
    {
        private readonly FantasyDbContext FantasyDB;

        private readonly UserManager<FantasyUser> UserManager;
        private readonly SignInManager<FantasyUser> SignInManager;

        private readonly IMapper Mapper;


        public SqlFantasyUserRepository(
            FantasyDbContext fantasyDbContext,
            UserManager<FantasyUser> userManager,
            SignInManager<FantasyUser> signInManager,
            IMapper mapper)
        {
            this.FantasyDB = fantasyDbContext;
            this.UserManager = userManager;
            this.SignInManager = signInManager;
            this.Mapper = mapper;
        }


        public async Task<IdentityResult> CreateAsync(FantasyUserIn fantasyUserIn)
        {
            if (fantasyUserIn == null)
            {
                throw new ArgumentNullException(nameof(fantasyUserIn));
            }

            FantasyUser fantasyUser = this.Mapper.Map<FantasyUser>(fantasyUserIn);
            IdentityResult result = await this.UserManager.CreateAsync(fantasyUser, fantasyUserIn.Password);

            return result;
        }


        public async Task<FantasyUser> ReadAsync(string email)
        {
            FantasyUser fantasyUser = await this.UserManager.FindByNameAsync(email);
            return fantasyUser;
        }

        public IEnumerable<FantasyUser> Read()
        {
            return this.UserManager.Users;
        }

        public async Task UpdateAsync(FantasyUser fantasyUser)
        {
            await this.FantasyDB.SaveChangesAsync();
        }

        public async Task<FantasyUser> DeleteAsync(string email)
        {
            FantasyUser fantasyUser = await this.UserManager.FindByNameAsync(email);
            if (fantasyUser == null)
            {
                throw new ArgumentNullException(nameof(FantasyUser));
            }

            await this.UserManager.DeleteAsync(fantasyUser);
            return fantasyUser;
        }
    }
}
