using AMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Data
{
    public class DbInitializer
    {
        public static void Initialize(AmsContext context, IConfiguration config, IServiceProvider serviceProvider)
        {
            var initializer = new DbInitializer();
            initializer.Seed(context, config, serviceProvider);
        }

        private void Seed(AmsContext context, IConfiguration config, IServiceProvider serviceProvider)
        {
            var dbConnectionType = config?["Database"];
            var isInMemory = ("Memory" == dbConnectionType);
            var DbConnectionString = config?.GetConnectionString(dbConnectionType ?? "DefaultConnection");
            
            if (!isInMemory)
            {
                context.Database.Migrate();
            }

            var um = serviceProvider.GetService<UserManager<AmsUser>>();
            var rm = serviceProvider.GetService<RoleManager<AmsRole>>();

            if (!context.Roles.Any(r => r.NormalizedName == "ADMIN"))
            {
                // Add Admin role
                var adminRole = new AmsRole { 
                    Name = "Admin", 
                };

                rm.CreateAsync(adminRole);
            }


            // Add default administrator
            var defaultAdmin = new
            {
                Username = config.GetValue<string>("Registration:Administrator:Username"),
                DisplayName = config.GetValue<string>("Registration:Administrator:DisplayName"),
                Password = config.GetValue<string>("Registration:Administrator:Password"),
                Email = config.GetValue<string>("Registration:Administrator:Email"),
                Phone = config.GetValue<string>("Registration:Administrator:Phone"),
                Company = config.GetValue<string>("Registration:Administrator:Company"),
                JobTitle = config.GetValue<string>("Registration:Administrator:JobTitle")
            };

            bool defaultAdminDefined = (defaultAdmin != null)
                && (defaultAdmin.Username != null)
                && (defaultAdmin.Email != null)
                && (defaultAdmin.Password != null);

            if(defaultAdminDefined)
            {
                if (!context.Users.Any(x => x.NormalizedUserName == defaultAdmin.Username.ToUpper()))
                {
                    // Add Users
                    var user = new AmsUser
                    {
                        UserName = defaultAdmin.Username,
                        Email = defaultAdmin.Email,
                        Company = defaultAdmin.Company,
                        DisplayName = defaultAdmin.DisplayName ?? defaultAdmin.Username,
                        JobTitle = defaultAdmin.JobTitle,
                        PhoneNumber = defaultAdmin.Phone
                    };

                    um.CreateAsync(user, defaultAdmin.Password);
                    um.AddToRoleAsync(user, "Admin");
                }
            }

            // Add default tenant
            var defaultTenant = new
            {
                Activate = config.GetValue<bool>("Registration:DefaultTenant:Activate"),
                Name = config.GetValue<string>("Registration:DefaultTenant:Name"),
                Username = config.GetValue<string>("Registration:DefaultTenant:Username"),
                DisplayName = config.GetValue<string>("Registration:DefaultTenant:DisplayName"),
                Password = config.GetValue<string>("Registration:DefaultTenant:Password"),
                Email = config.GetValue<string>("Registration:DefaultTenant:Email"),
                Phone = config.GetValue<string>("Registration:DefaultTenant:Phone"),
                Company = config.GetValue<string>("Registration:DefaultTenant:Company"),
                JobTitle = config.GetValue<string>("Registration:DefaultTenant:JobTitle")
            };

            bool defaultTenantDefined = (defaultTenant != null)
                && (defaultTenant.Name != null)
                && (defaultTenant.Username != null)
                && (defaultTenant.Email != null)
                && (defaultTenant.Password != null);

            if(defaultTenantDefined)
            {
                if (!context.Tenants.Any(x => x.Name == defaultTenant.Name))
                {
                    if (defaultTenant != null && defaultTenant.Activate)
                    {
                        // Add default tenant
                        var tenant = new Tenant
                        {
                            Name = defaultTenant.Name
                        };
                        context.Tenants.Add(tenant);
                        context.SaveChanges();

                        // Add default tenant administration role
                        var tenantRoleName = $"{defaultTenant.Name}$Admin";
                        if (!context.Roles.Any(r => r.NormalizedName == tenantRoleName.ToUpper()))
                        {
                            var role = new AmsRole
                            {
                                Name = tenantRoleName,
                                NormalizedName = tenantRoleName.ToUpper(),
                                ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                                TenantId = tenant.Id
                            };
                            context.Roles.Add(role);
                            context.SaveChanges();
                        }

                        // Add default tenant administrator
                        AmsUser tenantAdmin = um.FindByNameAsync(defaultTenant.Username).Result;
                        if (tenantAdmin == null)
                        {
                            tenantAdmin = new AmsUser
                            {
                                UserName = defaultTenant.Username,
                                Email = defaultTenant.Email,
                                Company = defaultTenant.Company,
                                DisplayName = defaultTenant.DisplayName,
                                JobTitle = defaultTenant.JobTitle,
                                PhoneNumber = defaultTenant.Phone,
                                TenantId = tenant.Id
                            };

                            um.CreateAsync(tenantAdmin, defaultTenant.Password);
                            um.AddToRoleAsync(tenantAdmin, tenantRoleName);
                            um.AddClaimAsync(tenantAdmin, new System.Security.Claims.Claim("TenantId", tenant.Id.ToString()));
                        }
                    }
                    context.SaveChanges();
                }
            }

            BuildTenant(context, defaultTenant.Name);
        }

        private void BuildTenant(AmsContext context, string tenantName)
        {
            var tenant = context.Tenants.FirstOrDefault(x => x.Name == tenantName);
            if (tenant == null)
                return;

            var ctCompany = new ClientType { Name = "Company" };
            tenant.ClientTypes.Add(ctCompany);

            var ltBld = new LocationType { Name = "Building" };
            var ltOffice = new LocationType { Name = "Office" };
            tenant.LocationTypes.Add(ltBld);
            tenant.LocationTypes.Add(ltOffice);

            var atPC = new AssetType { Name = "PC", Code = "PC", TenantId = tenant.Id };
            var atTower = new AssetType { Name = "Tower", Code = "TW", TenantId = tenant.Id };
            context.AssetTypes.Add(atPC);
            context.AssetTypes.Add(atTower);

            var ttGeneral = new TicketType { Name = "General", TenantId = tenant.Id };
            var ttPCMaintenance = new TicketType { Name = "PC Maintenance", Code = "PCM", TenantId = tenant.Id };
            context.TicketTypes.Add(ttGeneral);
            context.TicketTypes.Add(ttPCMaintenance);

            var tttMemory = new TodoTaskType { Name = "Check Memory", TenantId = tenant.Id };
            var tttOS = new TodoTaskType { Name = "Format OS", TenantId = tenant.Id };
            context.TodoTaskTypes.Add(tttMemory);
            context.TodoTaskTypes.Add(tttOS);
            context.SaveChanges();

            for (int cid = 1; cid < 10; cid++)
            {
                Client client = new Client
                {
                    Name = $"Client {cid}",
                    ClientTypeId = ctCompany.Id
                };
                tenant.Clients.Add(client);
            }
            context.SaveChanges();

            var locMainBldg = new Location { Name = "Main", LocationTypeId = ltBld.Id, TenantId = tenant.Id };
            context.Locations.Add(locMainBldg);
            context.SaveChanges();

            for (int i = 1; i < 26; i++)
            {
                var locOffice = new Location { Name = $"Office {i}", LocationTypeId = ltOffice.Id, TenantId = tenant.Id, ParentId = locMainBldg.Id };
                context.Locations.Add(locOffice);
            }

            context.SaveChanges();

            for (int i = 1; i < 101; i++)
            {
                var r = new Random();
                var client = tenant.Clients.FirstOrDefault(x => x.TenantId == tenant.Id && x.Name == $"Client {r.Next(1,9)}");
                var office = tenant.Locations.FirstOrDefault(x => x.TenantId == tenant.Id && x.LocationTypeId == ltOffice.Id && x.Name == $"Office {r.Next(1,25)}");
                var asset = new Asset { TenantId = tenant.Id, ClientId = client?.Id, LocationId = office?.Id, AssetTypeId = atPC.Id, CodeNumber = i, Code = $"{atPC.Code}-{i:5}", Name = $"PC {i}" };
                context.Assets.Add(asset);
            }
            context.SaveChanges();
        }
    }
}
