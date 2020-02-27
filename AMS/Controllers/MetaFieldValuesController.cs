using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AMS.Data;
using AMS.Models;
using Microsoft.Extensions.Logging;
using AMS.Services;

namespace AMS.Controllers
{
    public class MetaFieldValuesController : Controller
    {
        private readonly ILogger<MetaFieldValuesController> logger;
        private readonly AmsContext _context;
        private readonly IUserService userService;

        public MetaFieldValuesController(ILogger<MetaFieldValuesController> logger, AmsContext context, IUserService userService)
        {
            this.logger = logger;
            _context = context;
            this.userService = userService;
        }

        // GET: MetaFieldValues
        public async Task<IActionResult> Index(int? assetTypeId = null, int? assetId = null, int? ticketTypeId = null, int? ticketId = null)
        {
            var amsContext = _context.MetaFieldValues
                .Include(m => m.Asset).Include(m => m.AssetType)
                .Include(m => m.Field).Include(m => m.Ticket)
                .Include(m => m.TicketType)
                .Where(x =>
                    (!assetTypeId.HasValue || x.AssetTypeId == assetTypeId)
                    && (!assetId.HasValue || x.AssetId == assetId)
                    && (!ticketId.HasValue || x.TicketId == ticketId)
                    && (!ticketTypeId.HasValue || x.TicketTypeId == ticketTypeId)
                );
                
            return View(await amsContext.ToListAsync());
        }

        // GET: MetaFieldValues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metaFieldValue = await _context.MetaFieldValues
                .Include(m => m.Asset)
                .Include(m => m.AssetType)
                .Include(m => m.Field)
                .Include(m => m.Ticket)
                .Include(m => m.TicketType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (metaFieldValue == null)
            {
                return NotFound();
            }

            return View(metaFieldValue);
        }

        // GET: MetaFieldValues/Create
        public async Task<IActionResult> Create()
        {
            await SetViewData();
            return View();
        }

        // POST: MetaFieldValues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FieldId,AssetTypeId,AssetId,TicketTypeId,TicketId,Value")] MetaFieldValue metaFieldValue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(metaFieldValue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await SetViewData(metaFieldValue);
            return View(metaFieldValue);
        }

        private async Task SetViewData(MetaFieldValue metaFieldValue = null)
        {
            ViewData["AssetId"] = await userService.GetAssetsSelectAsync(metaFieldValue?.AssetId);
            ViewData["AssetTypeId"] = await userService.GetAssetTypesSelectAsync(metaFieldValue?.AssetTypeId);
            ViewData["FieldId"] = await userService.GetMetaFieldsSelectAsync(metaFieldValue?.FieldId);
            ViewData["TicketId"] = await userService.GetTicketsSelectAsync(metaFieldValue?.TicketId);
            ViewData["TicketTypeId"] = await userService.GetTicketTypesSelectAsync(metaFieldValue?.TicketTypeId);
        }

        // GET: MetaFieldValues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metaFieldValue = await _context.MetaFieldValues.FindAsync(id);
            if (metaFieldValue == null)
            {
                return NotFound();
            }
            await SetViewData(metaFieldValue);
            return View(metaFieldValue);
        }

        // POST: MetaFieldValues/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FieldId,AssetTypeId,AssetId,TicketTypeId,TicketId,Value")] MetaFieldValue metaFieldValue)
        {
            if (id != metaFieldValue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(metaFieldValue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MetaFieldValueExists(metaFieldValue.Id))
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
            await SetViewData(metaFieldValue);
            return View(metaFieldValue);
        }

        // GET: MetaFieldValues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metaFieldValue = await _context.MetaFieldValues
                .Include(m => m.Asset)
                .Include(m => m.AssetType)
                .Include(m => m.Field)
                .Include(m => m.Ticket)
                .Include(m => m.TicketType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (metaFieldValue == null)
            {
                return NotFound();
            }

            return View(metaFieldValue);
        }

        // POST: MetaFieldValues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var metaFieldValue = await _context.MetaFieldValues.FindAsync(id);
            _context.MetaFieldValues.Remove(metaFieldValue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MetaFieldValueExists(int id)
        {
            return _context.MetaFieldValues.Any(e => e.Id == id);
        }
    }
}
