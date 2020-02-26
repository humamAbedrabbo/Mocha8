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
    public class LocationsController : Controller
    {
        private readonly ILogger<LocationsController> logger;
        private readonly AmsContext _context;
        private readonly IUserService userService;

        public LocationsController(ILogger<LocationsController> logger, AmsContext context, IUserService userService)
        {
            this.logger = logger;
            _context = context;
            this.userService = userService;
        }

        // GET: Locations
        public async Task<IActionResult> Index()
        {
            var amsContext = _context.Locations.Include(l => l.LocationType).Include(l => l.Parent).Include(l => l.Tenant);
            return View(await amsContext.ToListAsync());
        }

        // GET: Locations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .Include(l => l.LocationType)
                .Include(l => l.Parent)
                .Include(l => l.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: Locations/Create
        public IActionResult Create()
        {
            ViewData["LocationTypeId"] = new SelectList(_context.LocationTypes, "Id", "Name");
            ViewData["ParentId"] = new SelectList(_context.Locations, "Id", "Name");
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name");
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenantId,Name,LocationTypeId,ParentId")] Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationTypeId"] = new SelectList(_context.LocationTypes, "Id", "Name", location.LocationTypeId);
            ViewData["ParentId"] = new SelectList(_context.Locations, "Id", "Name", location.ParentId);
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", location.TenantId);
            return View(location);
        }

        // GET: Locations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            ViewData["LocationTypeId"] = new SelectList(_context.LocationTypes, "Id", "Name", location.LocationTypeId);
            ViewData["ParentId"] = new SelectList(_context.Locations, "Id", "Name", location.ParentId);
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", location.TenantId);
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenantId,Name,LocationTypeId,ParentId")] Location location)
        {
            if (id != location.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.Id))
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
            ViewData["LocationTypeId"] = new SelectList(_context.LocationTypes, "Id", "Name", location.LocationTypeId);
            ViewData["ParentId"] = new SelectList(_context.Locations, "Id", "Name", location.ParentId);
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", location.TenantId);
            return View(location);
        }

        // GET: Locations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .Include(l => l.LocationType)
                .Include(l => l.Parent)
                .Include(l => l.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.Id == id);
        }
    }
}
