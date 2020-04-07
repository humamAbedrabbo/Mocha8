using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.Utils
{
    public static class HashTag
    {
        public static string ReplaceTicketHashTags(string input)
        {
            if (input == null)
                return null;

            var list = input.Split(" ").Where(x => x.StartsWith("#")).Distinct();
            foreach(var s in list)
            {
                input = input.Replace(s, $"<a href='/Tickets/details?code={s.Substring(1)}'>{s}</a>");
            }

            return input;
        }
    }
}
