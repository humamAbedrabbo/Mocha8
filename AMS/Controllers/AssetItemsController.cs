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
    public class AssetItemsController : Controller
    {
        private readonly ILogger<AssetItemsController> logger;
        private readonly AmsContext _context;
        private readonly IUserService userService;

        public AssetItemsController(ILogger<AssetItemsController> logger, AmsContext context, IUserService userService)
        {
            this.logger = logger;
            _context = context;
            this.userService = userService;
        }

        // GET: AssetItems
        public async Task<IActionResult> Index(int? assetId = null, int? itemTypeId = null)
        {
            var amsContext = _context.AssetItems
                .Include(a => a.Asset)
                .Include(a => a.ItemType)
                .Where(a => (!assetId.HasValue || a.AssetId == assetId)
                    && (!itemTypeId.HasValue || a.ItemTypeId == itemTypeId)
                );
            ViewData["FilterAssetId"] = assetId;
            ViewData["FilterItemTypeId"] = itemTypeId;
            return View(await amsContext.ToListAsync());
        }

        // GET: AssetItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assetItem = await _context.AssetItems
                .Include(a => a.Asset)
                .Include(a => a.ItemType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assetItem == null)
            {
                return NotFound();
            }

            return View(assetItem);
        }

        // GET: AssetItems/Create
        public async Task<IActionResult> Create(int? assetId = null, int? itemTypeId = null)
        {
            var model = new AssetItem { AssetId = assetId ?? 0, ItemTypeId = itemTypeId };
            await SetViewData(model);
            return View(model);
        }

        // POST: AssetItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AssetId,ItemTypeId,PartNumber")] AssetItem assetItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assetItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { assetId = assetItem.AssetId });
            }
            await SetViewData(assetItem);
            
            return View(assetItem);
        }

        private async Task SetViewData(AssetItem assetItem = null)
        {
            ViewData["AssetId"] = await userService.GetAssetsSelectAsync(assetItem?.AssetId);
            ViewData["ItemTypeId"] = await userService.GetItemTypesSelectAsync(assetItem?.ItemTypeId);
            ViewData["FilterAssetId"] = assetItem?.AssetId;
            ViewData["FilterItemTypeId"] = assetItem?.ItemTypeId;
        }

        // GET: AssetItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assetItem = await _context.AssetItems.FindAsync(id);
            if (assetItem == null)
            {
                return NotFound();
            }
            await SetViewData(assetItem);
            return View(assetItem);
        }

        // POST: AssetItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AssetId,ItemTypeId,PartNumber")] AssetItem assetItem)
        {
            if (id != assetItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assetItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssetItemExists(assetItem.Id))
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
            await SetViewData(assetItem);
            return View(assetItem);
        }

        // GET: AssetItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assetItem = await _context.AssetItems
                .Include(a => a.Asset)
                .Include(a => a.ItemType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assetItem == null)
            {
                return NotFound();
            }

            return View(assetItem);
        }

        // POST: AssetItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assetItem = await _context.AssetItems.FindAsync(id);
            _context.AssetItems.Remove(assetItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssetItemExists(int id)
        {
            return _context.AssetItems.Any(e => e.Id == id);
        }
    }
}
