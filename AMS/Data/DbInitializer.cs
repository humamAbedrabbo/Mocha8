using AMS.Models;
using AMS.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(AmsContext context, IConfiguration config, IServiceProvider serviceProvider)
        {
            var initializer = new DbInitializer();
            await initializer.Seed(context, config, serviceProvider);
        }

        private async Task Seed(AmsContext context, IConfiguration config, IServiceProvider serviceProvider)
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

                await rm.CreateAsync(adminRole);
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

                    await um.CreateAsync(user, defaultAdmin.Password);
                    await um.AddToRoleAsync(user, "Admin");
                    await um.AddClaimAsync(user, new System.Security.Claims.Claim("SYSADMIN", ""));
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
                        await context.SaveChangesAsync();

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
                            await context.SaveChangesAsync();
                        }

                        // Add default tenant administrator
                        AmsUser tenantAdmin = await um.FindByNameAsync(defaultTenant.Username);
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

                            await um.CreateAsync(tenantAdmin, defaultTenant.Password);
                            await um.AddToRoleAsync(tenantAdmin, tenantRoleName);
                            await um.AddClaimAsync(tenantAdmin, new System.Security.Claims.Claim("TenantId", tenant.Id.ToString()));
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }

            switch (config["BuildCase"])
            {
                case "Syriatel":
                    BuildTenantSyriatel(context, defaultTenant.Name);
                    break;
                case "TicketDemo":
                    BuildTenantTicketDemo(context, defaultTenant.Name);
                    break;
                default:
                    break;
            }
        }

        private void BuildTenantSyriatel(AmsContext context, string tenantName)
        {
            var tenant = context.Tenants.FirstOrDefault(x => x.Name == tenantName);
            if (tenant == null)
                return;

            var codeGenerator = new CodeGenerator(context);

            var ctCompany = new ClientType { Name = "Department" };
            tenant.ClientTypes.Add(ctCompany);

            var ltBld = new LocationType { Name = "Building" };
            var ltOffice = new LocationType { Name = "Office" };
            tenant.LocationTypes.Add(ltBld);
            tenant.LocationTypes.Add(ltOffice);

            var itCase = new ItemType { Name = "PC Case", TenantId = tenant.Id }; 
            var itRAM = new ItemType { Name = "RAM", TenantId = tenant.Id }; 
            var itHD1 = new ItemType { Name = "SATA HDD", TenantId = tenant.Id }; 
            var itHD2 = new ItemType { Name = "SSD HDD", TenantId = tenant.Id }; 
            var itDvd = new ItemType { Name = "DVD Drive", TenantId = tenant.Id };
            context.ItemTypes.Add(itCase);
            context.ItemTypes.Add(itRAM);
            context.ItemTypes.Add(itHD1);
            context.ItemTypes.Add(itHD2);
            context.ItemTypes.Add(itDvd);

            /*
             *  Dev. Type / PC Name, 
                Brand, Model, Barcode, User's Name, Department, Floor, Done, 
                User's Sign

            */
            var CLDepartments = new CustomList { Name = "Departments", TenantId = tenant.Id };
            CLDepartments.Items.Add(new CustomListItem { Key = "D1", Value = "HO" });
            CLDepartments.Items.Add(new CustomListItem { Key = "D2", Value = "Management" });
            CLDepartments.Items.Add(new CustomListItem { Key = "D3", Value = "IT" });
            CLDepartments.Items.Add(new CustomListItem { Key = "D4", Value = "Operations" });
            CLDepartments.Items.Add(new CustomListItem { Key = "D5", Value = "HelpDesk" });
            CLDepartments.Items.Add(new CustomListItem { Key = "D6", Value = "Sales" });
            context.CustomLists.Add(CLDepartments);
            context.SaveChanges();

            var fPcName = new MetaField { FieldType = FieldType.Text, Name = "PC Name", TenantId = tenant.Id };
            var fBrand = new MetaField { FieldType = FieldType.Text, Name = "Brand", TenantId = tenant.Id };
            var fModel = new MetaField { FieldType = FieldType.Text, Name = "Model", TenantId = tenant.Id };
            var fBarCode = new MetaField { FieldType = FieldType.Text, Name = "Barcode", TenantId = tenant.Id };
            var fUserName = new MetaField { FieldType = FieldType.Text, Name = "User Name", TenantId = tenant.Id };
            var fDepartment = new MetaField { FieldType = FieldType.ListItem, Name = "Department", CustomListId = CLDepartments.Id, TenantId = tenant.Id };
            var fFloor = new MetaField { FieldType = FieldType.Number, Name = "Floor", TenantId = tenant.Id };
            var fDone = new MetaField { FieldType = FieldType.Boolean, Name = "Done", TenantId = tenant.Id };
            var fWebsite = new MetaField { FieldType = FieldType.Url, Name = "Website", TenantId = tenant.Id };
            context.MetaFields.Add(fPcName);            
            context.MetaFields.Add(fBrand);            
            context.MetaFields.Add(fModel);            
            context.MetaFields.Add(fBarCode);            
            context.MetaFields.Add(fUserName);            
            context.MetaFields.Add(fDepartment);            
            context.MetaFields.Add(fFloor);            
            context.MetaFields.Add(fDone);
            context.MetaFields.Add(fWebsite);
            context.SaveChanges();

            var atPC = new AssetType { Name = "PC", Code = "PC", TenantId = tenant.Id };
            atPC.Values.Add(new MetaFieldValue { FieldId = fPcName.Id });
            atPC.Values.Add(new MetaFieldValue { FieldId = fBrand.Id });
            atPC.Values.Add(new MetaFieldValue { FieldId = fModel.Id });
            atPC.Values.Add(new MetaFieldValue { FieldId = fBarCode.Id });
            atPC.Values.Add(new MetaFieldValue { FieldId = fUserName.Id });
            atPC.Values.Add(new MetaFieldValue { FieldId = fDepartment.Id });
            atPC.Values.Add(new MetaFieldValue { FieldId = fFloor.Id });
            atPC.Values.Add(new MetaFieldValue { FieldId = fWebsite.Id, UrlValue = "http://google.com" });
            var atTower = new AssetType { Name = "Tower", Code = "TW", TenantId = tenant.Id };
            context.AssetTypes.Add(atPC);
            context.AssetTypes.Add(atTower);

            var ttGeneral = new TicketType { Name = "General", TenantId = tenant.Id };
            var ttPCMaintenance = new TicketType { Name = "PC Maintenance", Code = "PCM", TenantId = tenant.Id };
            ttPCMaintenance.Values.Add(new MetaFieldValue { FieldId = fDone.Id, BooleanValue = true });
            ttPCMaintenance.Values.Add(new MetaFieldValue { FieldId = fDepartment.Id, Value = null });
            context.TicketTypes.Add(ttGeneral);
            context.TicketTypes.Add(ttPCMaintenance);


            var t1 = new TodoTaskType { Name = "Check the bios password", TenantId = tenant.Id };
            var t2 = new TodoTaskType { Name = "Make the 1st boot device", TenantId = tenant.Id };
            var t3 = new TodoTaskType { Name = "Place a sticker on the hard drive for each laptop", TenantId = tenant.Id };
            var t4 = new TodoTaskType { Name = "Reset administrator password", TenantId = tenant.Id };
            var t5 = new TodoTaskType { Name = "Make sure the guest account is disabled", TenantId = tenant.Id };
            var t6 = new TodoTaskType { Name = "Check the local users and groups on PCs", TenantId = tenant.Id };
            var t7 = new TodoTaskType { Name = "Make sure the domain admins", TenantId = tenant.Id };
            var t8 = new TodoTaskType { Name = "Check the device lock service", TenantId = tenant.Id };
            var t9 = new TodoTaskType { Name = "Check the OCS", TenantId = tenant.Id };
            var t10 = new TodoTaskType { Name = "Check the antivirus and make sure it is updated", TenantId = tenant.Id };
            var t11 = new TodoTaskType { Name = "Check the service pack for windows", TenantId = tenant.Id };
            var t12 = new TodoTaskType { Name = "Delete all users", TenantId = tenant.Id };
            var t13 = new TodoTaskType { Name = "Clean and delete all Temp Files", TenantId = tenant.Id };
            var t14 = new TodoTaskType { Name = "Delete all none business related data on C", TenantId = tenant.Id };
            var t15 = new TodoTaskType { Name = "Uninstall non-business applications", TenantId = tenant.Id };
            var t16 = new TodoTaskType { Name = "Check hardware functionality", TenantId = tenant.Id };
            var t17 = new TodoTaskType { Name = "Arrange all cables", TenantId = tenant.Id };
            context.TodoTaskTypes.Add(t1);
            context.TodoTaskTypes.Add(t2);
            context.TodoTaskTypes.Add(t3);
            context.TodoTaskTypes.Add(t4);
            context.TodoTaskTypes.Add(t5);
            context.TodoTaskTypes.Add(t6);
            context.TodoTaskTypes.Add(t7);
            context.TodoTaskTypes.Add(t8);
            context.TodoTaskTypes.Add(t9);
            context.TodoTaskTypes.Add(t10);
            context.TodoTaskTypes.Add(t11);
            context.TodoTaskTypes.Add(t12);
            context.TodoTaskTypes.Add(t13);
            context.TodoTaskTypes.Add(t14);
            context.TodoTaskTypes.Add(t15);
            context.TodoTaskTypes.Add(t16);
            context.TodoTaskTypes.Add(t17);
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
            string[] Brands = { "DELL", "TOSHIBA", "LENOVO", "ASUS" };
            for (int i = 1; i < 101; i++)
            {
                var r = new Random();
                var client = tenant.Clients.FirstOrDefault(x => x.TenantId == tenant.Id && x.Name == $"Client {r.Next(1,9)}");
                var office = tenant.Locations.FirstOrDefault(x => x.TenantId == tenant.Id && x.LocationTypeId == ltOffice.Id && x.Name == $"Office {r.Next(1,25)}");
                var asset = new Asset { TenantId = tenant.Id, ClientId = client?.Id, LocationId = office?.Id, AssetTypeId = atPC.Id, CodeNumber = i, Code = $"{atPC.Code}-{i:5}", Name = $"PC {i}" };
                asset.CodeNumber = codeGenerator.GetAssetCode(tenant.Id).Result;
                asset.Code = $"{atPC.Code}{asset.CodeNumber.ToString("D5")}";
                asset.Values.Add(new MetaFieldValue { FieldId = fPcName.Id, Value = $"Desktop-{i}" });
                asset.Values.Add(new MetaFieldValue { FieldId = fBrand.Id, Value = $"{Brands[r.Next(0,3)]}" });
                asset.Values.Add(new MetaFieldValue { FieldId = fFloor.Id, Value = $"{r.Next(1,6)}" });
                var dep = CLDepartments.Items.FirstOrDefault(x => x.Key == $"D{r.Next(1, 6)}")?.Value;
                asset.Values.Add(new MetaFieldValue { FieldId = fDepartment.Id, Value = $"{dep}" });
                asset.Values.Add(new MetaFieldValue { FieldId = fUserName.Id, Value = $"User{i}" });
                asset.Values.Add(new MetaFieldValue { FieldId = fBarCode.Id, Value = $"XXX-{i}-XXX" });
                asset.Values.Add(new MetaFieldValue { FieldId = fDone.Id, Value = $"true" });
                asset.Values.Add(new MetaFieldValue { FieldId = fWebsite.Id, Value = $"http://google.com" });
                context.Assets.Add(asset);
                context.SaveChanges();
            }
            context.SaveChanges();

            var hasher = new PasswordHasher<AmsUser>();
            var dic = new Dictionary<int, AmsUser>();

            for (int i = 1; i <= 10; i++)
            {
                var user = new AmsUser { UserName = $"user{i}", DisplayName = $"User{i}", Email = $"user{i}@ams", PhoneNumber = $"+963-11-9999-{i}", TenantId = tenant.Id, Company = "AMS", JobTitle = "AMS User", PictureUrl = $"/images/avatars/{i}.jpg" };
                user.NormalizedEmail = user.Email.ToUpper();
                user.NormalizedUserName = user.UserName.ToUpper();
                user.ConcurrencyStamp = Guid.NewGuid().ToString("D");
                user.SecurityStamp = Guid.NewGuid().ToString("D");
                user.PasswordHash = hasher.HashPassword(user, "123456");
                context.Users.Add(user);
                dic[i] = user;
                context.SaveChanges();
                context.UserClaims.Add(new IdentityUserClaim<int> { UserId = user.Id, ClaimType = "TenantId", ClaimValue = $"{user.TenantId}"  });
                context.SaveChanges();
            }
            context.SaveChanges();
            
            var grpManagement = new UserGroup { Name = "Management", TenantId = tenant.Id };
            grpManagement.Members.Add(new Member { UserId = dic[1].Id, Name = dic[1].DisplayName });
            grpManagement.Members.Add(new Member { UserId = dic[2].Id, Name = dic[2].DisplayName });
            grpManagement.Members.Add(new Member { UserId = dic[3].Id, Name = dic[3].DisplayName });
            context.UserGroups.Add(grpManagement);

            var grpIT = new UserGroup { Name = "IT", TenantId = tenant.Id };
            grpIT.Members.Add(new Member { UserId = dic[1].Id, Name = dic[1].DisplayName });
            grpIT.Members.Add(new Member { UserId = dic[4].Id, Name = dic[4].DisplayName });
            grpIT.Members.Add(new Member { UserId = dic[5].Id, Name = dic[5].DisplayName });
            context.UserGroups.Add(grpIT);

            var grpHR = new UserGroup { Name = "HR", TenantId = tenant.Id };
            grpHR.Members.Add(new Member { UserId = dic[1].Id, Name = dic[1].DisplayName });
            grpHR.Members.Add(new Member { UserId = dic[7].Id, Name = dic[7].DisplayName });
            context.UserGroups.Add(grpHR);

            var grpOperations = new UserGroup { Name = "Operations", TenantId = tenant.Id };
            grpOperations.Members.Add(new Member { UserId = dic[6].Id, Name = dic[6].DisplayName });
            grpOperations.Members.Add(new Member { UserId = dic[3].Id, Name = dic[3].DisplayName });
            context.UserGroups.Add(grpOperations);

            var grpHelpDesk = new UserGroup { Name = "HelpDesk", TenantId = tenant.Id };
            grpHelpDesk.Members.Add(new Member { UserId = dic[10].Id, Name = dic[10].DisplayName });
            context.UserGroups.Add(grpHelpDesk);

            var grpSales = new UserGroup { Name = "Sales", TenantId = tenant.Id };
            grpSales.Members.Add(new Member { UserId = dic[8].Id, Name = dic[8].DisplayName });
            grpSales.Members.Add(new Member { UserId = dic[9].Id, Name = dic[9].DisplayName });
            context.UserGroups.Add(grpSales);

            context.SaveChanges();

            var ulist = new int[] { 1, 4, 5, 10 };
            var roleNames = new[] { "Owner", "Maintainer", "Supervisor", "Support Focal" };
            Random rnd = new Random();
            foreach (var item in context.Assets)
            {
                var user = dic[ulist[rnd.Next(0, 3)]];
                item.Custodians.Add(new AssetCustdian { UserId = user.Id, Name = user.DisplayName, RoleName = roleNames[rnd.Next(0, 3)] });
            }
            context.SaveChanges();

            using (var reader = new StreamReader(@"locations.csv"))
            {
                List<string> listA = new List<string>();
                List<string> listB = new List<string>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    var asset = context.Assets.FirstOrDefault(x => x.Id.ToString() == values[0]);
                    if(asset != null)
                    {
                        asset.Lat = Convert.ToDouble(values[1]);
                        asset.Lng = Convert.ToDouble(values[2]);
                        context.SaveChanges();
                    }
                }
            }

            for (int year = 2020; year <= 2020; year++)
            {
                for (int month = 1; month <= 2; month++)
                {
                    for (int counter = 1; counter <= rnd.Next(3, 8); counter++)
                    {
                        var date = new DateTime(year, month, rnd.Next(1, 27));
                        var userGroup = new[] { 2, 5 };
                        var office = context.Locations.Where(x => x.Name == $"Office {rnd.Next(1, 26)}").FirstOrDefault();
                        var client = context.Clients.Where(x => x.Id == rnd.Next(1, 9)).FirstOrDefault();
                        List<int> assetIDs = new List<int>();
                        for (int assetCount = 1; assetCount < rnd.Next(1, 4); assetCount++)
                        {
                            var id = rnd.Next(1, 100);
                            if(!assetIDs.Contains(id))
                            {
                                assetIDs.Add(id);
                            }
                        }

                        var ticket = new Ticket { 
                            ClientId = client?.Id, LocationId = office?.Id, 
                            DueDate = date.AddDays(rnd.Next(2, 10)), EstDuration = 2, 
                            StartDate = date, Status = 
                            WorkStatus.Open,
                            Summary = LoremNET.Lorem.Words(10), 
                            Description = LoremNET.Lorem.Paragraph(8, 9, 4, 5), 
                            TicketTypeId = ttPCMaintenance.Id, TenantId  = tenant.Id };
                        ticket.CodeNumber = codeGenerator.GetTicketCode(tenant.Id).Result;
                        ticket.Code = $"{ttPCMaintenance.Code}{ticket.CodeNumber.ToString("D5")}";
                        foreach (var fld in ttPCMaintenance.Values)
                        {
                            ticket.Values.Add(new MetaFieldValue { FieldId = fld.FieldId, Value = fld.Value });
                        }
                        ticket.Assignments.Add(new Assignment { UserGroupId = 2, RoleName = "Assigned To" });
                        foreach (var ass in assetIDs)
                        {
                            ticket.TicketAssets.Add(new TicketAsset { AssetId = ass });
                        }

                        foreach (var ttt in context.TodoTaskTypes)
                        {
                            var task = new TodoTask { TenantId = tenant.Id,
                                Summary = $"{ttt.Name} - {LoremNET.Lorem.Words(6)}"
                                ,DueDate = ticket.DueDate,
                                StartDate = date,
                                TodoTaskTypeId = ttt.Id,
                                Description = LoremNET.Lorem.Paragraph(8, 9, 4, 5)
                        };
                            var ug = ticket.Assignments.Select(x => x.UserGroupId).FirstOrDefault();
                            if(ug.HasValue)
                            {
                                foreach (var mem in context.UserGroups.Find(ug).Members)
                                {
                                    task.Assignments.Add(new Assignment { UserId = mem.UserId, RoleName = "Assigned To" });
                                }
                            }
                            ticket.TodoTasks.Add(task);

                        }

                        var date2 = date.AddDays(rnd.Next(1, 3));
                        // 0 completed, 1 pending, 2 cancelled, 3 open
                        var result = rnd.Next(0, 8);
                        if (result > 3) result = 3;

                        if (result == 0)
                        {
                            ticket.Status = WorkStatus.Completed;
                            ticket.MarkCompleted = true;
                            ticket.CompletionDate = date2;
                            foreach (var t in ticket.TodoTasks)
                            {
                                t.Status = WorkStatus.Completed;
                                t.CompletionDate = date2;
                                t.MarkCompleted = true;
                            }
                        }

                        if (result == 1)
                        {
                            ticket.Status = WorkStatus.Pending;
                            ticket.MarkCompleted = false;
                            ticket.PendingDate = date2;
                            foreach (var t in ticket.TodoTasks)
                            {
                                t.Status = WorkStatus.Pending;
                                t.PendingDate = date2;
                                t.MarkCompleted = false;
                            }
                        }

                        if (result == 2)
                        {
                            ticket.Status = WorkStatus.Cancelled;
                            ticket.MarkCompleted = false;
                            ticket.CancellationDate = date2;
                            foreach (var t in ticket.TodoTasks)
                            {
                                t.Status = WorkStatus.Cancelled;
                                t.CancellationDate = date2;
                                t.MarkCompleted = false;
                            }
                        }

                        context.Tickets.Add(ticket);
                        context.SaveChanges();
                    }

                }
            }
        }

        private void BuildTenantTicketDemo(AmsContext context, string tenantName)
        {
            var tenant = context.Tenants.FirstOrDefault(x => x.Name == tenantName);
            if (tenant == null)
                return;

            var codeGenerator = new CodeGenerator(context);

            var ctCompany = new ClientType { Name = "Legal Entity" };
            tenant.ClientTypes.Add(ctCompany);

            var ltGeneral = new LocationType { Name = "Default" };
            tenant.LocationTypes.Add(ltGeneral);

            var ttGeneral = new TicketType { Name = "Incoming Post", TenantId = tenant.Id };
            context.TicketTypes.Add(ttGeneral);


            var t1 = new TodoTaskType { Name = "General", TenantId = tenant.Id };
            context.TodoTaskTypes.Add(t1);
            context.SaveChanges();

            Client client = new Client
            {
                Name = $"Central Bank",
                ClientTypeId = ctCompany.Id
            };
            tenant.Clients.Add(client);
            context.SaveChanges();

            var locHO = new Location { Name = "Head Office", LocationTypeId = ltGeneral.Id, TenantId = tenant.Id };
            context.Locations.Add(locHO);
            context.SaveChanges();

            var hasher = new PasswordHasher<AmsUser>();
            var dic = new Dictionary<int, AmsUser>();

            for (int i = 1; i <= 10; i++)
            {
                var user = new AmsUser { UserName = $"user{i}", DisplayName = $"User{i}", Email = $"user{i}@ams", PhoneNumber = $"+963-11-9999-{i}", TenantId = tenant.Id, Company = "AMS", JobTitle = "AMS User", PictureUrl = $"/images/avatars/{i}.jpg" };
                user.NormalizedEmail = user.Email.ToUpper();
                user.NormalizedUserName = user.UserName.ToUpper();
                user.ConcurrencyStamp = Guid.NewGuid().ToString("D");
                user.SecurityStamp = Guid.NewGuid().ToString("D");
                user.PasswordHash = hasher.HashPassword(user, "123456");
                context.Users.Add(user);
                dic[i] = user;
                context.SaveChanges();
                context.UserClaims.Add(new IdentityUserClaim<int> { UserId = user.Id, ClaimType = "TenantId", ClaimValue = $"{user.TenantId}" });
                context.SaveChanges();
            }
            context.SaveChanges();

            var grpGM = new UserGroup { Name = "General Manager", TenantId = tenant.Id };
            grpGM.Members.Add(new Member { UserId = dic[1].Id, Name = dic[1].DisplayName });
            context.UserGroups.Add(grpGM);

            var grpAssistant = new UserGroup { Name = "Assistant", TenantId = tenant.Id };
            grpAssistant.Members.Add(new Member { UserId = dic[2].Id, Name = dic[2].DisplayName });
            context.UserGroups.Add(grpAssistant);

            var grpM1 = new UserGroup { Name = "Manager 1", TenantId = tenant.Id };
            grpM1.Members.Add(new Member { UserId = dic[3].Id, Name = dic[3].DisplayName });
            context.UserGroups.Add(grpM1);

            var grpM2 = new UserGroup { Name = "Manager 2", TenantId = tenant.Id };
            grpM1.Members.Add(new Member { UserId = dic[4].Id, Name = dic[4].DisplayName });
            context.UserGroups.Add(grpM2);

            context.SaveChanges();

            
        }
    }
}
