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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace AMS.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly AmsContext _context;
        private readonly ILogger<UsersController> logger;
        private readonly IUserService userService;
        private readonly IWebHostEnvironment env;

        public UsersController(ILogger<UsersController> logger, AmsContext context, IUserService userService, IWebHostEnvironment env)
        {
            _context = context;
            this.logger = logger;
            this.userService = userService;
            this.env = env;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await userService.GetUsersAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var amsUser = await _context.Users
                .Include(a => a.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (amsUser == null)
            {
                return NotFound();
            }

            return View(amsUser);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        public void SetViewData(AmsUser amsUser = null)
        {
            ViewData["TenantId"] = userService.GetUserTenantId();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenantId,DisplayName,Company,JobTitle,PictureUrl,UserName,Email,PhoneNumber")] AmsUser amsUser, string password, string confirmPassword, IFormFile file)
        {
            if(string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword) || password != confirmPassword)
            {
                ModelState.AddModelError("password", "Invalid password");
                SetViewData(amsUser);
                return View(amsUser);
            }

            if(file != null && file.Length > 307200)
            {
                ModelState.AddModelError("file", "Maximum file size is 300KB");
                SetViewData(amsUser);
                return View(amsUser);
            }

            if (ModelState.IsValid)
            {

                var hasher = new PasswordHasher<AmsUser>();
                amsUser.NormalizedEmail = amsUser.Email.ToUpper();
                amsUser.NormalizedUserName = amsUser.UserName.ToUpper();
                amsUser.ConcurrencyStamp = Guid.NewGuid().ToString("D");
                amsUser.SecurityStamp = Guid.NewGuid().ToString("D");
                amsUser.PasswordHash = hasher.HashPassword(amsUser, password);
                amsUser.TenantId = userService.GetUserTenantId();

                _context.Add(amsUser);
                await _context.SaveChangesAsync();
                
                // Upload Picture
                if (file != null && file.Length > 0)
                {
                    var fileName = $"{amsUser.Id}{Path.GetExtension(file.FileName)}";
                    var filePath = Path.Combine(env.WebRootPath, "images", "avatars", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                    }

                    amsUser.PictureUrl = $"/images/avatars/{fileName}";
                    await _context.SaveChangesAsync();
                }

                _context.UserClaims.Add(new IdentityUserClaim<int> { UserId = amsUser.Id, ClaimType = "TenantId", ClaimValue = $"{amsUser.TenantId}" });
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            SetViewData(amsUser);
            return View(amsUser);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var amsUser = await _context.Users.FindAsync(id);
            if (amsUser == null)
            {
                return NotFound();
            }
            SetViewData(amsUser);
            return View(amsUser);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TenantId,DisplayName,Company,JobTitle,PictureUrl,Id,UserName,Email,PhoneNumber")] AmsUser amsUser)
        {
            if (id != amsUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(amsUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AmsUserExists(amsUser.Id))
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
            SetViewData(amsUser);
            return View(amsUser);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var amsUser = await _context.Users
                .Include(a => a.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (amsUser == null)
            {
                return NotFound();
            }

            return View(amsUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var amsUser = await _context.Users.FindAsync(id);
            _context.Users.Remove(amsUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AmsUserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
