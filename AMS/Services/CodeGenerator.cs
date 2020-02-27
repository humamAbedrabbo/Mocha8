using AMS.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Services
{
    public class CodeGenerator : ICodeGenerator
    {
        private readonly AmsContext context;

        public CodeGenerator(AmsContext context)
        {
            this.context = context;
        }

        public async Task<int> GetAssetCode(int tenantId)
        {
            return 1 + await context.Assets.CountAsync(x => x.TenantId == tenantId);
        }

        public async Task<int> GetTicketCode(int tenantId)
        {
            return 1 + await context.Tickets.CountAsync(x => x.TenantId == tenantId);
        }
    }
}
