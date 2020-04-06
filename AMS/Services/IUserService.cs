using AMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMS.Services
{
    public interface IUserService
    {
        Task<IEnumerable<Asset>> GetAssetsAsync(int? assetTypeId = null, int? clientId = null, int? locationId = null);
        Task<SelectList> GetAssetsSelectAsync(int? id = null);
        Task<IEnumerable<AssetType>> GetAssetTypesAsync();
        Task<SelectList> GetAssetTypesSelectAsync(int? id = null);
        Task<IEnumerable<Client>> GetClientsAsync();
        Task<SelectList> GetClientsSelectAsync(int? id = null);
        Task<IEnumerable<ClientType>> GetClientTypesAsync();
        Task<SelectList> GetClientTypesSelectAsync(int? id = null);
        Task<AmsUser> GetCurrentUserAsync();
        Task<IEnumerable<CustomListItem>> GetCustomListItemsAsync(int listId);
        Task<SelectList> GetCustomListItemsSelectAsync(int listId, string key = null);
        Task<IEnumerable<CustomList>> GetCustomListsAsync();
        Task<SelectList> GetCustomListsSelectAsync(int? id = null);
        Task<IEnumerable<ItemType>> GetItemTypesAsync();
        Task<SelectList> GetItemTypesSelectAsync(int? id = null);
        Task<IEnumerable<Location>> GetLocationsAsync();
        Task<SelectList> GetLocationsSelectAsync(int? id = null);
        Task<IEnumerable<LocationType>> GetLocationTypesAsync();
        Task<SelectList> GetLocationTypesSelectAsync(int? id = null);
        Task<IEnumerable<Member>> GetMembersAsync(int? userGroupId = null);
        Task<SelectList> GetMembersSelectAsync(int? id = null);
        Task<IEnumerable<MetaField>> GetMetaFieldsAsync();
        Task<SelectList> GetMetaFieldsSelectAsync(int? id = null);
        Task<int> GetTicketDefaultDuration(int ticketTypeId);
        Task<IEnumerable<TicketJob>> GetTicketJobsAsync();
        Task<IEnumerable<Ticket>> GetTicketsAsync(int? ticketTypeId = null, int? clientId = null, int? locationId = null, int? userGroupId = null, int? userId = null, bool isActive = false);
        Task<SelectList> GetTicketsSelectAsync(int? id = null);
        Task<IEnumerable<TicketType>> GetTicketTypesAsync();
        Task<SelectList> GetTicketTypesSelectAsync(int? id = null);
        Task<int> GetTodoTaskDefaultDuration(int todoTaskTypeId);
        Task<IEnumerable<TodoTask>> GetTodoTasksAsync(int? todoTaskTypeId = null, int? ticketId = null, int? userGroupId = null, int? userId = null, bool isActive = false);
        Task<SelectList> GetTodoTasksSelectAsync(int? id = null);
        Task<IEnumerable<TodoTaskType>> GetTodoTaskTypesAsync();
        Task<MultiSelectList> GetTodoTaskTypesMultiSelectAsync(IEnumerable<int> ids = null);
        Task<SelectList> GetTodoTaskTypesSelectAsync(int? id = null);
        Task<int> GetUserAssetsCount(int userId);
        Task<IEnumerable<UserGroup>> GetUserGroupsAsync();
        Task<SelectList> GetUserGroupsSelectAsync(int? id = null);
        Task<IEnumerable<AmsUser>> GetUsersAsync();
        Task<SelectList> GetUsersSelectAsync(int? id = null);
        Task<int> GetUserTasksCount(int userId);
        int? GetUserTenantId();
        Task<int> GetUserUserGroupsCount(int userId);
        bool IsSysAdmin();
        Task<bool> SetTaskState(int taskId, WorkStatus status);
        Task SetTicketState(int ticketId, WorkStatus status);
    }
}