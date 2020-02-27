using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AMS.Models;
using Microsoft.AspNetCore.Authorization;
using AMS.Services;

namespace AMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService userService;

        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var assets = await userService.GetAssetsAsync();
            var tickets = await userService.GetTicketsAsync();
            ViewData["OpenTickets"] = tickets.Where(x => x.Status == WorkStatus.Open || x.Status == WorkStatus.Pending).Count();
            ViewData["ClosedTickets"] = tickets.Where(x => x.Status == WorkStatus.Completed).Count();
            ViewData["PendingTickets"] = tickets.Where(x => x.Status == WorkStatus.Pending).Count();
            ViewData["OverdueTickets"] = tickets.Where(x => (x.Status == WorkStatus.Pending || x.Status == WorkStatus.Open)
                && DateTime.Now > x.DueDate).Count();
            return View(assets);
        }

        public IActionResult Floors()
        {
            return View();
        }

        public async Task<IActionResult> Calendar()
        {
            var todoTasks = (await userService.GetTodoTasksAsync()).Where(x => x.Status == WorkStatus.Open || x.Status == WorkStatus.Pending);
            return View(todoTasks);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
