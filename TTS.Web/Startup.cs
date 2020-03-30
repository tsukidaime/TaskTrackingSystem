using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TTS.BLL.Options;
using TTS.BLL.Services;
using TTS.BLL.Services.Abstract;
using TTS.DAL;
using TTS.DAL.Entities;
using TTS.Shared.Infrastructure;
using TTS.Shared.Profiles;

namespace TTS.Web
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddAutoMapper(typeof(JobProfile));
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));
            services.AddIdentity<User, IdentityRole<Guid>>(opts =>
                {
                    opts.User.RequireUniqueEmail = true;
                    opts.SignIn.RequireConfirmedEmail = false;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireUppercase = false;
                    opts.Password.RequireDigit = false;
                    opts.Password.RequiredLength = 3;
                    //opts.SignIn.RequireConfirmedAccount = true;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services.AddControllersWithViews(opt =>
            {
                opt.CacheProfiles.Add("Default",
                    new CacheProfile()
                    {
                        Duration = 300,
                        Location = ResponseCacheLocation.None
                    });
            });
            services.AddRazorPages();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });
            #region Transient
            services.AddTransient<IUserService,UserService>();
            services.AddTransient<IJobService,JobService>();
            services.AddTransient<IRoleService,RoleService>();
            services.AddTransient<IEmployeeService,EmployeeService>();
            services.AddTransient<IStatusService,StatusService>();
            services.AddTransient<ITodoService, TodoService>();
            #endregion
            
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(3);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton<OperationHelper>();
            services.AddSingleton<IdentityErrorHelper>();
            services.Configure<AuthMessageSenderOptions>(Configuration); 
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Add("Cache-Control", "public,max-age=600");
                }
            });
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}