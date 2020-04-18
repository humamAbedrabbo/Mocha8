using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AMS.Data;
using AMS.Models;
using AMS.Services;
using AMS.ViewModels.BankApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AMS.Extensions;

namespace AMS.Controllers
{
    [Authorize]
    public class BankAppController : Controller
    {
        private readonly ILogger<BankAppController> logger;
        private readonly AmsContext context;
        private readonly IUserService userService;
        private readonly ICodeGenerator codeGenerator;
        private readonly IWebHostEnvironment env;

        public BankAppController
            (
            ILogger<BankAppController> logger,
            AmsContext context,
            IUserService userService,
            ICodeGenerator codeGenerator,
            IWebHostEnvironment env
            )
        {
            this.logger = logger;
            this.context = context;
            this.userService = userService;
            this.codeGenerator = codeGenerator;
            this.env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<IncomingPost> posts = new List<IncomingPost>();

            var tickets = await context.Tickets
                    .Include(x => x.Client)
                    .Include(x => x.TicketType)
                    .Include(x => x.Values).ThenInclude(x => x.Field)
                    .Include(x => x.Attachements)
                    .Include(x => x.Assignments).ThenInclude(x => x.User)
                    .Include(x => x.TodoTasks).ThenInclude(x => x.TodoTaskType)
                    .Include(x => x.TodoTasks).ThenInclude(x => x.Assignments).ThenInclude(x => x.User)
                    .Where(x => x.TenantId == userService.GetUserTenantId() && x.TicketTypeId == 1)
                    .ToListAsync();
            foreach (var ticket in tickets)
            {
                IncomingPost post = new IncomingPost();
                post.Id = ticket.Id;
                post.Subject = ticket.Summary;
                post.Translation = ticket.Description;
                post.SenderId = ticket.ClientId ?? 3;
                post.From = ticket.Client?.Name;
                post.ReceivedOn = ticket.StartDate;
                post.LetterReference = ticket.FieldValues["Ref"].Value;
                post.LetterOutReference = ticket.FieldValues["OutRef"].Value;
                post.IncomingPostType = (IncomingPostType)int.Parse(ticket.FieldValues["LType"].Value ?? "0");
                post.Status = (PostStatus)int.Parse(ticket.FieldValues["PostStatus"].Value ?? "0");
                post.DeadlineType = (DeadlineType)int.Parse(ticket.FieldValues["DeadlineType"].Value ?? "0");
                post.DayNo = ticket.FieldValues["DayNo"].NumberValue ?? 0;
                post.MonthNo = ticket.FieldValues["MonthNo"].NumberValue ?? 0;
                post.AttentionToId = ticket.FieldValues["ToId"].Value;
                post.CCToId = ticket.FieldValues["CCId"].Value;
                post.CEOComments = ticket.FieldValues["CEO Comments"].Value;
                post.Deadline = ticket.DueDate;
                post.Attachments = ticket.Attachements.ToList();
                post.CompletionDate = ticket.CompletionDate;
                post.LetterSent = ticket.FieldValues["LetterSent"].BooleanValue;
                post.LetterSentOn = ticket.FieldValues["LetterSentOn"].DateValue;
                post.LetterDelivered = ticket.FieldValues["LetterDelivered"].BooleanValue;
                post.LetterDeliveredOn = ticket.FieldValues["LetterDeliveredOn"].DateValue;

                post.ResponsiblePeople = ticket.Assignments.Select(x => new ResponsiblePerson(x.User.DisplayName)).ToList();

                //foreach (var task in ticket.TodoTasks)
                //{
                //    var t = new PostTask();
                //    t.Id = task.Id;
                //    t.IsDone = (task.Status == WorkStatus.Completed);
                //    t.ResponsilePerson = task.Assignments.Select(x => x.User.DisplayName).First();
                //    t.UserName = task.Assignments.Select(x => x.User.UserName).First();
                //    t.Comments = task.Description;
                //    t.SortId = 100;
                //    post.PostTasks.Add(t);
                //}
                posts.Add(post);
            }

            return View(posts);
        }

        public async Task<IActionResult> Post(int? id)
        {
            var user = await userService.GetCurrentUserAsync();

            IncomingPost post = new IncomingPost();
            if(id.HasValue)
            {
                var ticket = await context.Tickets
                    .Include(x => x.Client)
                    .Include(x => x.TicketType)
                    .Include(x => x.Values).ThenInclude(x => x.Field)
                    .Include(x => x.Attachements)
                    .Include(x => x.Assignments).ThenInclude(x => x.User)
                    .Include(x => x.TodoTasks).ThenInclude(x => x.TodoTaskType)
                    .Include(x => x.TodoTasks).ThenInclude(x => x.Assignments).ThenInclude(x => x.User)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                post.Id = ticket.Id;
                post.Subject = ticket.Summary;
                post.Translation = ticket.Description;
                post.SenderId = ticket.ClientId ?? 3;
                post.ReceivedOn = ticket.StartDate;
                post.LetterReference = ticket.FieldValues["Ref"].Value;
                post.LetterOutReference = ticket.FieldValues["OutRef"].Value;
                post.IncomingPostType = (IncomingPostType) int.Parse(ticket.FieldValues["LType"].Value ?? "0");
                post.Status = (PostStatus) int.Parse(ticket.FieldValues["PostStatus"].Value ?? "0");
                post.DeadlineType = (DeadlineType) int.Parse(ticket.FieldValues["DeadlineType"].Value ?? "0");
                post.DayNo = ticket.FieldValues["DayNo"].NumberValue ?? 0;
                post.MonthNo = ticket.FieldValues["MonthNo"].NumberValue ?? 0;
                post.AttentionToId = ticket.FieldValues["ToId"].Value;
                post.CCToId = ticket.FieldValues["CCId"].Value;
                post.CEOComments = ticket.FieldValues["CEO Comments"].Value;
                post.Deadline = ticket.DueDate;
                post.Attachments = ticket.Attachements.ToList();
                post.CompletionDate = ticket.CompletionDate;
                post.LetterSent = ticket.FieldValues["LetterSent"].BooleanValue;
                post.LetterSentOn = ticket.FieldValues["LetterSentOn"].DateValue;
                post.LetterDelivered = ticket.FieldValues["LetterDelivered"].BooleanValue;
                post.LetterDeliveredOn = ticket.FieldValues["LetterDeliveredOn"].DateValue;

                post.ResponsiblePeople = ticket.Assignments.Select(x => new ResponsiblePerson(x.UserId.ToString())).ToList();

                post.ResponseTask = new ResponseTask();
                if(ticket.TodoTasks.Count > 0)
                {
                    var task = ticket.TodoTasks.First();
                    post.ResponseTask = new ResponseTask();
                    if(task.Status == WorkStatus.Pending)
                    {
                        post.ResponseTask.StartDate = null;
                    }
                    else
                    {
                        post.ResponseTask.StartDate = task.StartDate;
                    }
                    post.ResponseTask.IsLocked = ticket.FieldValues["Checkout"].BooleanValue;
                    post.ResponseTask.CheckOutDate = ticket.FieldValues["CheckoutDate"].DateValue;
                    post.ResponseTask.CheckOutBy = ticket.FieldValues["CheckoutBy"].Value;
                    post.ResponseTask.CheckOutByName = ticket.FieldValues["CheckoutByName"].Value;
                    post.ResponseTask.Id = task.Id;
                    post.ResponseTask.IsDone = (task.Status == WorkStatus.Completed) ;
                    post.ResponseTask.CompletionDate = task.CompletionDate;
                    
                }
            }
            else
            {
                post = new IncomingPost();
            }

            post.Senders = await userService.GetClientsSelectAsync(post.SenderId);
            post.Attentions = await userService.GetCustomListItemsSelectAsync(1, post.AttentionToId);
            post.Users = await userService.GetUsersSelectAsync();
            return View(post);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> PostAssistant(IncomingPost post, string submit = "save")
        {
            var user = await userService.GetCurrentUserAsync();
            var users = await userService.GetUsersAsync();
            var meta = await userService.GetMetaFieldsAsync();

            Ticket ticket;
            if(post.Id == 0)
            {
                ticket = new Ticket();
            }
            else
            {
                ticket = await context.Tickets
                    .Include(x => x.Client)
                    .Include(x => x.TicketType)
                    .Include(x => x.Values).ThenInclude(x => x.Field)
                    .Include(x => x.Attachements)
                    .Include(x => x.Assignments).ThenInclude(x => x.User)
                    .Include(x => x.TodoTasks).ThenInclude(x => x.TodoTaskType)
                    .Include(x => x.TodoTasks).ThenInclude(x => x.Assignments).ThenInclude(x => x.User)
                    .Where(x => x.Id == post.Id)
                    .FirstOrDefaultAsync();
            }

            ticket.TenantId = userService.GetUserTenantId() ?? 1;
            ticket.Summary = post.Subject;
            ticket.Description = post.Translation;
            ticket.StartDate = post.ReceivedOn;
            ticket.TicketTypeId = 1; // IncomingPost
            if (post.Id == 0)
            {
                ticket.CodeNumber = codeGenerator.GetTicketCode(ticket.TenantId).Result;
                ticket.Code = $"P-{ticket.CodeNumber.ToString("D5")}";
            }
            ticket.DueDate = post.Deadline;

            switch(post.DeadlineType)
            {
                case DeadlineType.Monthly:
                    ticket.DueDate = new DateTime(ticket.StartDate.Year, ticket.StartDate.Month + 1, post.DayNo);
                    break;
                case DeadlineType.Quarterly:
                    DateTime d = ticket.StartDate.QuarterStart().AddMonths(post.MonthNo).AddDays(post.DayNo);
                    if(d <= ticket.StartDate)
                    {
                        d = ticket.StartDate.QuarterEnd().AddDays(2).QuarterStart().AddMonths(post.MonthNo).AddDays(post.DayNo);
                    }
                    ticket.DueDate = d;
                    break;
                case DeadlineType.Annual:
                    DateTime d1 = new DateTime(ticket.StartDate.Year, post.MonthNo, post.DayNo);
                    if(d1 <= ticket.StartDate)
                    {
                        d1 = new DateTime(ticket.StartDate.Year + 1, post.MonthNo, post.DayNo);
                    }
                    ticket.DueDate = d1;
                    break;
                case DeadlineType.OnlyOnce:
                default:
                    ticket.DueDate = post.Deadline;
                    break;
            }

            if (post.SenderId != 3)
            {
                ticket.ClientId = post.SenderId;
            }
            else
            {
                ticket.Client = new Client();
                ticket.Client.TenantId = userService.GetUserTenantId() ?? 1;
                ticket.Client.ClientTypeId = 2;
                ticket.Client.Name = post.SenderName;
            }

            if (post.Id == 0)
            {
                // LetterSent
                ticket.Values.Add(new MetaFieldValue
                {
                    FieldId = meta.First(x => x.Name == "LetterSent").Id,
                    BooleanValue = false
                });
                // LetterSentOn
                ticket.Values.Add(new MetaFieldValue
                {
                    FieldId = meta.First(x => x.Name == "LetterSentOn").Id,
                    Value = null
                });
                // LetterDelivered
                ticket.Values.Add(new MetaFieldValue
                {
                    FieldId = meta.First(x => x.Name == "LetterDelivered").Id,
                    BooleanValue = false
                });
                // LetterDeliveredOn
                ticket.Values.Add(new MetaFieldValue
                {
                    FieldId = meta.First(x => x.Name == "LetterDeliveredOn").Id,
                    Value = null
                });
                // Checkout
                ticket.Values.Add(new MetaFieldValue
                {
                    FieldId = meta.First(x => x.Name == "Checkout").Id,
                    BooleanValue = false
                });
                // CheckoutBy
                ticket.Values.Add(new MetaFieldValue
                {
                    FieldId = meta.First(x => x.Name == "CheckoutBy").Id,
                    Value = null
                });
                // CheckoutByName
                ticket.Values.Add(new MetaFieldValue
                {
                    FieldId = meta.First(x => x.Name == "CheckoutByName").Id,
                    Value = null
                });
                // CheckoutDate
                ticket.Values.Add(new MetaFieldValue
                {
                    FieldId = meta.First(x => x.Name == "CheckoutDate").Id,
                    Value = null
                });
                // Letter Type
                ticket.Values.Add(new MetaFieldValue
                {
                    FieldId = meta.First(x => x.Name == "LType").Id,
                    Value = ((int)post.IncomingPostType).ToString()
                });
                // Letter Ref
                ticket.Values.Add(new MetaFieldValue
                {
                    FieldId = meta.First(x => x.Name == "Ref").Id,
                    Value = post.LetterReference
                });
                // Letter OutRef
                ticket.Values.Add(new MetaFieldValue
                {
                    FieldId = meta.First(x => x.Name == "OutRef").Id,
                    Value = post.LetterOutReference
                });
                //Deadline type
                ticket.Values.Add(new MetaFieldValue
                {
                    FieldId = meta.First(x => x.Name == "DeadlineType").Id,
                    Value = ((int)post.DeadlineType).ToString()
                });
                //DayNo
                ticket.Values.Add(new MetaFieldValue
                {
                    FieldId = meta.First(x => x.Name == "DayNo").Id,
                    Value = post.DayNo.ToString()
                });
                //MonthNo
                ticket.Values.Add(new MetaFieldValue
                {
                    FieldId = meta.First(x => x.Name == "MonthNo").Id,
                    Value = post.MonthNo.ToString()
                });
                //ToId
                ticket.Values.Add(new MetaFieldValue
                {
                    FieldId = meta.First(x => x.Name == "ToId").Id,
                    Value = post.AttentionToId
                });
                //CCId
                ticket.Values.Add(new MetaFieldValue
                {
                    FieldId = meta.First(x => x.Name == "CCId").Id,
                    Value = post.CCToId
                });
                //PostStatus
                ticket.Values.Add(new MetaFieldValue
                {
                    FieldId = meta.First(x => x.Name == "PostStatus").Id,
                    Value = ((int)post.Status).ToString()
                });
                //CEO Comments
                ticket.Values.Add(new MetaFieldValue
                {
                    FieldId = meta.First(x => x.Name == "CEO Comments").Id,
                    Value = post.CEOComments
                });
            }
            else
            {
                ticket.FieldValues["CEO Comments"].Value = post.CEOComments;
                ticket.FieldValues["PostStatus"].Value = ((int) post.Status).ToString();
                ticket.FieldValues["LType"].Value = ((int) post.IncomingPostType).ToString();
                ticket.FieldValues["DeadlineType"].Value = ((int) post.DeadlineType).ToString();
                ticket.FieldValues["CCId"].Value = post.CCToId;
                ticket.FieldValues["ToId"].Value = post.AttentionToId;
                ticket.FieldValues["Ref"].Value = post.LetterReference;
                ticket.FieldValues["OutRef"].Value = post.LetterOutReference;
                ticket.FieldValues["DayNo"].Value = post.DayNo.ToString();
                ticket.FieldValues["MonthNo"].Value = post.MonthNo.ToString();
                ticket.FieldValues["CheckoutBy"].Value = post.ResponseTask?.CheckOutBy;
                ticket.FieldValues["CheckoutByName"].Value = post.ResponseTask?.CheckOutByName;
                ticket.FieldValues["CheckoutDate"].DateValue = post.ResponseTask?.CheckOutDate;
                ticket.FieldValues["Checkout"].BooleanValue = post.ResponseTask?.IsLocked ?? false;
                ticket.FieldValues["LetterSent"].BooleanValue = post.LetterSent;
                ticket.FieldValues["LetterSentOn"].DateValue = post.LetterSentOn;
                ticket.FieldValues["LetterDelivered"].BooleanValue = post.LetterDelivered;
                ticket.FieldValues["LetterDeliveredOn"].DateValue = post.LetterDeliveredOn;
            }

            // Change Status
            switch (post.Status)
            {
                case PostStatus.New:
                case PostStatus.InProgress:
                case PostStatus.Incomplete:
                    ticket.Status = WorkStatus.Open;
                    break;
                case PostStatus.Pending:
                    ticket.Status = WorkStatus.Pending;
                    break;
                case PostStatus.Completed:
                    ticket.Status = WorkStatus.Completed;
                    break;
                default:
                    ticket.Status = WorkStatus.Open;
                    break;
            }

            if(post.Id == 0)
            {
                context.Tickets.Add(ticket);
            }
            await context.SaveChangesAsync();


            //if(post.LetterSentOn.HasValue)
            //{
            //    ticket.FieldValues["LetterSent"].BooleanValue = true;
            //    ticket.FieldValues["LetterSentOn"].DateValue = post.LetterSentOn.Value;
            //}

            //if (post.LetterDeliveredOn.HasValue)
            //{
            //    ticket.FieldValues["LetterDelivered"].BooleanValue = true;
            //    ticket.FieldValues["LetterDeliveredOn"].DateValue = post.LetterDeliveredOn.Value;
            //}

            // Upload files
            if (post.Files != null)
            {
                // Upload files
                long size = post.Files.Sum(f => f.Length);
                var filesPath = Path.Combine(env.WebRootPath, "files", ticket.Id.ToString());
                if (!Directory.Exists(filesPath))
                {
                    Directory.CreateDirectory(filesPath);
                }

                foreach (var formFile in post.Files)
                {
                    if (formFile.Length > 0)
                    {
                        var filePath = Path.Combine(filesPath, formFile.FileName);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await formFile.CopyToAsync(stream);
                        }

                        Attachment att = new Attachment();
                        att.FileName = formFile.FileName;
                        att.Title = formFile.FileName;
                        att.ContentType = formFile.ContentType;
                        att.TicketId = ticket.Id;
                        att.Length = formFile.Length;
                        att.Url = $"/files/{ticket.Id}/{formFile.FileName}";
                        att.CreatedBy = user.DisplayName;
                        att.CreatedOn = DateTime.Now;
                        att.Version = 1;
                        context.Attachements.Add(att);

                    }
                }
                await context.SaveChangesAsync();
            }

            if (submit == "Submit")
            {
                ticket.Status = WorkStatus.Pending;
                ticket.FieldValues["PostStatus"].Value = ((int)PostStatus.Pending).ToString();
                var n = new Notification();
                n.Message = $"New letter {post.LetterReference}";
                n.EntityId = ticket.Id;
                n.NotificationType = NotificationType.IncomingLetter;
                n.UserId = users.First(x => x.NormalizedUserName == "CEO").Id;
                n.DateCreated = DateTime.Now;
                context.Add(n);
                await context.SaveChangesAsync();
            }

            if(submit == "Create Outgoing Letter")
            {
                ticket.Status = WorkStatus.Pending;
                ticket.FieldValues["PostStatus"].Value = ((int)PostStatus.Incomplete).ToString();
                TodoTask outTask = new TodoTask();
                outTask.TenantId = userService.GetUserTenantId() ?? 1;
                outTask.Summary = $"Outgoing";
                outTask.StartDate = DateTime.Today;
                outTask.DueDate = ticket.DueDate;
                outTask.Assignments.Add(new Assignment { UserId = users.First(x => x.NormalizedUserName == "ASSISTANT").Id });
                ticket.TodoTasks.Add(outTask);
                await context.SaveChangesAsync();
            }

            if (submit == "Complete")
            {
                ticket.CompletionDate = DateTime.Today;
                ticket.Status = WorkStatus.Completed;
                ticket.FieldValues["PostStatus"].Value = ((int)PostStatus.Completed).ToString();
                var task = ticket.TodoTasks.FirstOrDefault();
                if(task != null)
                {
                    task.Status = WorkStatus.Completed;
                    task.CompletionDate = DateTime.Now;
                }
                var n = new Notification();
                n.Message = $"{post.LetterReference} letter has been closed successfully";
                n.EntityId = ticket.Id;
                n.NotificationType = NotificationType.IncomingLetter;
                n.UserId = users.First(x => x.NormalizedUserName == "CEO").Id;
                n.DateCreated = DateTime.Now;
                context.Add(n);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Post), new { id = ticket.Id });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> PostCeo(IncomingPost post, string submit = "save")
        {
            var user = await userService.GetCurrentUserAsync();
            var users = await userService.GetUsersAsync();
            var meta = await userService.GetMetaFieldsAsync();

            Ticket ticket = await context.Tickets
                    .Include(x => x.Client)
                    .Include(x => x.TicketType)
                    .Include(x => x.Values).ThenInclude(x => x.Field)
                    .Include(x => x.Attachements)
                    .Include(x => x.Assignments).ThenInclude(x => x.User)
                    .Include(x => x.TodoTasks).ThenInclude(x => x.TodoTaskType)
                    .Include(x => x.TodoTasks).ThenInclude(x => x.Assignments).ThenInclude(x => x.User)
                    .Where(x => x.Id == post.Id)
                    .FirstOrDefaultAsync();

            ticket.FieldValues["CEO Comments"].Value = post.CEOComments;

            //ticket.Assignments.RemoveAll(x => !post.ResponsiblePeople.Contains(x.User.DisplayName));
            foreach (var person in post.ResponsiblePeople.Select(x => x.Value))
            {
                if(!ticket.Assignments.Any(x => x.UserId.ToString() == person))
                {
                    ticket.Assignments.Add(new Assignment { UserId = users.First(x => x.Id.ToString() == person).Id });
                }
            }

            await context.SaveChangesAsync();

            if (submit == "Submit")
            {
                var path = Path.Combine(env.WebRootPath, "files", "Template.docx");
                var path2 = Path.Combine(env.WebRootPath, "files", ticket.Id.ToString());
                var letterResponsePath = Path.Combine(path2, "LetterResponse.docx");

                if (!Directory.Exists(path2))
                {
                    Directory.CreateDirectory(path2);
                }

                if(!System.IO.File.Exists(letterResponsePath))
                {
                    System.IO.File.Copy(path, letterResponsePath);
                }



                var taskAssignments = ticket.TodoTasks.SelectMany(x => x.Assignments);
                var task = new TodoTask();
                task.TicketId = ticket.Id;
                task.TodoTaskTypeId = 1;
                task.Summary = ticket.Summary;
                task.StartDate = DateTime.Today;
                task.DueDate = ticket.DueDate;
                task.Status = WorkStatus.Pending;
                foreach (var a in ticket.Assignments)
                {
                    var n = new Notification();
                    n.Message = $"You are assigned to {ticket.FieldValues["Ref"].Value}";
                    n.EntityId = ticket.Id;
                    n.NotificationType = NotificationType.IncomingLetter;
                    n.UserId = a.UserId.Value;
                    n.DateCreated = DateTime.Now;
                    context.Add(n);
                }
                ticket.TodoTasks.Add(task);
                ticket.Status = WorkStatus.Open;
                ticket.FieldValues["PostStatus"].Value = ((int)PostStatus.InProgress).ToString();
                ticket.FieldValues["OutRef"].Value = ticket.FieldValues["Ref"].Value + "/OUT";
                {
                    var n = new Notification();
                    n.Message = $"CEO has assigned {ticket.FieldValues["Ref"].Value} to managers";
                    n.EntityId = ticket.Id;
                    n.NotificationType = NotificationType.IncomingLetter;
                    n.UserId = users.First(x => x.NormalizedUserName == "ASSISTANT").Id;
                    n.DateCreated = DateTime.Now;
                    context.Add(n);
                }
                await context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Post), new { id = ticket.Id });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> PostProgress(int ticketId, int taskId, string userName, List<IFormFile> workFiles, IFormFile mainFile, string submit = "save")
        {
            var user = await userService.GetCurrentUserAsync();
            var users = await userService.GetUsersAsync();
            var meta = await userService.GetMetaFieldsAsync();

            Ticket ticket = await context.Tickets
                    .Include(x => x.Client)
                    .Include(x => x.TicketType)
                    .Include(x => x.Values).ThenInclude(x => x.Field)
                    .Include(x => x.Attachements)
                    .Include(x => x.Assignments).ThenInclude(x => x.User)
                    .Include(x => x.TodoTasks)
                    .Include(x => x.TodoTasks).ThenInclude(x => x.TodoTaskType)
                    .Include(x => x.TodoTasks).ThenInclude(x => x.Assignments).ThenInclude(x => x.User)
                    .Where(x => x.Id == ticketId)
                    .FirstOrDefaultAsync();

            var task = ticket.TodoTasks.FirstOrDefault();
            if(task != null)
            {
                // Start Response
                if(submit == "Start Response" && task.Status == WorkStatus.Pending)
                {
                    task.Status = WorkStatus.Open;
                    ticket.FieldValues["Checkout"].BooleanValue = true;
                    ticket.FieldValues["CheckoutBy"].Value = user.UserName;
                    ticket.FieldValues["CheckoutByName"].Value = user.DisplayName;
                    ticket.FieldValues["CheckoutDate"].DateValue = DateTime.Now;
                    
                }

                // Edit Response
                if(submit == "Checkout Response")
                {
                    ticket.FieldValues["Checkout"].BooleanValue = true;
                    ticket.FieldValues["CheckoutBy"].Value = user.UserName;
                    ticket.FieldValues["CheckoutByName"].Value = user.DisplayName;
                    ticket.FieldValues["CheckoutDate"].DateValue = DateTime.Now;
                }

                // Submit Document
                if(submit == "Checkin Response" && User.Identity.Name == ticket.FieldValues["CheckoutBy"].Value)
                {
                    var docPath = Path.Combine(env.WebRootPath, "files", ticket.Id.ToString());
                    if(mainFile != null && mainFile.Length > 0)
                    {
                        var filePath = Path.Combine(docPath, "LetterResponse.docx");

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await mainFile.CopyToAsync(stream);
                        }

                        ticket.FieldValues["Checkout"].BooleanValue = false;
                        ticket.FieldValues["CheckoutBy"].Value = null;
                        ticket.FieldValues["CheckoutByName"].Value = null;
                        ticket.FieldValues["CheckoutDate"].Value = null;
                    }
                }
            }

            await context.SaveChangesAsync();

            // Upload files
            if (workFiles != null)
            {
                // Upload files
                long size = workFiles.Sum(f => f.Length);
                var filesPath = Path.Combine(env.WebRootPath, "files", ticket.Id.ToString());
                if (!Directory.Exists(filesPath))
                {
                    Directory.CreateDirectory(filesPath);
                }

                foreach (var formFile in workFiles)
                {
                    if (formFile.Length > 0)
                    {
                        var filePath = Path.Combine(filesPath, formFile.FileName);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await formFile.CopyToAsync(stream);
                        }

                        Attachment att = new Attachment();
                        att.FileName = formFile.FileName;
                        att.Title = formFile.FileName;
                        att.ContentType = formFile.ContentType;
                        att.TicketId = ticket.Id;
                        att.Length = formFile.Length;
                        att.Url = $"/files/{ticket.Id}/{formFile.FileName}";
                        att.CreatedBy = user.DisplayName;
                        att.CreatedOn = DateTime.Now;
                        att.Version = 1;
                        context.Attachements.Add(att);

                    }
                }
                await context.SaveChangesAsync();
            }

            //if (submit == "Submit")
            //{
            //    task.Status = WorkStatus.Completed;
            //    {
            //        var n = new Notification();
            //        n.Message = $"{user.DisplayName} has updated {ticket.FieldValues["Ref"].Value}";
            //        n.EntityId = ticket.Id;
            //        n.NotificationType = NotificationType.IncomingLetter;
            //        n.UserId = users.First(x => x.NormalizedUserName == "ASSISTANT").Id;
            //        n.DateCreated = DateTime.Now;
            //        context.Add(n);
            //    }
            //    await context.SaveChangesAsync();
            //}

            return RedirectToAction(nameof(Post), new { id = ticket.Id });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> PostMail(int ticketId, string LetterOutReference, DateTime? LetterSentOn, DateTime? LetterDeliveredOn)
        {
            var user = await userService.GetCurrentUserAsync();
            var users = await userService.GetUsersAsync();
            var meta = await userService.GetMetaFieldsAsync();

            Ticket ticket = await context.Tickets
                    .Include(x => x.Client)
                    .Include(x => x.TicketType)
                    .Include(x => x.Values).ThenInclude(x => x.Field)
                    .Include(x => x.Attachements)
                    .Include(x => x.Assignments).ThenInclude(x => x.User)
                    .Include(x => x.TodoTasks)
                    .Include(x => x.TodoTasks).ThenInclude(x => x.TodoTaskType)
                    .Include(x => x.TodoTasks).ThenInclude(x => x.Assignments).ThenInclude(x => x.User)
                    .Where(x => x.Id == ticketId)
                    .FirstOrDefaultAsync();

            if(LetterSentOn.HasValue)
            {
                ticket.FieldValues["LetterSent"].BooleanValue = true;
                ticket.FieldValues["LetterSentOn"].DateValue = LetterSentOn;
            }

            if(LetterDeliveredOn.HasValue)
            {
                ticket.FieldValues["LetterDelivered"].BooleanValue = true;
                ticket.FieldValues["LetterDeliveredOn"].DateValue = LetterSentOn;
            }

            if(!string.IsNullOrEmpty(LetterOutReference))
                ticket.FieldValues["OutRef"].Value = LetterOutReference;

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Post), new { id = ticket.Id });
        }
        public IActionResult Show()
        {
            return View();
        }
    }
}