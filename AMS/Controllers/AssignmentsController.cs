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

namespace AMS.Controllers
{
    [Authorize]
    public class AssignmentsController : Controller
    {
        private readonly AmsContext _context;
        private readonly IUserService userService;

        public AssignmentsController(AmsContext context, IUserService userService)
        {
            _context = context;
            this.userService = userService;
        }

        // GET: Assignments
        public async Task<IActionResult> Index()
        {
            var amsContext = _context.Assignment.Include(a => a.Ticket).Include(a => a.TodoTask).Include(a => a.User).Include(a => a.UserGroup);
            return View(await amsContext.ToListAsync());
        }

        // GET: Assignments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignment
                .Include(a => a.Ticket)
                .Include(a => a.TodoTask)
                .Include(a => a.User)
                .Include(a => a.UserGroup)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        // GET: Assignments/Create
        public IActionResult Create()
        {
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Summary");
            ViewData["TodoTaskId"] = new SelectList(_context.TodoTasks, "Id", "Summary");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["UserGroupId"] = new SelectList(_context.UserGroups, "Id", "Name");
            return View();
        }

        // POST: Assignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserGroupId,UserId,RoleName,TicketId,TodoTaskId")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Summary", assignment.TicketId);
            ViewData["TodoTaskId"] = new SelectList(_context.TodoTasks, "Id", "Summary", assignment.TodoTaskId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", assignment.UserId);
            ViewData["UserGroupId"] = new SelectList(_context.UserGroups, "Id", "Name", assignment.UserGroupId);
            return View(assignment);
        }

        // GET: Assignments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignment.FindAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Summary", assignment.TicketId);
            ViewData["TodoTaskId"] = new SelectList(_context.TodoTasks, "Id", "Summary", assignment.TodoTaskId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", assignment.UserId);
            ViewData["UserGroupId"] = new SelectList(_context.UserGroups, "Id", "Name", assignment.UserGroupId);
            return View(assignment);
        }

        // POST: Assignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserGroupId,UserId,RoleName,TicketId,TodoTaskId")] Assignment assignment)
        {
            if (id != assignment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignmentExists(assignment.Id))
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
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Summary", assignment.TicketId);
            ViewData["TodoTaskId"] = new SelectList(_context.TodoTasks, "Id", "Summary", assignment.TodoTaskId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", assignment.UserId);
            ViewData["UserGroupId"] = new SelectList(_context.UserGroups, "Id", "Name", assignment.UserGroupId);
            return View(assignment);
        }

        // GET: Assignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignment
                .Include(a => a.Ticket)
                .Include(a => a.TodoTask)
                .Include(a => a.User)
                .Include(a => a.UserGroup)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assignment = await _context.Assignment.FindAsync(id);
            _context.Assignment.Remove(assignment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssignmentExists(int id)
        {
            return _context.Assignment.Any(e => e.Id == id);
        }
    }
}
