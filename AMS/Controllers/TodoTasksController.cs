using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AMS.Data;
using AMS.Models;
using Microsoft.AspNetCore.Authorization;
using AMS.Services;
using Microsoft.Extensions.Logging;

namespace AMS.Controllers
{
    [Authorize]
    public class TodoTasksController : Controller
    {
        private readonly ILogger<TodoTasksController> logger;
        private readonly AmsContext _context;
        private readonly IUserService userService;

        public TodoTasksController(ILogger<TodoTasksController> logger, AmsContext context, IUserService userService)
        {
            this.logger = logger;
            _context = context;
            this.userService = userService;
        }

        public async Task<IActionResult> Add()
        {
            var model = new TodoTask();
            await SetViewData();
            return PartialView("_Add", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TodoTask model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
            }
            await SetViewData(model);
            return PartialView("_Add", model);
        }

        public async Task<IActionResult> ChangeState(int id, WorkStatus status)
        {
            await userService.SetTaskState(id, status);
            return RedirectToAction(nameof(Index));
        }

        public async Task<bool> CompleteTask(int id)
        {
            return await userService.SetTaskState(id, WorkStatus.Completed);
        }

        // GET: TodoTasks
        public async Task<IActionResult> Index(int? todoTaskTypeId = null, int? ticketId = null, int? userGroupId = null, int? userId = null, bool isActive = true)
        {
            return View(await userService.GetTodoTasksAsync(todoTaskTypeId, ticketId, userGroupId, userId, isActive));
        }

        // GET: TodoTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoTask = await _context.TodoTasks
                .Include(t => t.Tenant)
                .Include(t => t.Ticket).ThenInclude(x => x.Location)
                .Include(t => t.Ticket).ThenInclude(x => x.Client)
                .Include(t => t.Ticket).ThenInclude(x => x.Values).ThenInclude(v => v.Field)
                .Include(t => t.TodoTaskType)
                .Include(t => t.Assignments).ThenInclude(a => a.User)
                .Include(t => t.Assignments).ThenInclude(a => a.UserGroup)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoTask == null)
            {
                return NotFound();
            }

            return View(todoTask);
        }

        // GET: TodoTasks/Create
        public async Task<IActionResult> Create()
        {
            await SetViewData();
            var task = new TodoTask();
            return View(task);
        }

        // POST: TodoTasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenantId,Summary,Description,TicketId,TodoTaskTypeId,Status,DueDate,StartDate,CompletionDate,CancellationDate,PendingDate,MarkCompleted,EstDuration")] TodoTask todoTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(todoTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await SetViewData(todoTask);
            return View(todoTask);
        }

        private async Task SetViewData(TodoTask todoTask = null)
        {
            ViewData["TenantId"] = userService.GetUserTenantId();
            ViewData["TicketId"] = await userService.GetTicketsSelectAsync(todoTask?.TicketId);
            ViewData["TodoTaskTypeId"] = await userService.GetTodoTaskTypesSelectAsync(todoTask?.TodoTaskTypeId);
        }

        // GET: TodoTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoTask = await _context.TodoTasks.FindAsync(id);
            if (todoTask == null)
            {
                return NotFound();
            }
            await SetViewData(todoTask);
            return View(todoTask);
        }

        // POST: TodoTasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenantId,Summary,Description,TicketId,TodoTaskTypeId,Status,DueDate,StartDate,CompletionDate,CancellationDate,PendingDate,MarkCompleted,EstDuration")] TodoTask todoTask)
        {
            if (id != todoTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todoTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoTaskExists(todoTask.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            await SetViewData(todoTask);
            return View(todoTask);
        }

        // GET: TodoTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoTask = await _context.TodoTasks
                .Include(t => t.Tenant)
                .Include(t => t.Ticket)
                .Include(t => t.TodoTaskType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoTask == null)
            {
                return NotFound();
            }

            return View(todoTask);
        }

        // POST: TodoTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todoTask = await _context.TodoTasks.FindAsync(id);
            _context.TodoTasks.Remove(todoTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoTaskExists(int id)
        {
            return _context.TodoTasks.Any(e => e.Id == id);
        }
    }
}
