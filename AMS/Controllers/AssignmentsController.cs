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
    public class AssignmentsController : Controller
    {
        private readonly ILogger<AssignmentsController> logger;
        private readonly AmsContext _context;
        private readonly IUserService userService;

        public AssignmentsController(ILogger<AssignmentsController> logger, AmsContext context, IUserService userService)
        {
            this.logger = logger;
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
        public async Task<IActionResult> Create()
        {
            await SetViewData();
            return View();
        }

        // POST: Assignments/Create
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
            await SetViewData(assignment);
            return View(assignment);
        }

        private async Task SetViewData(Assignment assignment = null)
        {
            ViewData["TicketId"] = await userService.GetTicketsSelectAsync(assignment?.TicketId);
            ViewData["TodoTaskId"] = await userService.GetTodoTasksSelectAsync(assignment?.TodoTaskId);
            ViewData["UserId"] = await userService.GetUsersSelectAsync(assignment?.UserId);
            ViewData["UserGroupId"] = await userService.GetUserGroupsSelectAsync(assignment?.UserGroupId);
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
            await SetViewData(assignment);
            return View(assignment);
        }

        // POST: Assignments/Edit/5
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
            await SetViewData(assignment);
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
