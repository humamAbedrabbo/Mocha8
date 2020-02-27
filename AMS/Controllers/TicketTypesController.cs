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
using Hangfire;

namespace AMS.Controllers
{
    [Authorize]
    public class TicketTypesController : Controller
    {
        private readonly ILogger<TicketTypesController> logger;
        private readonly AmsContext _context;
        private readonly IUserService userService;
        private readonly ITicketGenerator ticketGenerator;

        public TicketTypesController(ILogger<TicketTypesController> logger, AmsContext context, IUserService userService, ITicketGenerator ticketGenerator)
        {
            this.logger = logger;
            _context = context;
            this.userService = userService;
            this.ticketGenerator = ticketGenerator;
        }

        // GET: TicketTypes
        public async Task<IActionResult> Index()
        {
            var amsContext = _context.TicketTypes.Include(t => t.Tenant);
            return View(await userService.GetTicketTypesAsync());
        }

        // GET: TicketTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = await _context.TicketTypes
                .Include(t => t.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketType == null)
            {
                return NotFound();
            }

            return View(ticketType);
        }

        // GET: TicketTypes/Create
        public IActionResult Create()
        {
            ViewData["TenantId"] = userService.GetUserTenantId();
            return View();
        }

        // POST: TicketTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenantId,Name,Code")] TicketType ticketType, int interval, int repeat, string summary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticketType);
                await _context.SaveChangesAsync();
                if (interval > 0 && repeat > 0)
                {
                    if (string.IsNullOrEmpty(summary))
                    {
                        summary = ticketType.Name;
                    }
                    for (int i = 0; i < repeat; i++)
                    {
                        var jobId = BackgroundJob.Schedule(
                            () => ticketGenerator.AddTicket(ticketType.Id, summary, userService.GetUserTenantId().Value),
                            TimeSpan.FromSeconds(interval));
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TenantId"] = userService.GetUserTenantId();


            

            return View(ticketType);
        }



        // GET: TicketTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = await _context.TicketTypes.FindAsync(id);
            if (ticketType == null)
            {
                return NotFound();
            }
            ViewData["TenantId"] = userService.GetUserTenantId();
            return View(ticketType);
        }

        // POST: TicketTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenantId,Name,Code")] TicketType ticketType)
        {
            if (id != ticketType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketTypeExists(ticketType.Id))
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
            ViewData["TenantId"] = userService.GetUserTenantId();
            return View(ticketType);
        }

        // GET: TicketTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = await _context.TicketTypes
                .Include(t => t.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketType == null)
            {
                return NotFound();
            }

            return View(ticketType);
        }

        // POST: TicketTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticketType = await _context.TicketTypes.FindAsync(id);
            _context.TicketTypes.Remove(ticketType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketTypeExists(int id)
        {
            return _context.TicketTypes.Any(e => e.Id == id);
        }
    }
}
