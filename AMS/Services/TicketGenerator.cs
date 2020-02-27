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
