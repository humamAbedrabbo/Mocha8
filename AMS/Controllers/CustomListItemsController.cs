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
    public class CustomListItemsController : Controller
    {
        private readonly ILogger<CustomListItemsController> logger;
        private readonly AmsContext _context;
        private readonly IUserService userService;

        public CustomListItemsController(ILogger<CustomListItemsController> logger, AmsContext context, IUserService userService)
        {
            this.logger = logger;
            _context = context;
            this.userService = userService;
        }

        // GET: CustomListItems
        public async Task<IActionResult> Index(int? listId = null)
        {
            var amsContext = _context.CustomListItems
                .Include(c => c.CustomList)
                .Where(a => (!listId.HasValue || a.CustomListId == listId));
            ViewData["FilterListId"] = listId;
            return View(await amsContext.ToListAsync());
        }

        // GET: CustomListItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customListItem = await _context.CustomListItems
                .Include(c => c.CustomList)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customListItem == null)
            {
                return NotFound();
            }

            return View(customListItem);
        }

        // GET: CustomListItems/Create
        public async Task<IActionResult> Create(int? listId = null)
        {
            var model = new CustomListItem { CustomListId = listId ?? 0 };
            ViewData["CustomListId"] = await userService.GetCustomListsSelectAsync(model.CustomListId);
            ViewData["FilterListId"] = listId;
            return View(model);
        }

        // POST: CustomListItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomListId,Key,Value")] CustomListItem customListItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customListItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomListId"] = await userService.GetCustomListsSelectAsync(customListItem.CustomListId);
            ViewData["FilterListId"] = customListItem.CustomListId;
            return View(customListItem);
        }

        // GET: CustomListItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customListItem = await _context.CustomListItems.FindAsync(id);
            if (customListItem == null)
            {
                return NotFound();
            }
            ViewData["CustomListId"] = await userService.GetCustomListsSelectAsync(customListItem.CustomListId);
            return View(customListItem);
        }

        // POST: CustomListItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomListId,Key,Value")] CustomListItem customListItem)
        {
            if (id != customListItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customListItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomListItemExists(customListItem.Id))
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
            ViewData["CustomListId"] = await userService.GetCustomListsSelectAsync(customListItem.CustomListId);
            return View(customListItem);
        }

        // GET: CustomListItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customListItem = await _context.CustomListItems
                .Include(c => c.CustomList)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customListItem == null)
            {
                return NotFound();
            }

            return View(customListItem);
        }

        // POST: CustomListItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customListItem = await _context.CustomListItems.FindAsync(id);
            _context.CustomListItems.Remove(customListItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomListItemExists(int id)
        {
            return _context.CustomListItems.Any(e => e.Id == id);
        }
    }
}
