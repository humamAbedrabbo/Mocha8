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
    public class TicketsController : Controller
    {
        private readonly ILogger<TicketsController> logger;
        private readonly AmsContext _context;
        private readonly IUserService userService;
        private readonly ICodeGenerator codeGenerator;

        public TicketsController(ILogger<TicketsController> logger, AmsContext context, IUserService userService, ICodeGenerator codeGenerator)
        {
            this.logger = logger;
            _context = context;
            this.userService = userService;
            this.codeGenerator = codeGenerator;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            return View(await userService.GetTicketsAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Client)
                .Include(t => t.Location)
                .Include(t => t.Tenant)
                .Include(t => t.TicketType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public async Task<IActionResult> Create()
        {
            await SetViewData();
            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenantId,Summary,Description,ClientId,TicketTypeId,LocationId,Status,DueDate,StartDate,CompletionDate,CancellationDate,PendingDate,MarkCompleted,EstDuration")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.CodeNumber = codeGenerator.GetTicketCode(ticket.TenantId).Result;
                if (ticket.TicketTypeId.HasValue)
                {
                    var type = await _context.TicketTypes.FindAsync(ticket.TicketTypeId);
                    ticket.Code = $"{type.Code}{ticket.CodeNumber.ToString("D5")}";
                }
                else
                {
                    ticket.Code = $"{ticket.CodeNumber.ToString("D5")}";
                }

                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await SetViewData(ticket);
            return View(ticket);
        }

        private async Task SetViewData(Ticket ticket = null)
        {
            ViewData["ClientId"] = await userService.GetClientsSelectAsync(ticket?.ClientId);
            ViewData["LocationId"] = await userService.GetLocationsSelectAsync(ticket?.LocationId);
            ViewData["TenantId"] = userService.GetUserTenantId();
            ViewData["TicketTypeId"] = await userService.GetTicketTypesSelectAsync(ticket?.TicketTypeId);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            await SetViewData(ticket);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenantId,Summary,Code,CodeNumber,Description,ClientId,TicketTypeId,LocationId,Status,DueDate,StartDate,CompletionDate,CancellationDate,PendingDate,MarkCompleted,EstDuration")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            await SetViewData(ticket);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Client)
                .Include(t => t.Location)
                .Include(t => t.Tenant)
                .Include(t => t.TicketType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
