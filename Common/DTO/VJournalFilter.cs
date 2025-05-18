using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public record VJournalFilter(int? UserId, string PartnerCode, DateTime? From, DateTime? To);
}
