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
    public class TodoTaskTypesController : Controller
    {
        private readonly AmsContext _context;

        public TodoTaskTypesController(AmsContext context)
        {
            _context = context;
        }

        // GET: TodoTaskTypes
        public async Task<IActionResult> Index()
        {
            var amsContext = _context.TodoTaskTypes.Include(t => t.Tenant);
            return View(await amsContext.ToListAsync());
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
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name");
            return View();
        }

        // POST: TodoTaskTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", todoTaskType.TenantId);
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
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", todoTaskType.TenantId);
            return View(todoTaskType);
        }

        // POST: TodoTaskTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", todoTaskType.TenantId);
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
