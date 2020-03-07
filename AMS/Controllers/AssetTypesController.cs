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
    public class AssetTypesController : Controller
    {
        private readonly ILogger<AssetTypesController> logger;
        private readonly AmsContext _context;
        private readonly IUserService userService;

        public AssetTypesController(ILogger<AssetTypesController> logger, AmsContext context, IUserService userService)
        {
            this.logger = logger;
            _context = context;
            this.userService = userService;
        }

        public async Task<IActionResult> Add()
        {
            var model = new AssetType();
            await SetViewData();
            return PartialView("_Add", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AssetType model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
            }
            await SetViewData();
            return PartialView("_Add", model);
        }

        // GET: AssetTypes
        public async Task<IActionResult> Index()
        {
            return View(await userService.GetAssetTypesAsync());
        }

        // GET: AssetTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assetType = await _context.AssetTypes
                .Include(a => a.Tenant)
                .Include(a => a.Assets).ThenInclude(x => x.Client)
                .Include(a => a.Assets).ThenInclude(x => x.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assetType == null)
            {
                return NotFound();
            }

            return View(assetType);
        }

        // GET: AssetTypes/Create
        public async Task<IActionResult> Create()
        {
            await SetViewData();
            return View();
        }

        public async Task SetViewData()
        {
            ViewData["FieldId"] = await userService.GetMetaFieldsSelectAsync();
            ViewData["TenantId"] = userService.GetUserTenantId();
        }

        // POST: AssetTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenantId,Name,Code,Values")] AssetType assetType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assetType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = assetType.Id });
            }
            await SetViewData();
            return View(assetType);
        }

        // GET: AssetTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assetType = await _context.AssetTypes
                .Include(x => x.Values).ThenInclude(x => x.Field)
                .FirstAsync(x => x.Id == id);
            if (assetType == null)
            {
                return NotFound();
            }
            await SetViewData();
            return View(assetType);
        }

        // POST: AssetTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenantId,Name,Code,Values")] AssetType assetType)
        {
            if (id != assetType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assetType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssetTypeExists(assetType.Id))
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
            await SetViewData();
            return View(assetType);
        }

        // GET: AssetTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assetType = await _context.AssetTypes
                .Include(a => a.Tenant)
                .Include(x => x.Values).ThenInclude(x => x.Field)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assetType == null)
            {
                return NotFound();
            }

            return View(assetType);
        }

        // POST: AssetTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assetType = await _context.AssetTypes.FindAsync(id);
            _context.AssetTypes.Remove(assetType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssetTypeExists(int id)
        {
            return _context.AssetTypes.Any(e => e.Id == id);
        }
    }
}
