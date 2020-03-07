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

            // Tenants
            builder.Entity<Tenant>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<Tenant>().HasIndex(p => p.Name).IsUnique();

            // ClientType
            builder.Entity<ClientType>().Property(p => p.Name).IsRequired().HasMaxLength(50);

            // LocationType
            builder.Entity<LocationType>().Property(p => p.Name).IsRequired().HasMaxLength(50);

            // AssetType
            builder.Entity<AssetType>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<AssetType>().Property(p => p.Code).IsRequired().HasMaxLength(5);
            builder.Entity<AssetType>().Ignore(p => p.FieldValues);

            // TicketType
            builder.Entity<TicketType>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<TicketType>().Property(p => p.Code).IsRequired().HasMaxLength(5);
            builder.Entity<TicketType>().Ignore(p => p.FieldValues);

            // TodoTaskType
            builder.Entity<TodoTaskType>().Property(p => p.Name).IsRequired().HasMaxLength(50);

            // ItemType
            builder.Entity<ItemType>().Property(p => p.Name).IsRequired().HasMaxLength(50);

            // Client
            builder.Entity<Client>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<Client>().HasOne(p => p.ClientType)
                .WithMany(p => p.Clients)
                .HasForeignKey(p => p.ClientTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserGroup
            builder.Entity<UserGroup>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<Member>().Property(p => p.Name).HasMaxLength(50);

            // MetaField
            builder.Entity<MetaField>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<MetaField>().HasOne(p => p.CustomList)
                .WithMany(p => p.MetaFields)
                .HasForeignKey(p => p.CustomListId)
                .OnDelete(DeleteBehavior.ClientSetNull);

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


            // CustomList
            builder.Entity<CustomList>().Property(p => p.Name).IsRequired().HasMaxLength(50);
            builder.Entity<CustomListItem>().Property(p => p.Key).IsRequired().HasMaxLength(10);
            builder.Entity<CustomListItem>().Property(p => p.Value).HasMaxLength(100);
            builder.Entity<CustomListItem>().HasOne(p => p.CustomList)
                .WithMany(p => p.Items)
                .HasForeignKey(p => p.CustomListId)
                .OnDelete(DeleteBehavior.Restrict);

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

            builder.Entity<AssetItem>().Property(p => p.PartNumber).IsRequired().HasMaxLength(50);
            builder.Entity<AssetCustdian>().Property(p => p.Name).HasMaxLength(50);
            builder.Entity<AssetCustdian>().Property(p => p.RoleName).HasMaxLength(50);

            // Ticket
            builder.Entity<Ticket>().Property(p => p.Summary).IsRequired().HasMaxLength(150);
            builder.Entity<Ticket>().Property(p => p.Description).HasMaxLength(500);
            builder.Entity<Ticket>().Ignore(p => p.RelatedAssets);
            builder.Entity<Ticket>().Ignore(p => p.FieldValues);
            builder.Entity<Ticket>().HasOne(p => p.TicketType)
                .WithMany(p => p.Tickets)
                .HasForeignKey(p => p.TicketTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Ticket>().HasOne(p => p.Location)
                .WithMany(p => p.Tickets)
                .HasForeignKey(p => p.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull);

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
            builder.Entity<TodoTask>().Property(p => p.Description).HasMaxLength(500);

            // TicketJob
            builder.Entity<TicketJob>().Property(p => p.Summary).IsRequired().HasMaxLength(150);
            builder.Entity<TicketJob>().Ignore(p => p.TaskTypes);
            builder.Entity<TicketJobTaskType>().HasKey(p => new { p.TicketJobId, p.TodoTaskTypeId });
            builder.Entity<TicketJobTaskType>().HasOne(x => x.TicketJob)
                .WithMany(x => x.TicketJobTaskTypes)
                .HasForeignKey(x => x.TicketJobId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<TicketJobTaskType>().HasOne(x => x.TodoTaskType)
                .WithMany()
                .HasForeignKey(x => x.TodoTaskTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Assignment
            builder.Entity<Assignment>().Property(p => p.RoleName).HasMaxLength(100);
            builder.Entity<Assignment>().Ignore(p => p.Name);
        }

        public DbSet<AMS.Models.Assignment> Assignment { get; set; }

        public DbSet<AMS.Models.TicketAsset> TicketAsset { get; set; }
    }
}
