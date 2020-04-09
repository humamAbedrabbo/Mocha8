using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AMS.Models;
using Microsoft.AspNetCore.Identity;

namespace AMS.Data
{
    public class AmsContext : IdentityDbContext<AmsUser, AmsRole, int>
    {
        public AmsContext(DbContextOptions<AmsContext> options)
            : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<ClientType> ClientTypes { get; set; }
        public DbSet<AssetType> AssetTypes { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<LocationType> LocationTypes { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<TodoTaskType> TodoTaskTypes { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<MetaField> MetaFields { get; set; }
        public DbSet<CustomList> CustomLists { get; set; }
        public DbSet<CustomListItem> CustomListItems { get; set; }
        public DbSet<MetaFieldValue> MetaFieldValues { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TodoTask> TodoTasks { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<AssetCustdian> AssetCustodians { get; set; }
        public DbSet<AssetItem> AssetItems { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<TicketJob> TicketJobs { get; set; }
        public DbSet<Attachment> Attachements { get; set; }
        public DbSet<AMS.Models.Assignment> Assignment { get; set; }

        public DbSet<AMS.Models.TicketAsset> TicketAsset { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Identity
            builder.Entity<AmsUser>().ToTable("Users");
            builder.Entity<AmsRole>().ToTable("Roles");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");

            builder.Entity<AmsUser>().HasIndex(p => p.TenantId);

            // Tenants
            builder.Entity<Tenant>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<Tenant>().HasIndex(p => p.Name).IsUnique();

            // ClientType
            builder.Entity<ClientType>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<ClientType>().HasIndex(p => p.TenantId);
            builder.Entity<ClientType>().HasIndex(p => p.Name);

            // LocationType
            builder.Entity<LocationType>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<LocationType>().HasIndex(p => p.TenantId);
            builder.Entity<LocationType>().HasIndex(p => p.Name);

            // AssetType
            builder.Entity<AssetType>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<AssetType>().Property(p => p.Code).IsRequired().HasMaxLength(5);
            builder.Entity<AssetType>().Ignore(p => p.FieldValues);
            builder.Entity<AssetType>().HasIndex(p => p.TenantId);
            builder.Entity<AssetType>().HasIndex(p => p.Name);
            builder.Entity<AssetType>().HasIndex(p => p.Code);

            // TicketType
            builder.Entity<TicketType>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<TicketType>().Property(p => p.Code).IsRequired().HasMaxLength(5);
            builder.Entity<TicketType>().Ignore(p => p.FieldValues);
            builder.Entity<TicketType>().HasIndex(p => p.TenantId);
            builder.Entity<TicketType>().HasIndex(p => p.Name);
            builder.Entity<TicketType>().HasIndex(p => p.Code);

            // TodoTaskType
            builder.Entity<TodoTaskType>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<TodoTaskType>().HasIndex(p => p.TenantId);
            builder.Entity<TodoTaskType>().HasIndex(p => p.Name);

            // ItemType
            builder.Entity<ItemType>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<ItemType>().HasIndex(p => p.TenantId);
            builder.Entity<ItemType>().HasIndex(p => p.Name);

            // Client
            builder.Entity<Client>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<Client>().HasOne(p => p.ClientType)
                .WithMany(p => p.Clients)
                .HasForeignKey(p => p.ClientTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Client>().HasIndex(p => p.TenantId);
            builder.Entity<Client>().HasIndex(p => p.ClientTypeId);
            builder.Entity<Client>().HasIndex(p => p.Name);

            // UserGroup
            builder.Entity<UserGroup>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<Member>().Property(p => p.Name).HasMaxLength(50);
            builder.Entity<UserGroup>().HasIndex(p => p.TenantId);
            builder.Entity<UserGroup>().HasIndex(p => p.Name);
            builder.Entity<Member>().HasIndex(p => p.UserId);
            builder.Entity<Member>().HasIndex(p => p.UserGroupId);

            // MetaField
            builder.Entity<MetaField>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<MetaField>().HasOne(p => p.CustomList)
                .WithMany(p => p.MetaFields)
                .HasForeignKey(p => p.CustomListId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.Entity<MetaField>().HasIndex(p => p.TenantId);
            builder.Entity<MetaField>().HasIndex(p => p.Name);

            // MetaFieldValue
            builder.Entity<MetaFieldValue>().Property(p => p.Value).HasMaxLength(250);
            builder.Entity<MetaFieldValue>().Ignore(p => p.UrlValue);
            builder.Entity<MetaFieldValue>().Ignore(p => p.NumberValue);
            builder.Entity<MetaFieldValue>().Ignore(p => p.DecimalValue);
            builder.Entity<MetaFieldValue>().Ignore(p => p.DateValue);
            builder.Entity<MetaFieldValue>().Ignore(p => p.DateTimeValue);
            builder.Entity<MetaFieldValue>().Ignore(p => p.BooleanValue);
            
            builder.Entity<MetaFieldValue>().HasOne(p => p.Field)
                .WithMany(p => p.Values)
                .HasForeignKey(p => p.FieldId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<MetaFieldValue>().HasOne(p => p.AssetType)
                .WithMany(p => p.Values)
                .HasForeignKey(p => p.AssetTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<MetaFieldValue>().HasOne(p => p.Asset)
                .WithMany(p => p.Values)
                .HasForeignKey(p => p.AssetId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<MetaFieldValue>().HasOne(p => p.TicketType)
                .WithMany(p => p.Values)
                .HasForeignKey(p => p.TicketTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<MetaFieldValue>().HasOne(p => p.Ticket)
                .WithMany(p => p.Values)
                .HasForeignKey(p => p.TicketId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<MetaFieldValue>().HasIndex(p => p.AssetId);
            builder.Entity<MetaFieldValue>().HasIndex(p => p.AssetTypeId);
            builder.Entity<MetaFieldValue>().HasIndex(p => p.TicketId);
            builder.Entity<MetaFieldValue>().HasIndex(p => p.TicketTypeId);
            builder.Entity<MetaFieldValue>().HasIndex(p => p.FieldId);
            builder.Entity<MetaFieldValue>().HasIndex(p => p.Value);


            // CustomList
            builder.Entity<CustomList>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<CustomListItem>().Property(p => p.Key).IsRequired().HasMaxLength(10);
            builder.Entity<CustomListItem>().Property(p => p.Value).HasMaxLength(100);
            builder.Entity<CustomListItem>().HasOne(p => p.CustomList)
                .WithMany(p => p.Items)
                .HasForeignKey(p => p.CustomListId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CustomList>().HasIndex(p => p.TenantId);
            builder.Entity<CustomList>().HasIndex(p => p.Name);
            builder.Entity<CustomListItem>().HasIndex(p => p.Key);
            builder.Entity<CustomListItem>().HasIndex(p => p.CustomListId);

            // Location
            builder.Entity<Location>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<Location>().HasOne(p => p.LocationType)
                .WithMany(p => p.Locations)
                .HasForeignKey(p => p.LocationTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Location>().HasOne(p => p.Parent)
                .WithMany(p => p.Childs)
                .HasForeignKey(p => p.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Location>().HasIndex(p => p.TenantId);
            builder.Entity<Location>().HasIndex(p => p.LocationTypeId);
            builder.Entity<Location>().HasIndex(p => p.ParentId);
            builder.Entity<Location>().HasIndex(p => p.Name);

            // Asset
            builder.Entity<Asset>().Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Entity<Asset>().Ignore(p => p.ActiveTickets);
            builder.Entity<Asset>().Ignore(p => p.FieldValues);
            builder.Entity<Asset>().HasOne(p => p.AssetType)
                .WithMany(p => p.Assets)
                .HasForeignKey(p => p.AssetTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Asset>().HasOne(p => p.Location)
                .WithMany(p => p.Assets)
                .HasForeignKey(p => p.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.Entity<Asset>().HasOne(p => p.Parent)
                .WithMany(p => p.Childs)
                .HasForeignKey(p => p.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Asset>().HasIndex(p => p.TenantId);
            builder.Entity<Asset>().HasIndex(p => p.AssetTypeId);
            builder.Entity<Asset>().HasIndex(p => p.ClientId);
            builder.Entity<Asset>().HasIndex(p => p.LocationId);
            builder.Entity<Asset>().HasIndex(p => p.Name);
            builder.Entity<Asset>().HasIndex(p => p.IsOn);
            builder.Entity<Asset>().HasIndex(p => p.Code);
            builder.Entity<Asset>().HasIndex(p => p.CodeNumber);

            builder.Entity<AssetItem>().Property(p => p.PartNumber).IsRequired().HasMaxLength(50);
            builder.Entity<AssetItem>().HasIndex(p => p.AssetId);
            builder.Entity<AssetItem>().HasIndex(p => p.ItemTypeId);
            builder.Entity<AssetItem>().HasIndex(p => p.PartNumber);

            builder.Entity<AssetCustdian>().Property(p => p.Name).HasMaxLength(50);
            builder.Entity<AssetCustdian>().Property(p => p.RoleName).HasMaxLength(50);
            builder.Entity<AssetCustdian>().HasIndex(p => p.AssetId);
            builder.Entity<AssetCustdian>().HasIndex(p => p.UserId);
            builder.Entity<AssetCustdian>().HasIndex(p => p.RoleName);

            // Ticket
            builder.Entity<Ticket>().Property(p => p.Summary).IsRequired().HasMaxLength(150);
            builder.Entity<Ticket>().Property(p => p.Description);
            builder.Entity<Ticket>().Ignore(p => p.RelatedAssets);
            builder.Entity<Ticket>().Ignore(p => p.FieldValues);
            builder.Entity<Ticket>().Ignore(p => p.TotalActiveTasks);
            builder.Entity<Ticket>().Ignore(p => p.TotalCancelledTasks);
            builder.Entity<Ticket>().Ignore(p => p.TotalCompletedTasks);
            builder.Entity<Ticket>().Ignore(p => p.TotalOpenTasks);
            builder.Entity<Ticket>().Ignore(p => p.TotalOverdueTasks);
            builder.Entity<Ticket>().Ignore(p => p.TotalPendingTasks);
            builder.Entity<Ticket>().HasOne(p => p.TicketType)
                .WithMany(p => p.Tickets)
                .HasForeignKey(p => p.TicketTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Ticket>().HasOne(p => p.Location)
                .WithMany(p => p.Tickets)
                .HasForeignKey(p => p.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Ticket>().HasIndex(p => p.TenantId);
            builder.Entity<Ticket>().HasIndex(p => p.TicketTypeId);
            builder.Entity<Ticket>().HasIndex(p => p.Summary);
            builder.Entity<Ticket>().HasIndex(p => p.ClientId);
            builder.Entity<Ticket>().HasIndex(p => p.LocationId);
            builder.Entity<Ticket>().HasIndex(p => p.StartDate);
            builder.Entity<Ticket>().HasIndex(p => p.DueDate);
            builder.Entity<Ticket>().HasIndex(p => p.Status);
            builder.Entity<Ticket>().HasIndex(p => p.CompletionDate);
            builder.Entity<Ticket>().HasIndex(p => p.Code);
            builder.Entity<Ticket>().HasIndex(p => p.CodeNumber);

            builder.Entity<TicketAsset>().HasOne(p => p.Ticket)
                .WithMany(p => p.TicketAssets)
                .HasForeignKey(p => p.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TicketAsset>().HasOne(p => p.Asset)
                .WithMany(p => p.TicketAssets)
                .HasForeignKey(p => p.AssetId)
                .OnDelete(DeleteBehavior.Restrict);

            // TodoTask
            builder.Entity<TodoTask>().Property(p => p.Summary).IsRequired().HasMaxLength(150);
            builder.Entity<TodoTask>().Property(p => p.Description);
            builder.Entity<TodoTask>().HasIndex(p => p.TenantId);
            builder.Entity<TodoTask>().HasIndex(p => p.TicketId);
            builder.Entity<TodoTask>().HasIndex(p => p.TodoTaskTypeId);
            builder.Entity<TodoTask>().HasIndex(p => p.Summary);
            builder.Entity<TodoTask>().HasIndex(p => p.Status);
            builder.Entity<TodoTask>().HasIndex(p => p.StartDate);
            builder.Entity<TodoTask>().HasIndex(p => p.DueDate);

            // TicketJob
            builder.Entity<TicketJob>().Property(p => p.Summary).IsRequired().HasMaxLength(150);
            builder.Entity<TicketJob>().Ignore(p => p.TaskTypes);
            
            builder.Entity<TicketJob>().HasOne(x => x.TicketType)
                .WithMany()
                .HasForeignKey(x => x.TicketTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<TicketJob>().HasOne(x => x.Client)
                .WithMany()
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<TicketJob>().HasOne(x => x.Location)
                .WithMany()
                .HasForeignKey(x => x.LocationId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<TicketJob>().HasOne(x => x.UserGroup)
                .WithMany()
                .HasForeignKey(x => x.UserGroupId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<TicketJob>().HasOne(x => x.Tenant)
                .WithMany()
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<TicketJob>().HasOne(x => x.Owner)
                .WithMany()
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<TicketJob>().HasOne(x => x.AssetType)
                .WithMany()
                .HasForeignKey(x => x.AssetTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TicketJobTaskType>().HasKey(p => new { p.TicketJobId, p.TodoTaskTypeId });
            builder.Entity<TicketJobTaskType>().HasOne(x => x.TicketJob)
                .WithMany(x => x.TicketJobTaskTypes)
                .HasForeignKey(x => x.TicketJobId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<TicketJobTaskType>().HasOne(x => x.TodoTaskType)
                .WithMany()
                .HasForeignKey(x => x.TodoTaskTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<TicketJob>().HasIndex(p => p.TenantId);
            builder.Entity<TicketJob>().HasIndex(p => p.TicketTypeId);
            builder.Entity<TicketJob>().HasIndex(p => p.UserGroupId);

            // Assignment
            builder.Entity<Assignment>().Property(p => p.RoleName).HasMaxLength(100);
            builder.Entity<Assignment>().Ignore(p => p.Name);
            builder.Entity<Assignment>().HasIndex(p => p.TicketId);
            builder.Entity<Assignment>().HasIndex(p => p.TodoTaskId);
            builder.Entity<Assignment>().HasIndex(p => p.UserId);
            builder.Entity<Assignment>().HasIndex(p => p.UserGroupId);
            builder.Entity<Assignment>().HasIndex(p => p.RoleName);

            // Attachements
            builder.Entity<Attachment>().Property(p => p.Title).HasMaxLength(100).IsRequired();
            builder.Entity<Attachment>().Property(p => p.FileName).HasMaxLength(200).IsRequired();
            builder.Entity<Attachment>().Property(p => p.ContentType).HasMaxLength(100);
            builder.Entity<Attachment>().HasOne(p => p.Ticket)
                .WithMany(p => p.Attachements)
                .HasForeignKey(p => p.TicketId)
                .OnDelete(DeleteBehavior.Restrict);

            // Notifications
            builder.Entity<Notification>(e =>
            {
                e.Property(p => p.Message).HasMaxLength(100).IsRequired();
                e.Ignore(p => p.Url);
                e.HasOne(p => p.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
        }


    }
}
