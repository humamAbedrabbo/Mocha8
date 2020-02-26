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
    public class TodoTaskTypesController : Controller
    {
        private readonly ILogger<TodoTaskTypesController> logger;
        private readonly AmsContext _context;
        private readonly IUserService userService;

        public TodoTaskTypesController(ILogger<TodoTaskTypesController> logger, AmsContext context, IUserService userService)
        {
            this.logger = logger;
            _context = context;
            this.userService = userService;
        }

        // GET: TodoTaskTypes
        public async Task<IActionResult> Index()
        {
            return View(await userService.GetTodoTaskTypesAsync());
        }

        // GET: TodoTaskTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoTaskType = await _context.TodoTaskTypes
                .Include(t => t.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoTaskType == null)
            {
                return NotFound();
            }

            return View(todoTaskType);
        }

        // GET: TodoTaskTypes/Create
        public IActionResult Create()
        {
            ViewData["TenantId"] = userService.GetUserTenantId();
            return View();
        }

        // POST: TodoTaskTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenantId,Name")] TodoTaskType todoTaskType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(todoTaskType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TenantId"] = userService.GetUserTenantId();
            return View(todoTaskType);
        }

        // GET: TodoTaskTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoTaskType = await _context.TodoTaskTypes.FindAsync(id);
            if (todoTaskType == null)
            {
                return NotFound();
            }
            ViewData["TenantId"] = userService.GetUserTenantId();
            return View(todoTaskType);
        }

        // POST: TodoTaskTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenantId,Name")] TodoTaskType todoTaskType)
        {
            if (id != todoTaskType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todoTaskType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoTaskTypeExists(todoTaskType.Id))
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
            ViewData["TenantId"] = userService.GetUserTenantId();
            return View(todoTaskType);
        }

        // GET: TodoTaskTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoTaskType = await _context.TodoTaskTypes
                .Include(t => t.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoTaskType == null)
            {
                return NotFound();
            }

            return View(todoTaskType);
        }

        // POST: TodoTaskTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todoTaskType = await _context.TodoTaskTypes.FindAsync(id);
            _context.TodoTaskTypes.Remove(todoTaskType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoTaskTypeExists(int id)
        {
            return _context.TodoTaskTypes.Any(e => e.Id == id);
        }
    }
}
