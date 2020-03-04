using AMS.Data;
using AMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Services
{
    [Authorize]
    public class UserService : IUserService
    {
        private readonly AmsContext context;
        private readonly UserManager<AmsUser> userManager;
        private readonly IHttpContextAccessor haccessor;
        private readonly AmsUser CurrentUser;
        public UserService(AmsContext context, UserManager<AmsUser> userManager, IHttpContextAccessor haccessor)
        {
            this.context = context;
            this.userManager = userManager;
            this.haccessor = haccessor;
            CurrentUser = GetCurrentUserAsync().Result;
        }

        public async Task<AmsUser> GetCurrentUserAsync()
        {
            var name = haccessor.HttpContext.User.Identity.Name;
            return await userManager.FindByNameAsync(name);
        }

        public int? GetUserTenantId()
            => CurrentUser?.TenantId;

        public async Task<IEnumerable<AmsUser>> GetUsersAsync()
            => await context.Users
            .Where(x => x.TenantId == GetUserTenantId())
            .OrderBy(x => x.DisplayName)
            .ToListAsync();

        public async Task<SelectList> GetUsersSelectAsync(int? id = null)
            => new SelectList(await GetUsersAsync(), "Id", "Title", id);

        public async Task<IEnumerable<TodoTask>> GetTodoTasksAsync()
            => await context.TodoTasks
            .Include(x => x.Ticket)
            .Include(x => x.TodoTaskType)
            .Where(x => x.TenantId == GetUserTenantId())
            .OrderBy(x => x.Summary)
            .ToListAsync();

        public async Task<SelectList> GetTodoTasksSelectAsync(int? id = null)
            => new SelectList(await GetTodoTasksAsync(), "Id", "Title", id, "GroupTitle");

        public async Task<IEnumerable<Ticket>> GetTicketsAsync()
            => await context.Tickets
            .Include(x => x.TicketType)
            .Include(x => x.Location)
            .Where(x => x.TenantId == GetUserTenantId())
            .OrderBy(x => x.Summary)
            .ToListAsync();

        public async Task<SelectList> GetTicketsSelectAsync(int? id = null)
            => new SelectList(await GetTicketsAsync(), "Id", "Title", id, "GroupTitle");

        public async Task<IEnumerable<Asset>> GetAssetsAsync()
            => await context.Assets
            .Include(x => x.AssetType)
            .Include(x => x.Client)
            .Include(x => x.Location)
            .Include(x => x.Parent)
            .Where(x => x.TenantId == GetUserTenantId())
            .OrderBy(x => x.Name)
            .ToListAsync();

        public async Task<SelectList> GetAssetsSelectAsync(int? id = null)
            => new SelectList(await GetAssetsAsync(), "Id", "Title", id, "GroupTitle");

        public async Task<IEnumerable<MetaField>> GetMetaFieldsAsync()
            => await context.MetaFields
            .Where(x => x.TenantId == GetUserTenantId())
            .OrderBy(x => x.Name)
            .ToListAsync();

        public async Task<SelectList> GetMetaFieldsSelectAsync(int? id = null)
            => new SelectList(await GetMetaFieldsAsync(), "Id", "Name", id);

        public async Task<IEnumerable<CustomList>> GetCustomListsAsync()
            => await context.CustomLists
            .Where(x => x.TenantId == GetUserTenantId())
            .OrderBy(x => x.Name)
            .ToListAsync();

        public async Task<SelectList> GetCustomListsSelectAsync(int? id = null)
            => new SelectList(await GetCustomListsAsync(), "Id", "Name", id);

        public async Task<IEnumerable<CustomListItem>> GetCustomListItemsAsync(int listId)
            => await context.CustomListItems
            .Where(x => x.CustomListId == listId)
            .OrderBy(x => x.Key)
            .ToListAsync();

        public async Task<SelectList> GetCustomListItemsSelectAsync(int listId, string key = null)
            => new SelectList(await GetCustomListItemsAsync(listId), "Key", "Value", key);

        public async Task<IEnumerable<Location>> GetLocationsAsync()
            => await context.Locations
            .Include(x => x.LocationType)
            .Where(x => x.TenantId == GetUserTenantId())
            .OrderBy(x => x.Name)
            .ToListAsync();

        public async Task<SelectList> GetLocationsSelectAsync(int? id = null)
            => new SelectList(await GetLocationsAsync(), "Id", "Name", id, "GroupTitle");

        public async Task<IEnumerable<Client>> GetClientsAsync()
            => await context.Clients
            .Include(x => x.ClientType)
            .Where(x => x.TenantId == GetUserTenantId())
            .OrderBy(x => x.Name)
            .ToListAsync();

        public async Task<SelectList> GetClientsSelectAsync(int? id = null)
            => new SelectList(await GetClientsAsync(), "Id", "Name", id, "GroupTitle");

        public async Task<IEnumerable<Member>> GetMembersAsync()
            => await context.Members
            .Include(x => x.UserGroup)
            .Include(x => x.User)
            .Where(x => x.UserGroup.TenantId == GetUserTenantId())
            .OrderBy(x => x.Name)
            .ToListAsync();

        public async Task<SelectList> GetMembersSelectAsync(int? id = null)
            => new SelectList(await GetMembersAsync(), "Id", "Title", id, "GroupTitle");

        public async Task<IEnumerable<UserGroup>> GetUserGroupsAsync()
            => await context.UserGroups
            .Where(x => x.TenantId == GetUserTenantId())
            .OrderBy(x => x.Name)
            .ToListAsync();

        public async Task<SelectList> GetUserGroupsSelectAsync(int? id = null)
            => new SelectList(await GetUserGroupsAsync(), "Id", "Name", id);

        public async Task<IEnumerable<ClientType>> GetClientTypesAsync()
            => await context.ClientTypes
            .Where(x => x.TenantId == GetUserTenantId())
            .OrderBy(x => x.Name)
            .ToListAsync();

        public async Task<SelectList> GetClientTypesSelectAsync(int? id = null)
            => new SelectList(await GetClientTypesAsync(), "Id", "Name", id);

        public async Task<IEnumerable<LocationType>> GetLocationTypesAsync()
            => await context.LocationTypes
            .Where(x => x.TenantId == GetUserTenantId())
            .OrderBy(x => x.Name)
            .ToListAsync();

        public async Task<SelectList> GetLocationTypesSelectAsync(int? id = null)
            => new SelectList(await GetLocationTypesAsync(), "Id", "Name", id);

        public async Task<IEnumerable<ItemType>> GetItemTypesAsync()
            => await context.ItemTypes
            .Where(x => x.TenantId == GetUserTenantId())
            .OrderBy(x => x.Name)
            .ToListAsync();

        public async Task<SelectList> GetItemTypesSelectAsync(int? id = null)
            => new SelectList(await GetItemTypesAsync(), "Id", "Name", id);

        public async Task<IEnumerable<AssetType>> GetAssetTypesAsync()
            => await context.AssetTypes
            .Where(x => x.TenantId == GetUserTenantId())
            .OrderBy(x => x.Name)
            .ToListAsync();

        public async Task<SelectList> GetAssetTypesSelectAsync(int? id = null)
            => new SelectList(await GetAssetTypesAsync(), "Id", "Name", id);

        public async Task<IEnumerable<TicketType>> GetTicketTypesAsync()
            => await context.TicketTypes
            .Where(x => x.TenantId == GetUserTenantId())
            .OrderBy(x => x.Name)
            .ToListAsync();

        public async Task<int> GetTicketDefaultDuration(int ticketTypeId)
            => await context.TicketTypes
            .Where(x => x.Id == ticketTypeId && x.TenantId == GetUserTenantId())
            .Select(x => x.DefaultDuration)
            .FirstOrDefaultAsync();

        public async Task<SelectList> GetTicketTypesSelectAsync(int? id = null)
            => new SelectList(await GetTicketTypesAsync(), "Id", "Name", id);

        public async Task<IEnumerable<TodoTaskType>> GetTodoTaskTypesAsync()
            => await context.TodoTaskTypes
            .Where(x => x.TenantId == GetUserTenantId())
            .OrderBy(x => x.Name)
            .ToListAsync();

        public async Task<int> GetTodoTaskDefaultDuration(int todoTaskTypeId)
            => await context.TodoTaskTypes
            .Where(x => x.Id == todoTaskTypeId && x.TenantId == GetUserTenantId())
            .Select(x => x.DefaultDuration)
            .FirstOrDefaultAsync();

        public async Task<SelectList> GetTodoTaskTypesSelectAsync(int? id = null)
            => new SelectList(await GetTodoTaskTypesAsync(), "Id", "Name", id);
    }
}
