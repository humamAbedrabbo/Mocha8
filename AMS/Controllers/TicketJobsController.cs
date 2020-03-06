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
using Hangfire;

namespace AMS.Controllers
{
    public class TicketJobsController : Controller
    {
        private readonly ILogger<TicketJobsController> logger;
        private readonly AmsContext _context;
        private readonly IUserService userService;
        private readonly ITicketGenerator ticketGenerator;

        public TicketJobsController(ILogger<TicketJobsController> logger, AmsContext context, IUserService userService, ITicketGenerator ticketGenerator)
        {
            this.logger = logger;
            _context = context;
            this.userService = userService;
            this.ticketGenerator = ticketGenerator;
        }

        // GET: TicketJobs
        public async Task<IActionResult> Index()
        {
            return View(await userService.GetTicketJobsAsync());
        }

        // GET: TicketJobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketJob = await _context.TicketJobs
                .Include(t => t.AssetType)
                .Include(t => t.Client)
                .Include(t => t.Location)
                .Include(t => t.Owner)
                .Include(t => t.Tenant)
                .Include(t => t.TicketType)
                .Include(t => t.UserGroup)
                .Include(t => t.TodoTaskTypes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketJob == null)
            {
                return NotFound();
            }

            return View(ticketJob);
        }

        // GET: TicketJobs/Create
        public async Task<IActionResult> Create()
        {
            await SetViewData();
            return View();
        }

        public async Task SetViewData(TicketJob ticketJob = null)
        {
            ViewData["AssetTypeId"] = await userService.GetAssetTypesSelectAsync(ticketJob?.AssetTypeId);
            ViewData["ClientId"] = await userService.GetClientsSelectAsync(ticketJob?.ClientId);
            ViewData["LocationId"] = await userService.GetLocationsSelectAsync(ticketJob?.LocationId);
            ViewData["OwnerId"] = await userService.GetUsersSelectAsync(ticketJob?.OwnerId);
            ViewData["TenantId"] = userService.GetUserTenantId();
            ViewData["TicketTypeId"] = await userService.GetTicketTypesSelectAsync(ticketJob?.TicketTypeId);
            ViewData["UserGroupId"] = await userService.GetUserGroupsSelectAsync(ticketJob?.UserGroupId);
        }

        // POST: TicketJobs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenantId,TicketTypeId,AssetTypeId,ClientId,LocationId,OwnerId,UserGroupId,Summary,JobId,TodoTaskTypes,IsOn")] TicketJob ticketJob)
        {
            if (ModelState.IsValid)
            {
                if(string.IsNullOrEmpty(ticketJob.JobId))
                {
                    ticketJob.JobId = Guid.NewGuid().ToString("D");
                }
                _context.Add(ticketJob);
                await _context.SaveChangesAsync();

                if(ticketJob.IsOn)
                {
                    RecurringJob.AddOrUpdate(ticketJob.JobId, () => ticketGenerator.RunTicketJob(ticketJob.Id), Cron.Minutely);
                }
                else
                {
                    RecurringJob.RemoveIfExists(ticketJob.JobId);
                }
                return RedirectToAction(nameof(Index));
            }
            await SetViewData(ticketJob);
            return View(ticketJob);
        }

        // GET: TicketJobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketJob = await _context.TicketJobs.FindAsync(id);
            if (ticketJob == null)
            {
                return NotFound();
            }
            await SetViewData(ticketJob);
            return View(ticketJob);
        }

        // POST: TicketJobs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenantId,TicketTypeId,AssetTypeId,ClientId,LocationId,OwnerId,UserGroupId,Summary,JobId,TodoTaskTypes,IsOn")] TicketJob ticketJob)
        {
            if (id != ticketJob.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketJob);
                    await _context.SaveChangesAsync();
                    if (ticketJob.IsOn)
                    {
                        RecurringJob.AddOrUpdate(ticketJob.JobId, () => ticketGenerator.RunTicketJob(ticketJob.Id), Cron.Minutely);
                    }
                    else
                    {
                        RecurringJob.RemoveIfExists(ticketJob.JobId);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketJobExists(ticketJob.Id))
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
            await SetViewData(ticketJob);
            return View(ticketJob);
        }

        // GET: TicketJobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketJob = await _context.TicketJobs
                .Include(t => t.AssetType)
                .Include(t => t.Client)
                .Include(t => t.Location)
                .Include(t => t.Owner)
                .Include(t => t.Tenant)
                .Include(t => t.TicketType)
                .Include(t => t.UserGroup)
                .Include(t => t.TodoTaskTypes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketJob == null)
            {
                return NotFound();
            }
            
            
            
            return View(ticketJob);
        }

        // POST: TicketJobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticketJob = await _context.TicketJobs.FindAsync(id);

            RecurringJob.RemoveIfExists(ticketJob.JobId);

            _context.TicketJobs.Remove(ticketJob);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketJobExists(int id)
        {
            return _context.TicketJobs.Any(e => e.Id == id);
        }
    }
}
