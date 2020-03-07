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
    public class AssetCustodiansController : Controller
    {
        private readonly ILogger<AssetCustodiansController> logger;
        private readonly AmsContext _context;
        private readonly IUserService userService;

        public AssetCustodiansController(ILogger<AssetCustodiansController> logger, AmsContext context, IUserService userService)
        {
            this.logger = logger;
            _context = context;
            this.userService = userService;
        }

        public async Task<IActionResult> AddCustodian(int assetId)
        {
            var model = new AssetCustdian { AssetId = assetId };
            await SetViewData(model);
            return PartialView("_AddCustodian", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCustodian(AssetCustdian model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
            }
            await SetViewData(model);
            return PartialView("_AddCustodian", model);
        }

        // GET: AssetCustdians
        public async Task<IActionResult> Index(int? assetId = null)
        {
            var amsContext = _context.AssetCustodians
                .Include(a => a.Asset)
                .Include(a => a.User)
                .Where(a => (!assetId.HasValue || a.AssetId == assetId));
            ViewData["FilterAssetId"] = assetId;
            return View(await amsContext.ToListAsync());
        }

        // GET: AssetCustdians/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assetCustdian = await _context.AssetCustodians
                .Include(a => a.Asset)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assetCustdian == null)
            {
                return NotFound();
            }

            return View(assetCustdian);
        }

        // GET: AssetCustdians/Create
        public async Task<IActionResult> Create(int? assetId = null)
        {
            var model = new AssetCustdian { AssetId = assetId ?? 0};
            await SetViewData(model);
            return View(model);
        }

        // POST: AssetCustdians/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AssetId,UserId,Name,RoleName")] AssetCustdian assetCustdian)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assetCustdian);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { assetId = assetCustdian?.AssetId });
            }
            await SetViewData(assetCustdian);
            return View(assetCustdian);
        }

        private async Task SetViewData(AssetCustdian assetCustdian = null)
        {
            ViewData["AssetId"] = await userService.GetAssetsSelectAsync(assetCustdian?.AssetId);
            ViewData["UserId"] = await userService.GetUsersSelectAsync(assetCustdian?.UserId);
            ViewData["FilterAssetId"] = assetCustdian?.AssetId;
        }

        // GET: AssetCustdians/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assetCustdian = await _context.AssetCustodians.FindAsync(id);
            if (assetCustdian == null)
            {
                return NotFound();
            }
            await SetViewData(assetCustdian);
            return View(assetCustdian);
        }

        // POST: AssetCustdians/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AssetId,UserId,Name,RoleName")] AssetCustdian assetCustdian)
        {
            if (id != assetCustdian.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assetCustdian);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssetCustdianExists(assetCustdian.Id))
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
            await SetViewData(assetCustdian);
            return View(assetCustdian);
        }

        // GET: AssetCustdians/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assetCustdian = await _context.AssetCustodians
                .Include(a => a.Asset)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assetCustdian == null)
            {
                return NotFound();
            }

            return View(assetCustdian);
        }

        // POST: AssetCustdians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assetCustdian = await _context.AssetCustodians.FindAsync(id);
            _context.AssetCustodians.Remove(assetCustdian);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssetCustdianExists(int id)
        {
            return _context.AssetCustodians.Any(e => e.Id == id);
        }
    }
}
