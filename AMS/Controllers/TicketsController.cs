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
using AMS.ViewModels.Archive;
using Humanizer;

namespace AMS.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ILogger<TicketsController> logger;
        private readonly AmsContext _context;
        private readonly IUserService userService;
        private readonly ICodeGenerator codeGenerator;
        private readonly IWebHostEnvironment env;
        private readonly IArchiveAdapter archiver;

        public TicketsController(ILogger<TicketsController> logger, 
            AmsContext context, 
            IUserService userService, 
            ICodeGenerator codeGenerator, 
            IWebHostEnvironment env,
            IArchiveAdapter archiver)
        {
            this.logger = logger;
            _context = context;
            this.userService = userService;
            this.codeGenerator = codeGenerator;
            this.env = env;
            this.archiver = archiver;
        }

        public async Task<IActionResult> AddTask(int ticketId)
        {
            var model = new TodoTask();
            ViewData["Users"] = await userService.GetUsersSelectAsync();
            ViewData["TodoTaskTypeId"] = await userService.GetTodoTaskTypesSelectAsync();
            ViewData["TicketId"] = ticketId;
            ViewData["TenantId"] = userService.GetUserTenantId();

            return PartialView("_AddTask", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTask(TodoTask model)
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
            ViewData["Users"] = await userService.GetUsersSelectAsync();
            ViewData["TodoTaskTypeId"] = await userService.GetTodoTaskTypesSelectAsync();
            ViewData["TicketId"] = model.TicketId;
            ViewData["TenantId"] = userService.GetUserTenantId();

            return PartialView("_AddTask", model);
        }

        public async Task<IActionResult> ChangeState(int id, WorkStatus status)
        {
            await userService.SetTicketState(id, status);
            return RedirectToAction(nameof(Details), new { id = id });
        }

        // GET: Tickets
        public async Task<IActionResult> Index(int? ticketTypeId = null, int? clientId = null, int? locationId = null, int? userGroupId = null, int? userId = null, bool isActive = false)
        {
            ViewData["FilterTicketTypeId"] = ticketTypeId;
            ViewData["FilterTicketType"] = await _context.TicketTypes
                .Include(x => x.Values).ThenInclude(x => x.Field)
                .FirstOrDefaultAsync(x => x.Id == ticketTypeId);
            return View(await userService.GetTicketsAsync(ticketTypeId,clientId,locationId,userGroupId,userId,isActive));
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id, string code = null)
        {
            if(!string.IsNullOrEmpty(code))
            {
                id = await _context.Tickets.Where(x => x.Code == code).Select(x => x.Id).FirstOrDefaultAsync();
            }

            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Client)
                .Include(t => t.Location)
                .Include(t => t.Tenant)
                .Include(t => t.TicketType)
                .Include(t => t.TicketAssets).ThenInclude(a => a.Asset).ThenInclude(a => a.AssetType)
                .Include(t => t.TicketAssets).ThenInclude(a => a.Asset).ThenInclude(a => a.Location)
                .Include(t => t.TodoTasks).ThenInclude(t => t.Assignments).ThenInclude(x => x.User)
                .Include(t => t.TodoTasks).ThenInclude(t => t.Assignments).ThenInclude(x => x.UserGroup)
                .Include(t => t.TodoTasks).ThenInclude(t => t.TodoTaskType)
                .Include(t => t.Assignments).ThenInclude(x => x.User)
                .Include(t => t.Assignments).ThenInclude(x => x.UserGroup)
                .Include(t => t.Values).ThenInclude(v => v.Field)
                .Include(t => t.Attachements)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public async Task<IActionResult> Create()
        {
            await SetViewData();
            var ticket = new Ticket();
            
            return View(ticket);
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenantId,Summary,Description,ClientId,TicketTypeId,LocationId,Status,DueDate,StartDate,CompletionDate,CancellationDate,PendingDate,MarkCompleted,EstDuration,Assignments")] Ticket ticket, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                ticket.CodeNumber = codeGenerator.GetTicketCode(ticket.TenantId).Result;
                if (ticket.TicketTypeId.HasValue)
                {
                    var type = await _context.TicketTypes.FindAsync(ticket.TicketTypeId);
                    ticket.Code = $"{type.Code}{ticket.CodeNumber.ToString("D5")}";
                    var metaValues = await _context.MetaFieldValues
                        .Include(x => x.Field)
                        .Where(x => x.TicketTypeId == type.Id)
                        .ToListAsync();

                    foreach (var value in metaValues)
                    {
                        ticket.Values.Add(new MetaFieldValue { FieldId = value.FieldId, Value = value.Value });
                    }
                }
                else
                {
                    ticket.Code = $"{ticket.CodeNumber.ToString("D5")}";
                }

                var currentUser = await userService.GetCurrentUserAsync();
                if(!ticket.Assignments.Any(x => x.UserId == currentUser.Id))
                {
                    ticket.Assignments.Add(new Assignment { UserId = currentUser.Id });
                }

                _context.Add(ticket);
                await _context.SaveChangesAsync();

                // Add Notification
                if (ticket.Assignments.Count > 0)
                {
                    DateTime now = DateTime.Now;
                    foreach (var a in ticket.Assignments.Where(x => x.UserId.HasValue))
                    {
                        var n = new Notification();
                        n.Message = $"ticket #{ticket.Code} created";
                        n.EntityId = ticket.Id;
                        n.NotificationType = NotificationType.Ticket;
                        n.UserId = a.UserId.Value;
                        n.DateCreated = now;
                        _context.Add(n);
                    }
                    await _context.SaveChangesAsync();
                }

                // Upload files
                long size = files.Sum(f => f.Length);
                var filesPath = Path.Combine(env.WebRootPath, "files", ticket.Id.ToString());
                if(!Directory.Exists(filesPath))
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

                        //DocumentAddModel docAddModel = new DocumentAddModel();
                        //docAddModel.RepositoryId = "1";
                        //docAddModel.Path = ticket.Code;
                        //docAddModel.Name = formFile.FileName;
                        //docAddModel.Title = formFile.FileName;
                        //docAddModel.Length = formFile.Length;
                        //docAddModel.Meta = new Dictionary<string, string>();
                        //docAddModel.Meta["TicketId"] = ticket.Id.ToString();
                        //docAddModel.Meta["TicketSummary"] = ticket.Summary;
                        //docAddModel.Meta["TicketCode"] = ticket.Code;
                        //docAddModel.Meta["TicketUrl"] = $"https://localhost:44388/tickets/details/{ticket.Id}";
                        //docAddModel.UserName = User.Identity.Name;
                        //docAddModel.ContentType = formFile.ContentType;


                        //var docDetail = await archiver.PostDocument(docAddModel);

                        //var chunk = new ChunkAddModel();
                        //chunk.File = formFile;
                        //chunk.CheckInKey = docDetail.CheckInKey;
                        //chunk.DocumentId = docDetail.Id;
                        //chunk.OriginalName = formFile.FileName;
                        //chunk.RepositoryId = docDetail.RepositoryId;
                        //chunk.SortId = 1;
                        //chunk.UserName = User.Identity.Name;
                        //chunk.Version = docDetail.Version;

                        //var chunkBool = await archiver.UploadChunk(chunk);

                        ;
                        Attachment att = new Attachment();
                        att.FileName = formFile.FileName;
                        att.Title = formFile.FileName;
                        att.ContentType = formFile.ContentType;
                        att.TicketId = ticket.Id;
                        att.Length = formFile.Length;
                        //att.RepositoryId = docDetail.RepositoryId;
                        //att.RepositoryName = docDetail.Repository;
                        //att.DocumentId = docDetail.Id;
                        //att.Version = docDetail.Version;
                        //att.Url = $"https://localhost:5001/doc/details/{docDetail.Id}";
                        att.Url = $"/files/{ticket.Id}/{formFile.FileName}";
                        _context.Attachements.Add(att);

                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = ticket.Id });
            }
            await SetViewData(ticket);
            return View(ticket);
        }

        private async Task SetViewData(Ticket ticket = null)
        {
            ViewData["ClientId"] = await userService.GetClientsSelectAsync(ticket?.ClientId);
            ViewData["LocationId"] = await userService.GetLocationsSelectAsync(ticket?.LocationId);
            ViewData["TenantId"] = userService.GetUserTenantId();
            ViewData["TicketTypeId"] = await userService.GetTicketTypesSelectAsync(ticket?.TicketTypeId);
            ViewData["Users"] = await userService.GetUsersSelectAsync();
            ViewData["UserGroups"] = await userService.GetUserGroupsSelectAsync();
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(x => x.Values).ThenInclude(x => x.Field)
                .FirstAsync(x => x.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }
            await SetViewData(ticket);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenantId,Summary,Code,CodeNumber,Description,ClientId,TicketTypeId,LocationId,Status,DueDate,StartDate,CompletionDate,CancellationDate,PendingDate,MarkCompleted,EstDuration,Values")] Ticket ticket, List<IFormFile> files)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);

                    // Add Notification
                    if (ticket.Assignments.Count > 0)
                    {
                        DateTime now = DateTime.Now;
                        foreach (var a in ticket.Assignments.Where(x => x.UserId.HasValue))
                        {
                            var n = new Notification();
                            n.Message = $"ticket #{ticket.Code} updated";
                            n.EntityId = ticket.Id;
                            n.NotificationType = NotificationType.Ticket;
                            n.UserId = a.UserId.Value;
                            n.DateCreated = now;
                            _context.Add(n);
                        }
                    }

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
                            _context.Attachements.Add(att);

                        }
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = ticket.Id });
            }
            await SetViewData(ticket);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Client)
                .Include(t => t.Location)
                .Include(t => t.Tenant)
                .Include(t => t.TicketType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
