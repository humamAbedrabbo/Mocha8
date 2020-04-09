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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Humanizer;

namespace AMS.Controllers
{
    [Authorize]
    public class TodoTasksController : Controller
    {
        private readonly ILogger<TodoTasksController> logger;
        private readonly AmsContext _context;
        private readonly IUserService userService;
        private readonly IWebHostEnvironment env;
        private readonly IArchiveAdapter archiver;

        public TodoTasksController(ILogger<TodoTasksController> logger, 
            AmsContext context, 
            IUserService userService,
            IWebHostEnvironment env,
            IArchiveAdapter archiver)
        {
            this.logger = logger;
            _context = context;
            this.userService = userService;
            this.env = env;
            this.archiver = archiver;
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

                // Add Notification
                if (model.Assignments.Count > 0)
                {
                    DateTime now = DateTime.Now;
                    foreach (var a in model.Assignments.Where(x => x.UserId.HasValue))
                    {
                        var n = new Notification();
                        n.Message = model.Summary.Truncate(10);
                        n.EntityId = model.Id;
                        n.NotificationType = NotificationType.Task;
                        n.UserId = a.UserId.Value;
                        n.DateCreated = now;
                        _context.Add(n);
                    }
                }

                await _context.SaveChangesAsync();
            }
            await SetViewData(model);
            return PartialView("_Add", model);
        }

        public async Task<IActionResult> ChangeState(int id, WorkStatus status)
        {
            await userService.SetTaskState(id, status);
            return RedirectToAction(nameof(Details), new { id = id });
        }

        public async Task<bool> CompleteTask(int id)
        {
            return await userService.SetTaskState(id, WorkStatus.Completed);
        }

        // GET: TodoTasks
        public async Task<IActionResult> Index(int? todoTaskTypeId = null, int? ticketId = null, int? userGroupId = null, int? userId = null, bool isActive = false)
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
                .Include(t => t.Ticket).ThenInclude(x => x.Attachements)
                .Include(t => t.Ticket).ThenInclude(x => x.TicketType)
                .Include(t => t.Ticket).ThenInclude(x => x.TicketAssets).ThenInclude(x => x.Asset).ThenInclude(x => x.AssetType)
                .Include(t => t.Ticket).ThenInclude(x => x.TicketAssets).ThenInclude(x => x.Asset).ThenInclude(x => x.Location)
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
        public async Task<IActionResult> Create([Bind("Id,TenantId,Summary,Description,TicketId,TodoTaskTypeId,Status,DueDate,StartDate,CompletionDate,CancellationDate,PendingDate,MarkCompleted,EstDuration,Assignments")] TodoTask todoTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(todoTask);
                var currentUser = await userService.GetCurrentUserAsync();
                if (todoTask.Assignments.Count == 0)
                {
                    todoTask.Assignments.Add(new Assignment { UserId = currentUser.Id });
                }
                await _context.SaveChangesAsync();

                // Add Notification
                if (todoTask.Assignments.Count > 0)
                {
                    DateTime now = DateTime.Now;
                    foreach (var a in todoTask.Assignments.Where(x => x.UserId.HasValue))
                    {
                        var n = new Notification();
                        n.Message = todoTask.Summary.Truncate(10);
                        n.EntityId = todoTask.Id;
                        n.NotificationType = NotificationType.Task;
                        n.UserId = a.UserId.Value;
                        n.DateCreated = now;
                        _context.Add(n);
                    }
                }


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = todoTask.Id });
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenantId,Summary,Description,TicketId,TodoTaskTypeId,Status,DueDate,StartDate,CompletionDate,CancellationDate,PendingDate,MarkCompleted,EstDuration")] TodoTask todoTask, List<IFormFile> files)
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
                    if(todoTask.TicketId.HasValue)
                    {
                        var ticket = await _context.Tickets
                            .Include(x => x.Attachements)
                            .Where(x => x.Id == todoTask.TicketId.Value)
                            .FirstOrDefaultAsync();

                        var currentUser = await userService.GetCurrentUserAsync();

                        // Upload files
                        long size = files.Sum(f => f.Length);
                        var filesPath = Path.Combine(env.WebRootPath, "files", ticket.Id.ToString());
                        if (!Directory.Exists(filesPath))
                        {
                            Directory.CreateDirectory(filesPath);
                        }

                        foreach (var formFile in files)
                        {
                            if (formFile.Length > 0)
                            {
                                var filePath = Path.Combine(filesPath, formFile.FileName);

                                using (var stream = System.IO.File.Create(filePath))
                                {
                                    await formFile.CopyToAsync(stream);
                                }

                                Attachment att = new Attachment();
                                att.FileName = formFile.FileName;
                                att.Title = formFile.FileName;
                                att.ContentType = formFile.ContentType;
                                att.TicketId = ticket.Id;
                                att.Length = formFile.Length;
                                att.Url = $"/files/{ticket.Id}/{formFile.FileName}";
                                att.CreatedBy = currentUser.DisplayName;
                                att.CreatedOn = DateTime.Now;
                                att.Version = 1;

                                _context.Attachements.Add(att);

                            }
                        }
                    }
                    // Add Notification
                    if (todoTask.Assignments.Count > 0)
                    {
                        DateTime now = DateTime.Now;
                        foreach (var a in todoTask.Assignments.Where(x => x.UserId.HasValue))
                        {
                            var n = new Notification();
                            n.Message = todoTask.Summary.Truncate(10);
                            n.EntityId = todoTask.Id;
                            n.NotificationType = NotificationType.Task;
                            n.UserId = a.UserId.Value;
                            n.DateCreated = now;
                            _context.Add(n);
                        }
                    }


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
                return RedirectToAction(nameof(Details), new { id = todoTask.Id });
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
