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
    public class ClientTypesController : Controller
    {
        private readonly AmsContext _context;
        private readonly IUserService userService;

        public ClientTypesController(AmsContext context, IUserService userService)
        {
            _context = context;
            this.userService = userService;
        }

        // GET: ClientTypes
        public async Task<IActionResult> Index()
        {
            var amsContext = _context.ClientTypes.Include(c => c.Tenant);
            return View(await amsContext.ToListAsync());
        }

        // GET: ClientTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientType = await _context.ClientTypes
                .Include(c => c.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientType == null)
            {
                return NotFound();
            }

            return View(clientType);
        }

        // GET: ClientTypes/Create
        public IActionResult Create()
        {
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name");
            return View();
        }

        // POST: ClientTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenantId,Name")] ClientType clientType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clientType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", clientType.TenantId);
            return View(clientType);
        }

        // GET: ClientTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientType = await _context.ClientTypes.FindAsync(id);
            if (clientType == null)
            {
                return NotFound();
            }
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", clientType.TenantId);
            return View(clientType);
        }

        // POST: ClientTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenantId,Name")] ClientType clientType)
        {
            if (id != clientType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clientType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientTypeExists(clientType.Id))
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
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", clientType.TenantId);
            return View(clientType);
        }

        // GET: ClientTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientType = await _context.ClientTypes
                .Include(c => c.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientType == null)
            {
                return NotFound();
            }

            return View(clientType);
        }

        // POST: ClientTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clientType = await _context.ClientTypes.FindAsync(id);
            _context.ClientTypes.Remove(clientType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientTypeExists(int id)
        {
            return _context.ClientTypes.Any(e => e.Id == id);
        }
    }
}
