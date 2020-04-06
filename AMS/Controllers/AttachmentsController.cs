using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.Data;
using AMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AMS.Controllers
{
    [Authorize]
    public class AttachmentsController : Controller
    {
        private readonly ILogger<AttachmentsController> logger;
        private readonly AmsContext context;
        private readonly IUserService userService;

        public AttachmentsController(ILogger<AttachmentsController> logger, AmsContext context, IUserService userService)
        {
            this.logger = logger;
            this.context = context;
            this.userService = userService;
        }

        public async Task<IActionResult> AddAttachment(int ticketId)
        {
            var ticket = await context.Tickets
                .Where(x => x.Id == ticketId)
                .FirstOrDefaultAsync();

            
            return View();
        }
    }
}