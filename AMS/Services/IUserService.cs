using AMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMS.Services
{
    public interface IUserService
    {
        Task<IEnumerable<Asset>> GetAssetsAsync();
        Task<SelectList> GetAssetsSelectAsync(int? id = null);
        Task<IEnumerable<AssetType>> GetAssetTypesAsync();
        Task<SelectList> GetAssetTypesSelectAsync(int? id = null);
        Task<IEnumerable<Client>> GetClientsAsync();
        Task<SelectList> GetClientsSelectAsync(int? id = null);
        Task<IEnumerable<ClientType>> GetClientTypesAsync();
        Task<SelectList> GetClientTypesSelectAsync(int? id = null);
        Task<AmsUser> GetCurrentUserAsync();
        Task<IEnumerable<CustomList>> GetCustomListsAsync();
        Task<SelectList> GetCustomListsSelectAsync(int? id = null);
        Task<IEnumerable<ItemType>> GetItemTypesAsync();
        Task<SelectList> GetItemTypesSelectAsync(int? id = null);
        Task<IEnumerable<LocationType>> GetLocationTypesAsync();
        Task<SelectList> GetLocationTypesSelectAsync(int? id = null);
        Task<IEnumerable<Member>> GetMembersAsync();
        Task<SelectList> GetMembersSelectAsync(int? id = null);
        Task<IEnumerable<MetaField>> GetMetaFieldsAsync();
        Task<SelectList> GetMetaFieldsSelectAsync(int? id = null);
        Task<IEnumerable<Ticket>> GetTicketsAsync();
        Task<SelectList> GetTicketsSelectAsync(int? id = null);
        Task<IEnumerable<TicketType>> GetTicketTypesAsync();
        Task<SelectList> GetTicketTypesSelectAsync(int? id = null);
        Task<IEnumerable<TodoTask>> GetTodoTasksAsync();
        Task<SelectList> GetTodoTasksSelectAsync(int? id = null);
        Task<IEnumerable<TodoTaskType>> GetTodoTaskTypesAsync();
        Task<SelectList> GetTodoTaskTypesSelectAsync(int? id = null);
        Task<IEnumerable<UserGroup>> GetUserGroupsAsync();
        Task<SelectList> GetUserGroupsSelectAsync(int? id = null);
        Task<IEnumerable<AmsUser>> GetUsersAsync();
        Task<SelectList> GetUsersSelectAsync(int? id = null);
        int? GetUserTenantIdAsync();
    }
}