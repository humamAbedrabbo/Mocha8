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
    public class AssetsController : Controller
    {
        private readonly ILogger<AssetsController> logger;
        private readonly AmsContext _context;
        private readonly IUserService userService;

        public AssetsController(ILogger<AssetsController> logger, AmsContext context, IUserService userService)
        {
            this.logger = logger;
            _context = context;
            this.userService = userService;
        }

        // GET: Assets
        public async Task<IActionResult> Index()
        {
            var amsContext = _context.Assets.Include(a => a.AssetType).Include(a => a.Client).Include(a => a.Location).Include(a => a.Parent).Include(a => a.Tenant);
            return View(await amsContext.ToListAsync());
        }

        // GET: Assets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Assets
                .Include(a => a.AssetType)
                .Include(a => a.Client)
                .Include(a => a.Location)
                .Include(a => a.Parent)
                .Include(a => a.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (asset == null)
            {
                return NotFound();
            }

            return View(asset);
        }

        // GET: Assets/Create
        public IActionResult Create()
        {
            ViewData["AssetTypeId"] = new SelectList(_context.AssetTypes, "Id", "Name");
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name");
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name");
            ViewData["ParentId"] = new SelectList(_context.Assets, "Id", "Name");
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name");
            return View();
        }

        // POST: Assets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenantId,Name,CodeNumber,Code,ClientId,AssetTypeId,LocationId,ParentId,IsOn")] Asset asset)
        {
            if (ModelState.IsValid)
            {
                _context.Add(asset);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssetTypeId"] = new SelectList(_context.AssetTypes, "Id", "Name", asset.AssetTypeId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", asset.ClientId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", asset.LocationId);
            ViewData["ParentId"] = new SelectList(_context.Assets, "Id", "Name", asset.ParentId);
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", asset.TenantId);
            return View(asset);
        }

        // GET: Assets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
            {
                return NotFound();
            }
            ViewData["AssetTypeId"] = new SelectList(_context.AssetTypes, "Id", "Name", asset.AssetTypeId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", asset.ClientId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", asset.LocationId);
            ViewData["ParentId"] = new SelectList(_context.Assets, "Id", "Name", asset.ParentId);
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", asset.TenantId);
            return View(asset);
        }

        // POST: Assets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenantId,Name,CodeNumber,Code,ClientId,AssetTypeId,LocationId,ParentId,IsOn")] Asset asset)
        {
            if (id != asset.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asset);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssetExists(asset.Id))
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
            ViewData["AssetTypeId"] = new SelectList(_context.AssetTypes, "Id", "Name", asset.AssetTypeId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", asset.ClientId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", asset.LocationId);
            ViewData["ParentId"] = new SelectList(_context.Assets, "Id", "Name", asset.ParentId);
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", asset.TenantId);
            return View(asset);
        }

        // GET: Assets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Assets
                .Include(a => a.AssetType)
                .Include(a => a.Client)
                .Include(a => a.Location)
                .Include(a => a.Parent)
                .Include(a => a.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (asset == null)
            {
                return NotFound();
            }

            return View(asset);
        }

        // POST: Assets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssetExists(int id)
        {
            return _context.Assets.Any(e => e.Id == id);
        }
    }
}
