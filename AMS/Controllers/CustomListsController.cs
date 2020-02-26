﻿using System;
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
    public class CustomListsController : Controller
    {
        private readonly ILogger<CustomListsController> logger;
        private readonly AmsContext _context;
        private readonly IUserService userService;

        public CustomListsController(ILogger<CustomListsController> logger, AmsContext context, IUserService userService)
        {
            this.logger = logger;
            _context = context;
            this.userService = userService;
        }

        // GET: CustomLists
        public async Task<IActionResult> Index()
        {
            var amsContext = _context.CustomLists.Include(c => c.Tenant);
            return View(await amsContext.ToListAsync());
        }

        // GET: CustomLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customList = await _context.CustomLists
                .Include(c => c.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customList == null)
            {
                return NotFound();
            }

            return View(customList);
        }

        // GET: CustomLists/Create
        public IActionResult Create()
        {
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name");
            return View();
        }

        // POST: CustomLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenantId,Name")] CustomList customList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", customList.TenantId);
            return View(customList);
        }

        // GET: CustomLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customList = await _context.CustomLists.FindAsync(id);
            if (customList == null)
            {
                return NotFound();
            }
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", customList.TenantId);
            return View(customList);
        }

        // POST: CustomLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenantId,Name")] CustomList customList)
        {
            if (id != customList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomListExists(customList.Id))
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
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", customList.TenantId);
            return View(customList);
        }

        // GET: CustomLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customList = await _context.CustomLists
                .Include(c => c.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customList == null)
            {
                return NotFound();
            }

            return View(customList);
        }

        // POST: CustomLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customList = await _context.CustomLists.FindAsync(id);
            _context.CustomLists.Remove(customList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomListExists(int id)
        {
            return _context.CustomLists.Any(e => e.Id == id);
        }
    }
}
