using AutoMapper;
using FantasyRestServer.Logic;
using FantasyRestServer.Models.Common;
using FantasyRestServer.Models.Data;
using FantasyRestServer.Models.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace FantasyRestServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IConfigurationRoot ConfigurationRoot { get; }
            

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            this.Configuration = configuration;

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            this.ConfigurationRoot = builder.Build();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<FantasyConfig>(Configuration.GetSection("FantasyConfig"));

            services.AddDbContextPool<FantasyDbContext>(
                opt => opt.UseSqlServer(
                    Configuration.GetConnectionString("FantasyDbConnection")));

            services.AddIdentity<FantasyUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            }).AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<FantasyDbContext>();

            services.AddScoped<IFantasyUserRepository, SqlFantasyUserRepository>();
            services.AddScoped<IPlayerRepository, SqlPlayerRepository>();
            services.AddScoped<IPositionRepository, SqlPositionRepository>();
            services.AddScoped<ITeamRepository, SqlTeamRepository>();
            services.AddScoped<ITransferRepository, SqlTransferRepository>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            UserManager<FantasyUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();


            app.UseAuthentication();
            FantasyDbInitializer.SeedData(userManager, roleManager);

            app.UseMvc();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Welcome to Fantasy Manager Server!");
            });
        }
    }
}
