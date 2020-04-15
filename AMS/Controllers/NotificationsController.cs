using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Data;
using AMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AMS.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly AmsContext context;
        private readonly IUserService userService;

        public NotificationsController(ILogger<NotificationsController> logger,
            AmsContext context,
            IUserService userService)
        {
            this.context = context;
            this.userService = userService;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Goto(int id)
        {
            var notification = await context.Notifications.FindAsync(id);
            if(notification != null)
            {
                notification.IsRead = true;
                await context.SaveChangesAsync();
                if (notification.EntityId.HasValue)
                {
                    if(notification.NotificationType == Models.NotificationType.IncomingLetter)
                    {
                        return RedirectToAction("Post", notification.Url, new { id = notification.EntityId });
                    }
                    else
                        return RedirectToAction("Details", notification.Url, new { id = notification.EntityId });
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}