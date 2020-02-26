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

namespace AMS.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly AmsContext _context;

        public TicketsController(AmsContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var amsContext = _context.Tickets.Include(t => t.Client).Include(t => t.Location).Include(t => t.Tenant).Include(t => t.TicketType);
            return View(await amsContext.ToListAsync());
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
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name");
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name");
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name");
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenantId,Summary,CodeNumber,Code,Description,ClientId,TicketTypeId,LocationId,Status,DueDate,StartDate,CompletionDate,CancellationDate,PendingDate,MarkCompleted,EstDuration")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", ticket.ClientId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", ticket.LocationId);
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", ticket.TenantId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", ticket.ClientId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", ticket.LocationId);
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", ticket.TenantId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenantId,Summary,CodeNumber,Code,Description,ClientId,TicketTypeId,LocationId,Status,DueDate,StartDate,CompletionDate,CancellationDate,PendingDate,MarkCompleted,EstDuration")] Ticket ticket)
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", ticket.ClientId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", ticket.LocationId);
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", ticket.TenantId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
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
