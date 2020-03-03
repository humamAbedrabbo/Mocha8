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
using AMS.ViewModels;

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

        public async Task<JsonResult> GetMonthlyTicketsData()
        {
            ChartModel model = new ChartModel();
            int currentMonth = DateTime.Today.Month;
            int currentYear = DateTime.Today.Year;
            var openTicketsDS = new ChartModel.DataSetLabel() { Label = "Open or Pending" };
            var completedTicketsDS = new ChartModel.DataSetLabel() { Label = "Closed" };

            var tickets = await userService.GetTicketsAsync();

            for (int i = 1; i <= 12; i++)
            {
                var start = new DateTime(currentYear, i, 1);
                model.Labels.Add(start.ToString("MMM"));
                var countOpen = tickets.Where(x =>
                    (x.Status == WorkStatus.Open || x.Status == WorkStatus.Pending)
                    && (x.StartDate.Month == i && x.StartDate.Year == currentYear)
                ).Count();
                openTicketsDS.Data.Add(countOpen);
                var countClosed = tickets.Where(x =>
                    (x.Status == WorkStatus.Completed)
                    && (x.StartDate.Month == i && x.StartDate.Year == currentYear)

                ).Count();
                completedTicketsDS.Data.Add(countClosed);
            }

            model.DataSetLabels.Add(openTicketsDS);
            model.DataSetLabels.Add(completedTicketsDS);

            return Json(model);
        }

        public async Task<JsonResult> GetTicketsStatusData()
        {
            ChartModel model = new ChartModel();
            int currentMonth = DateTime.Today.Month;
            int currentYear = DateTime.Today.Year;
            var ticketStatusDS = new ChartModel.DataSetLabel();

            var tickets = await userService.GetTicketsAsync();
            model.Labels.Add("Open");
            model.Labels.Add("Closed");
            model.Labels.Add("Pending");
            var countOpen = tickets.Where(x =>
                    (x.Status == WorkStatus.Open)
                    && (x.StartDate.Year == currentYear)
                ).Count();
            var countClosed = tickets.Where(x =>
                (x.Status == WorkStatus.Completed)
                && (x.StartDate.Year == currentYear)
            ).Count();
            var countPending = tickets.Where(x =>
                (x.Status == WorkStatus.Completed)
                && (x.StartDate.Year == currentYear)
            ).Count();
            ticketStatusDS.Data.Add(countOpen);
            ticketStatusDS.Data.Add(countClosed);
            ticketStatusDS.Data.Add(countPending);
            model.DataSetLabels.Add(ticketStatusDS);

            return Json(model);
        }

        public async Task<JsonResult> GetTasksStatusData()
        {
            ChartModel model = new ChartModel();
            int currentMonth = DateTime.Today.Month;
            int currentYear = DateTime.Today.Year;
            var tasksStatusDS = new ChartModel.DataSetLabel();

            var tasks = await userService.GetTodoTasksAsync();
            model.Labels.Add("Open");
            model.Labels.Add("Closed");
            model.Labels.Add("Pending");
            var countOpen = tasks.Where(x =>
                    (x.Status == WorkStatus.Open)
                    && (x.StartDate.Year == currentYear)
                ).Count();
            var countClosed = tasks.Where(x =>
                (x.Status == WorkStatus.Completed)
                && (x.StartDate.Year == currentYear)
            ).Count();
            var countPending = tasks.Where(x =>
                (x.Status == WorkStatus.Completed)
                && (x.StartDate.Year == currentYear)
            ).Count();
            tasksStatusDS.Data.Add(countOpen);
            tasksStatusDS.Data.Add(countClosed);
            tasksStatusDS.Data.Add(countPending);
            model.DataSetLabels.Add(tasksStatusDS);

            return Json(model);
        }

        public async Task<JsonResult> GetLocationTicketsData()
        {
            ChartModel model = new ChartModel();
            int currentMonth = DateTime.Today.Month;
            int currentYear = DateTime.Today.Year;
            var openTicketsDS = new ChartModel.DataSetLabel() { Label = "Open or Pending" };
            var completedTicketsDS = new ChartModel.DataSetLabel() { Label = "Closed" };

            var tickets = await userService.GetTicketsAsync();
            var locs = tickets
                .Where(x => x.LocationId.HasValue)
                .Select(x => x.Location).Distinct().ToList().OrderBy(x => x.Id);

            foreach (var loc in locs)
            {
                model.Labels.Add(loc.Name);
                var countOpen = tickets.Where(x =>
                    (x.Status == WorkStatus.Open || x.Status == WorkStatus.Pending)
                    && (x.LocationId == loc.Id)
                    && (x.StartDate.Year == DateTime.Today.Year)
                ).Count();
                openTicketsDS.Data.Add(countOpen);
                var countClosed = tickets.Where(x =>
                    (x.Status == WorkStatus.Completed)
                    && (x.LocationId == loc.Id)
                    && (x.StartDate.Year == DateTime.Today.Year)

                ).Count();
                completedTicketsDS.Data.Add(countClosed);
            }

            model.DataSetLabels.Add(openTicketsDS);
            model.DataSetLabels.Add(completedTicketsDS);

            return Json(model);
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
