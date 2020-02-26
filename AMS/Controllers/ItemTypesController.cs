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
    public class ItemTypesController : Controller
    {
        private readonly AmsContext _context;

        public ItemTypesController(AmsContext context)
        {
            _context = context;
        }

        // GET: ItemTypes
        public async Task<IActionResult> Index()
        {
            var amsContext = _context.ItemTypes.Include(i => i.Tenant);
            return View(await amsContext.ToListAsync());
        }

        // GET: ItemTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemType = await _context.ItemTypes
                .Include(i => i.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemType == null)
            {
                return NotFound();
            }

            return View(itemType);
        }

        // GET: ItemTypes/Create
        public IActionResult Create()
        {
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name");
            return View();
        }

        // POST: ItemTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenantId,Name")] ItemType itemType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", itemType.TenantId);
            return View(itemType);
        }

        // GET: ItemTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemType = await _context.ItemTypes.FindAsync(id);
            if (itemType == null)
            {
                return NotFound();
            }
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", itemType.TenantId);
            return View(itemType);
        }

        // POST: ItemTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenantId,Name")] ItemType itemType)
        {
            if (id != itemType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemTypeExists(itemType.Id))
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
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", itemType.TenantId);
            return View(itemType);
        }

        // GET: ItemTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemType = await _context.ItemTypes
                .Include(i => i.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemType == null)
            {
                return NotFound();
            }

            return View(itemType);
        }

        // POST: ItemTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemType = await _context.ItemTypes.FindAsync(id);
            _context.ItemTypes.Remove(itemType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemTypeExists(int id)
        {
            return _context.ItemTypes.Any(e => e.Id == id);
        }
    }
}
