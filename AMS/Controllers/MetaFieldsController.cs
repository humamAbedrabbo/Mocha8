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
    public class MetaFieldsController : Controller
    {
        private readonly AmsContext _context;
        private readonly IUserService userService;

        public MetaFieldsController(AmsContext context, IUserService userService)
        {
            _context = context;
            this.userService = userService;
        }

        // GET: MetaFields
        public async Task<IActionResult> Index()
        {
            var amsContext = _context.MetaFields.Include(m => m.CustomList).Include(m => m.Tenant);
            return View(await amsContext.ToListAsync());
        }

        // GET: MetaFields/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metaField = await _context.MetaFields
                .Include(m => m.CustomList)
                .Include(m => m.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (metaField == null)
            {
                return NotFound();
            }

            return View(metaField);
        }

        // GET: MetaFields/Create
        public IActionResult Create()
        {
            ViewData["CustomListId"] = new SelectList(_context.CustomLists, "Id", "Name");
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name");
            return View();
        }

        // POST: MetaFields/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenantId,FieldType,CustomListId,Name")] MetaField metaField)
        {
            if (ModelState.IsValid)
            {
                _context.Add(metaField);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomListId"] = new SelectList(_context.CustomLists, "Id", "Name", metaField.CustomListId);
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", metaField.TenantId);
            return View(metaField);
        }

        // GET: MetaFields/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metaField = await _context.MetaFields.FindAsync(id);
            if (metaField == null)
            {
                return NotFound();
            }
            ViewData["CustomListId"] = new SelectList(_context.CustomLists, "Id", "Name", metaField.CustomListId);
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", metaField.TenantId);
            return View(metaField);
        }

        // POST: MetaFields/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenantId,FieldType,CustomListId,Name")] MetaField metaField)
        {
            if (id != metaField.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(metaField);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MetaFieldExists(metaField.Id))
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
            ViewData["CustomListId"] = new SelectList(_context.CustomLists, "Id", "Name", metaField.CustomListId);
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", metaField.TenantId);
            return View(metaField);
        }

        // GET: MetaFields/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metaField = await _context.MetaFields
                .Include(m => m.CustomList)
                .Include(m => m.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (metaField == null)
            {
                return NotFound();
            }

            return View(metaField);
        }

        // POST: MetaFields/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var metaField = await _context.MetaFields.FindAsync(id);
            _context.MetaFields.Remove(metaField);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MetaFieldExists(int id)
        {
            return _context.MetaFields.Any(e => e.Id == id);
        }
    }
}
