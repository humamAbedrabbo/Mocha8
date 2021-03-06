using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using AMS.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AMS.Models;
using AMS.Services;
using Hangfire;
using Hangfire.MemoryStorage;

namespace AMS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public string DbConnectionType => Configuration?["Database"];
        public bool IsInMemory => "Memory" == DbConnectionType;
        public string DbConnectionString => Configuration?.GetConnectionString(DbConnectionType ?? "DefaultConnection");

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (IsInMemory)
            {
                services.AddDbContext<AmsContext>(options =>
                    options.UseInMemoryDatabase(DbConnectionString));
            }
            else
            {
                services.AddDbContext<AmsContext>(options =>
                    options.UseSqlServer(DbConnectionString));
            }
            services.AddDefaultIdentity<AmsUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.User.RequireUniqueEmail = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddRoles<AmsRole>()
                .AddEntityFrameworkStores<AmsContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHttpContextAccessor();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICodeGenerator, CodeGenerator>();
            services.AddScoped<ITicketGenerator, TicketGenerator>();

            services.AddHangfire(cfg => {
                cfg.UseMemoryStorage();
            });
            services.AddHangfireServer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseHangfireDashboard();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
            });
        }
    }
}
