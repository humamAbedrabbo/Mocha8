using AMS.Data;
using AMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Services
{
    public class TicketGenerator : ITicketGenerator
    {
        private readonly AmsContext _context;

        public TicketGenerator(AmsContext context)
        {
            this._context = context;
        }

        public void RunTicketJob(int id)
        {
            var cg = new CodeGenerator(_context);
            var ticketJob = _context.TicketJobs
                .Include(x => x.TicketType)
                .Include(x => x.AssetType)
                .Include(x => x.Client)
                .Include(x => x.Location)
                .Include(x => x.Owner)
                .Include(x => x.UserGroup)
                .Include(x => x.TicketJobTaskTypes).ThenInclude(x => x.TodoTaskType)
                .FirstOrDefault(x => x.Id == id);
            var ticket = new Ticket
            {
                TicketTypeId = ticketJob.TicketTypeId,
                TenantId = ticketJob.TenantId,
                ClientId = ticketJob.ClientId,
                LocationId = ticketJob.LocationId,
                Summary = ticketJob.Summary,
                DueDate = DateTime.Now.AddDays(2),
            };
            ticket.CodeNumber = _context.Tickets.Count() + 1;
            ticket.Code = $"{ticketJob.TicketType.Code}{ticket.CodeNumber.ToString("D5")}";
            var metaValues = _context.MetaFieldValues
            .Include(x => x.Field)
            .Where(x => x.TicketTypeId == ticket.TicketTypeId)
            .ToList();
            foreach (var value in metaValues)
            {
                ticket.Values.Add(new MetaFieldValue { FieldId = value.FieldId, Value = value.Value });
            }

            ticket.Assignments.Add(new Assignment { RoleName = "Owner", UserId = ticketJob.OwnerId });
            if (ticketJob.UserGroupId.HasValue)
            {
                ticket.Assignments.Add(new Assignment { RoleName = "Assigned To", UserGroupId = ticketJob.UserGroupId });
            }

            var assets = _context.Assets
                .Where(x => (x.TenantId == ticketJob.TenantId)
                    && (x.AssetTypeId == ticketJob.AssetTypeId)
                    && (!ticketJob.ClientId.HasValue || x.ClientId == ticketJob.ClientId)
                    && (x.LocationId == ticketJob.LocationId)
                )
                .Select(x => x.Id).ToList();
            foreach (var a in assets)
            {
                ticket.TicketAssets.Add(new TicketAsset { AssetId = a });
            }

            foreach (var taskId in ticketJob.TicketJobTaskTypes)
            {
                var task = new TodoTask
                {
                    TodoTaskTypeId = taskId.TodoTaskTypeId,
                    Summary = $"{ticket.Code} {taskId.TodoTaskType.Name}",
                    StartDate = ticket.StartDate,
                    EstDuration = taskId.TodoTaskType.DefaultDuration,
                    TenantId = ticket.TenantId,
                    DueDate = ticket.StartDate.AddDays(taskId.TodoTaskType.DefaultDuration)
                };
                if(task.DueDate > ticket.DueDate)
                {
                    task.DueDate = ticket.DueDate;
                }
                ticket.TodoTasks.Add(task);
            }

            _context.Tickets.Add(ticket);
            _context.SaveChanges();
        }

        public void AddTicket(int typeId, string summary, int tenantId)
        {
            var cg = new CodeGenerator(_context);
            var type = _context.TicketTypes.Find(typeId);

            var ticket = new Ticket
            {
                TicketTypeId = typeId,
                TenantId = tenantId,
                Summary = summary,
                DueDate = DateTime.Today.AddDays(2),
            };
            ticket.CodeNumber = cg.GetTicketCode(ticket.TenantId).Result;
            ticket.Code = $"{type.Code}{ticket.CodeNumber.ToString("D5")}";
            var metaValues = _context.MetaFieldValues
                .Include(x => x.Field)
                .Where(x => x.TicketTypeId == type.Id)
                .ToList();
            foreach (var value in metaValues)
            {
                ticket.Values.Add(new MetaFieldValue { FieldId = value.FieldId, Value = value.Value });
            }
            _context.Tickets.Add(ticket);
            _context.SaveChanges();

        }
    }
}
