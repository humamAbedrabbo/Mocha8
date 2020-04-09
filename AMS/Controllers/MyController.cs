using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Data;
using AMS.Models;
using AMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AMS.Controllers
{
    [Authorize]
    public class MyController : Controller
    {
        private readonly ILogger<MyController> logger;
        private readonly AmsContext context;
        private readonly IUserService userService;

        public MyController(ILogger<MyController> logger, AmsContext context, IUserService userService)
        {
            this.logger = logger;
            this.context = context;
            this.userService = userService;
        }

        public async Task<IActionResult> UserGroups()
        {
            var me = await userService.GetCurrentUserAsync();

            var userGroups = await context.Members
                .Include(x => x.UserGroup)
                .Where(x => (x.UserGroup.TenantId == userService.GetUserTenantId())
                    && (x.UserId == me.Id)
                )
                .Select(x => x.UserGroup)
                .Distinct()
                .ToListAsync();

            return View(userGroups);
        }

        public async Task<IActionResult> Tickets(string code = null, StatusFilter? tstatus = null)
        {
            
            var me = await userService.GetCurrentUserAsync();

            var userGroupIDs = await context.Members
                .Include(x => x.UserGroup)
                .Where(x => (x.UserGroup.TenantId == userService.GetUserTenantId())
                    && (x.UserId == me.Id)
                )
                .Select(x => x.UserGroupId)
                .Distinct()
                .ToListAsync();
            var tickets = await context.Assignment
                .Include(x => x.Ticket).ThenInclude(x => x.TicketType)
                .Include(x => x.Ticket).ThenInclude(x => x.Client)
                .Include(x => x.Ticket).ThenInclude(x => x.Location)
                .Include(x => x.Ticket).ThenInclude(x => x.Assignments).ThenInclude(x => x.User)
                .Include(x => x.Ticket).ThenInclude(x => x.Assignments).ThenInclude(x => x.UserGroup)
                .Include(x => x.Ticket).ThenInclude(x => x.TodoTasks)
                .Where(x => x.TicketId.HasValue
                    && (x.Ticket.TenantId == userService.GetUserTenantId())
                    && (x.UserId == me.Id || (x.UserGroupId.HasValue && userGroupIDs.Contains(x.UserGroupId.Value))
                    && (!string.IsNullOrEmpty(code) || x.Ticket.Code == code)
                    && ((tstatus != null) || (tstatus == StatusFilter.All) || (tstatus == StatusFilter.Active && x.Ticket.IsActive))
                    && ((tstatus != null) || (tstatus == StatusFilter.All) || (tstatus == StatusFilter.Open && x.Ticket.Status == WorkStatus.Open))
                    && ((tstatus != null) || (tstatus == StatusFilter.All) || (tstatus == StatusFilter.Pending && x.Ticket.IsPending))
                    && ((tstatus != null) || (tstatus == StatusFilter.All) || (tstatus == StatusFilter.Completed && x.Ticket.Status == WorkStatus.Completed))
                    && ((tstatus != null) || (tstatus == StatusFilter.All) || (tstatus == StatusFilter.Cancelled && x.Ticket.Status == WorkStatus.Cancelled))
                    && ((tstatus != null) || (tstatus == StatusFilter.All) || (tstatus == StatusFilter.Overdue && x.Ticket.IsOverdue))
                ))
                .Select(x => x.Ticket)
                .Distinct()
                .OrderByDescending(x => x.StartDate)
                .ToListAsync();

            //var tickets = await context.Assignment
            //    .Include(x => x.Ticket).ThenInclude(x => x.TicketType)
            //    .Include(x => x.Ticket).ThenInclude(x => x.Client)
            //    .Include(x => x.Ticket).ThenInclude(x => x.Location)
            //    .Include(x => x.Ticket).ThenInclude(x => x.Assignments).ThenInclude(x => x.User)
            //    .Include(x => x.Ticket).ThenInclude(x => x.Assignments).ThenInclude(x => x.UserGroup)
            //    .Where(x => x.TicketId.HasValue
            //        && (x.Ticket.TenantId == userService.GetUserTenantId())
            //        && (x.UserId == me.Id || (x.UserGroupId.HasValue && userGroupIDs.Contains(x.UserGroupId.Value))
            //        && (x.Ticket.Status == Models.WorkStatus.Open || x.Ticket.Status == Models.WorkStatus.Pending)
            //    ))
            //    .Select(x => x.Ticket)
            //    .Distinct()
            //    .OrderByDescending(x => x.StartDate)
            //    .ToListAsync();

            ViewData["Me"] = me;
            return View(tickets);
        }

        public async Task<IActionResult> TodoTasks()
        {
            var me = await userService.GetCurrentUserAsync();

            var tasks = await context.Assignment
                .Include(x => x.TodoTask).ThenInclude(x => x.TodoTaskType)
                .Include(x => x.TodoTask).ThenInclude(x => x.Ticket).ThenInclude(x => x.Client)
                .Include(x => x.TodoTask).ThenInclude(x => x.Ticket).ThenInclude(x => x.TicketType)
                .Include(x => x.TodoTask).ThenInclude(x => x.Ticket).ThenInclude(x => x.Location)
                .Include(x => x.TodoTask).ThenInclude(x => x.Assignments).ThenInclude(x => x.User)
                .Include(x => x.TodoTask).ThenInclude(x => x.Assignments).ThenInclude(x => x.UserGroup)
                .Where(x => x.TodoTaskId.HasValue
                    && (x.TodoTask.Ticket.TenantId == userService.GetUserTenantId())
                    && (x.UserId == me.Id)
                    && (x.TodoTask.Status == Models.WorkStatus.Open || x.TodoTask.Status == Models.WorkStatus.Pending)
                )
                .Select(x => x.TodoTask)
                .Distinct()
                .OrderByDescending(x => x.StartDate)
                .ToListAsync();

            ViewData["Me"] = me;
            return View(tasks);
        }

        public async Task<IActionResult> Pick(int ticketId)
        {
            var me = await userService.GetCurrentUserAsync();
            var ticket = await context.Tickets
                .Include(x => x.Assignments)
                .Include(x => x.TodoTasks).ThenInclude(x => x.Assignments)
                .FirstOrDefaultAsync(x => x.Id == ticketId);
            if(ticket != null)
            {
                ticket.Assignments.Add(new Models.Assignment { UserId = me.Id, RoleName = "Assigned To"  });
                foreach (var tt in ticket.TodoTasks)
                {
                    tt.Assignments.Add(new Models.Assignment { UserId = me.Id, RoleName = "Assigned To" });
                }
                await context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Tickets));
        }
    }
}