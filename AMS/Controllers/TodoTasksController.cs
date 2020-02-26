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

namespace AMS.Controllers
{
    [Authorize]
    public class TodoTasksController : Controller
    {
        private readonly AmsContext _context;

        public TodoTasksController(AmsContext context)
        {
            _context = context;
        }

        // GET: TodoTasks
        public async Task<IActionResult> Index()
        {
            var amsContext = _context.TodoTasks.Include(t => t.Tenant).Include(t => t.Ticket).Include(t => t.TodoTaskType);
            return View(await amsContext.ToListAsync());
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
                .Include(t => t.Ticket)
                .Include(t => t.TodoTaskType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoTask == null)
            {
                return NotFound();
            }

            return View(todoTask);
        }

        // GET: TodoTasks/Create
        public IActionResult Create()
        {
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name");
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Summary");
            ViewData["TodoTaskTypeId"] = new SelectList(_context.TodoTaskTypes, "Id", "Name");
            return View();
        }

        // POST: TodoTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", todoTask.TenantId);
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Summary", todoTask.TicketId);
            ViewData["TodoTaskTypeId"] = new SelectList(_context.TodoTaskTypes, "Id", "Name", todoTask.TodoTaskTypeId);
            return View(todoTask);
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
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", todoTask.TenantId);
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Summary", todoTask.TicketId);
            ViewData["TodoTaskTypeId"] = new SelectList(_context.TodoTaskTypes, "Id", "Name", todoTask.TodoTaskTypeId);
            return View(todoTask);
        }

        // POST: TodoTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", todoTask.TenantId);
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Summary", todoTask.TicketId);
            ViewData["TodoTaskTypeId"] = new SelectList(_context.TodoTaskTypes, "Id", "Name", todoTask.TodoTaskTypeId);
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
