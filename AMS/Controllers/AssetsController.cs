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
        private readonly ICodeGenerator codeGenerator;

        public AssetsController(ILogger<AssetsController> logger, AmsContext context, IUserService userService, ICodeGenerator codeGenerator)
        {
            this.logger = logger;
            _context = context;
            this.userService = userService;
            this.codeGenerator = codeGenerator;
        }

        // GET: Assets
        public async Task<IActionResult> Index()
        {
            return View(await userService.GetAssetsAsync());
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
        public async Task<IActionResult> Create()
        {
            await SetViewData();
            return View();
        }

        // POST: Assets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenantId,Name,ClientId,AssetTypeId,LocationId,ParentId,IsOn")] Asset asset)
        {
            if (ModelState.IsValid)
            {
                asset.CodeNumber = codeGenerator.GetAssetCode(asset.TenantId).Result;
                if(asset.AssetTypeId.HasValue)
                {
                    var type = await _context.AssetTypes.FindAsync(asset.AssetTypeId);
                    asset.Code = $"{type.Code}{asset.CodeNumber.ToString("D5")}";
                    var metaValues = await _context.MetaFieldValues
                        .Include(x => x.Field)
                        .Where(x => x.AssetTypeId == type.Id)
                        .ToListAsync();
                    foreach(var value in metaValues)
                    {
                        asset.Values.Add(new MetaFieldValue { FieldId = value.FieldId, Value = value.Value });
                    }
                }
                else
                {
                    asset.Code = $"{asset.CodeNumber.ToString("D5")}";
                }
                
                
                _context.Add(asset);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await SetViewData();
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
            await SetViewData(asset);
            return View(asset);
        }

        private async Task SetViewData(Asset asset = null)
        {
            ViewData["AssetTypeId"] = await userService.GetAssetTypesSelectAsync(asset?.AssetTypeId);
            ViewData["ClientId"] = await userService.GetClientsSelectAsync(asset?.ClientId);
            ViewData["LocationId"] = await userService.GetLocationsSelectAsync(asset?.LocationId);
            ViewData["ParentId"] = await userService.GetAssetsSelectAsync(asset?.ParentId);
            ViewData["TenantId"] = userService.GetUserTenantId();
        }

        // POST: Assets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenantId,Name,Code,CodeNumber,ClientId,AssetTypeId,LocationId,ParentId,IsOn")] Asset asset)
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
            await SetViewData(asset);
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
