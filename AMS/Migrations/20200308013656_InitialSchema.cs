using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AMS.Migrations
{
    public partial class InitialSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Code = table.Column<string>(maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetTypes_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientTypes_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomLists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomLists_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemTypes_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationTypes_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Code = table.Column<string>(maxLength: 5, nullable: false),
                    DefaultDuration = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketTypes_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TodoTaskTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    DefaultDuration = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoTaskTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoTaskTypes_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroups_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    DisplayName = table.Column<string>(maxLength: 50, nullable: false),
                    Company = table.Column<string>(maxLength: 50, nullable: true),
                    JobTitle = table.Column<string>(maxLength: 50, nullable: true),
                    PictureUrl = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    ClientTypeId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_ClientTypes_ClientTypeId",
                        column: x => x.ClientTypeId,
                        principalTable: "ClientTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clients_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomListItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomListId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(maxLength: 10, nullable: false),
                    Value = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomListItems_CustomLists_CustomListId",
                        column: x => x.CustomListId,
                        principalTable: "CustomLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MetaFields",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    FieldType = table.Column<int>(nullable: false),
                    CustomListId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetaFields_CustomLists_CustomListId",
                        column: x => x.CustomListId,
                        principalTable: "CustomLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MetaFields_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    LocationTypeId = table.Column<int>(nullable: true),
                    ParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_LocationTypes_LocationTypeId",
                        column: x => x.LocationTypeId,
                        principalTable: "LocationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Locations_Locations_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Locations_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserGroupId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_UserGroups_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    CodeNumber = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    ClientId = table.Column<int>(nullable: true),
                    AssetTypeId = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    IsOn = table.Column<bool>(nullable: false),
                    Lng = table.Column<double>(nullable: false),
                    Lat = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assets_AssetTypes_AssetTypeId",
                        column: x => x.AssetTypeId,
                        principalTable: "AssetTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assets_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assets_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assets_Assets_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assets_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketJobs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    TicketTypeId = table.Column<int>(nullable: false),
                    AssetTypeId = table.Column<int>(nullable: false),
                    ClientId = table.Column<int>(nullable: true),
                    LocationId = table.Column<int>(nullable: false),
                    OwnerId = table.Column<int>(nullable: false),
                    UserGroupId = table.Column<int>(nullable: true),
                    Summary = table.Column<string>(maxLength: 150, nullable: false),
                    JobId = table.Column<string>(nullable: true),
                    IsOn = table.Column<bool>(nullable: false),
                    CronSchedule = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketJobs_AssetTypes_AssetTypeId",
                        column: x => x.AssetTypeId,
                        principalTable: "AssetTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketJobs_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketJobs_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketJobs_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketJobs_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketJobs_TicketTypes_TicketTypeId",
                        column: x => x.TicketTypeId,
                        principalTable: "TicketTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketJobs_UserGroups_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    Summary = table.Column<string>(maxLength: 150, nullable: false),
                    CodeNumber = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    ClientId = table.Column<int>(nullable: true),
                    TicketTypeId = table.Column<int>(nullable: true),
                    LocationId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    CompletionDate = table.Column<DateTime>(nullable: true),
                    CancellationDate = table.Column<DateTime>(nullable: true),
                    PendingDate = table.Column<DateTime>(nullable: true),
                    MarkCompleted = table.Column<bool>(nullable: false),
                    EstDuration = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_TicketTypes_TicketTypeId",
                        column: x => x.TicketTypeId,
                        principalTable: "TicketTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssetCustodians",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    RoleName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetCustodians", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetCustodians_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetCustodians_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssetItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetId = table.Column<int>(nullable: false),
                    ItemTypeId = table.Column<int>(nullable: true),
                    PartNumber = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetItems_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssetItems_ItemTypes_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "ItemTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketJobTaskType",
                columns: table => new
                {
                    TicketJobId = table.Column<int>(nullable: false),
                    TodoTaskTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketJobTaskType", x => new { x.TicketJobId, x.TodoTaskTypeId });
                    table.ForeignKey(
                        name: "FK_TicketJobTaskType_TicketJobs_TicketJobId",
                        column: x => x.TicketJobId,
                        principalTable: "TicketJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketJobTaskType_TodoTaskTypes_TodoTaskTypeId",
                        column: x => x.TodoTaskTypeId,
                        principalTable: "TodoTaskTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MetaFieldValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldId = table.Column<int>(nullable: false),
                    AssetTypeId = table.Column<int>(nullable: true),
                    AssetId = table.Column<int>(nullable: true),
                    TicketTypeId = table.Column<int>(nullable: true),
                    TicketId = table.Column<int>(nullable: true),
                    Value = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaFieldValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetaFieldValues_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MetaFieldValues_AssetTypes_AssetTypeId",
                        column: x => x.AssetTypeId,
                        principalTable: "AssetTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MetaFieldValues_MetaFields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "MetaFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MetaFieldValues_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MetaFieldValues_TicketTypes_TicketTypeId",
                        column: x => x.TicketTypeId,
                        principalTable: "TicketTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketAsset",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetId = table.Column<int>(nullable: false),
                    TicketId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketAsset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketAsset_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketAsset_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TodoTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: false),
                    Summary = table.Column<string>(maxLength: 150, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    TicketId = table.Column<int>(nullable: true),
                    TodoTaskTypeId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    CompletionDate = table.Column<DateTime>(nullable: true),
                    CancellationDate = table.Column<DateTime>(nullable: true),
                    PendingDate = table.Column<DateTime>(nullable: true),
                    MarkCompleted = table.Column<bool>(nullable: false),
                    EstDuration = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoTasks_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TodoTasks_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TodoTasks_TodoTaskTypes_TodoTaskTypeId",
                        column: x => x.TodoTaskTypeId,
                        principalTable: "TodoTaskTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Assignment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserGroupId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    RoleName = table.Column<string>(maxLength: 100, nullable: true),
                    TicketId = table.Column<int>(nullable: true),
                    TodoTaskId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignment_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignment_TodoTasks_TodoTaskId",
                        column: x => x.TodoTaskId,
                        principalTable: "TodoTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignment_UserGroups_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignment_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetCustodians_AssetId",
                table: "AssetCustodians",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetCustodians_RoleName",
                table: "AssetCustodians",
                column: "RoleName");

            migrationBuilder.CreateIndex(
                name: "IX_AssetCustodians_UserId",
                table: "AssetCustodians",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetItems_AssetId",
                table: "AssetItems",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetItems_ItemTypeId",
                table: "AssetItems",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetItems_PartNumber",
                table: "AssetItems",
                column: "PartNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetTypeId",
                table: "Assets",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_ClientId",
                table: "Assets",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_Code",
                table: "Assets",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_CodeNumber",
                table: "Assets",
                column: "CodeNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_IsOn",
                table: "Assets",
                column: "IsOn");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_LocationId",
                table: "Assets",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_Name",
                table: "Assets",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_ParentId",
                table: "Assets",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_TenantId",
                table: "Assets",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetTypes_Code",
                table: "AssetTypes",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_AssetTypes_Name",
                table: "AssetTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_AssetTypes_TenantId",
                table: "AssetTypes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_RoleName",
                table: "Assignment",
                column: "RoleName");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_TicketId",
                table: "Assignment",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_TodoTaskId",
                table: "Assignment",
                column: "TodoTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_UserGroupId",
                table: "Assignment",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_UserId",
                table: "Assignment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ClientTypeId",
                table: "Clients",
                column: "ClientTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Name",
                table: "Clients",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_TenantId",
                table: "Clients",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientTypes_Name",
                table: "ClientTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ClientTypes_TenantId",
                table: "ClientTypes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomListItems_CustomListId",
                table: "CustomListItems",
                column: "CustomListId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomListItems_Key",
                table: "CustomListItems",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_CustomLists_Name",
                table: "CustomLists",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_CustomLists_TenantId",
                table: "CustomLists",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_Name",
                table: "ItemTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_TenantId",
                table: "ItemTypes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LocationTypeId",
                table: "Locations",
                column: "LocationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Name",
                table: "Locations",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ParentId",
                table: "Locations",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_TenantId",
                table: "Locations",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationTypes_Name",
                table: "LocationTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_LocationTypes_TenantId",
                table: "LocationTypes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_UserGroupId",
                table: "Members",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_UserId",
                table: "Members",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MetaFields_CustomListId",
                table: "MetaFields",
                column: "CustomListId");

            migrationBuilder.CreateIndex(
                name: "IX_MetaFields_Name",
                table: "MetaFields",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_MetaFields_TenantId",
                table: "MetaFields",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MetaFieldValues_AssetId",
                table: "MetaFieldValues",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_MetaFieldValues_AssetTypeId",
                table: "MetaFieldValues",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MetaFieldValues_FieldId",
                table: "MetaFieldValues",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_MetaFieldValues_TicketId",
                table: "MetaFieldValues",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_MetaFieldValues_TicketTypeId",
                table: "MetaFieldValues",
                column: "TicketTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MetaFieldValues_Value",
                table: "MetaFieldValues",
                column: "Value");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_TenantId",
                table: "Roles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Name",
                table: "Tenants",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketAsset_AssetId",
                table: "TicketAsset",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketAsset_TicketId",
                table: "TicketAsset",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketJobs_AssetTypeId",
                table: "TicketJobs",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketJobs_ClientId",
                table: "TicketJobs",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketJobs_LocationId",
                table: "TicketJobs",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketJobs_OwnerId",
                table: "TicketJobs",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketJobs_TenantId",
                table: "TicketJobs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketJobs_TicketTypeId",
                table: "TicketJobs",
                column: "TicketTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketJobs_UserGroupId",
                table: "TicketJobs",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketJobTaskType_TodoTaskTypeId",
                table: "TicketJobTaskType",
                column: "TodoTaskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ClientId",
                table: "Tickets",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Code",
                table: "Tickets",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CodeNumber",
                table: "Tickets",
                column: "CodeNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CompletionDate",
                table: "Tickets",
                column: "CompletionDate");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_DueDate",
                table: "Tickets",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_LocationId",
                table: "Tickets",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_StartDate",
                table: "Tickets",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Status",
                table: "Tickets",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Summary",
                table: "Tickets",
                column: "Summary");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TenantId",
                table: "Tickets",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketTypeId",
                table: "Tickets",
                column: "TicketTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketTypes_Code",
                table: "TicketTypes",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_TicketTypes_Name",
                table: "TicketTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_TicketTypes_TenantId",
                table: "TicketTypes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoTasks_DueDate",
                table: "TodoTasks",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_TodoTasks_StartDate",
                table: "TodoTasks",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_TodoTasks_Status",
                table: "TodoTasks",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TodoTasks_Summary",
                table: "TodoTasks",
                column: "Summary");

            migrationBuilder.CreateIndex(
                name: "IX_TodoTasks_TenantId",
                table: "TodoTasks",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoTasks_TicketId",
                table: "TodoTasks",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoTasks_TodoTaskTypeId",
                table: "TodoTasks",
                column: "TodoTaskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoTaskTypes_Name",
                table: "TodoTaskTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_TodoTaskTypes_TenantId",
                table: "TodoTaskTypes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_Name",
                table: "UserGroups",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_TenantId",
                table: "UserGroups",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TenantId",
                table: "Users",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetCustodians");

            migrationBuilder.DropTable(
                name: "AssetItems");

            migrationBuilder.DropTable(
                name: "Assignment");

            migrationBuilder.DropTable(
                name: "CustomListItems");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "MetaFieldValues");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "TicketAsset");

            migrationBuilder.DropTable(
                name: "TicketJobTaskType");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "ItemTypes");

            migrationBuilder.DropTable(
                name: "TodoTasks");

            migrationBuilder.DropTable(
                name: "MetaFields");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "TicketJobs");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "TodoTaskTypes");

            migrationBuilder.DropTable(
                name: "CustomLists");

            migrationBuilder.DropTable(
                name: "AssetTypes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "TicketTypes");

            migrationBuilder.DropTable(
                name: "ClientTypes");

            migrationBuilder.DropTable(
                name: "LocationTypes");

            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
