using System;
using Chatter.Areas.Identity.Data;
using Chatter.Data;
using Chatter.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Chatter.Areas.Identity.IdentityHostingStartup))]
namespace Chatter.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ChatterContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ChatterContextConnection")));

               
                services.AddDefaultIdentity<ChatterUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<ChatterContext>();

                services.AddTransient<IUnitOfWork, UnitOfWork>();
            });
        }
    }
}