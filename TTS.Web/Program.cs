using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TTS.BLL.Utils;
using TTS.DAL.Entities;

namespace TTS.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var _userManager = serviceProvider.GetService<UserManager<User>>();
                    var _roleManager = serviceProvider.GetService<RoleManager<IdentityRole<Guid>>>();
                    SeedData.Initialize(_userManager, _roleManager).Wait();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}