using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OTSSiteMVC.Data;
using OTSSiteMVC.Entities;

namespace OTSSiteMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<AppIdentityUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<AppIdentityRole>>();
                    var dbContext = services.GetRequiredService<ApplicationDbContext>();
                    var env = services.GetRequiredService<IHostingEnvironment>();
                    DefaultAccountSeeds.Seed(userManager, roleManager, dbContext, env);
                }
                catch (Exception e)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(e, "An error ocurred seeding default account data.");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
