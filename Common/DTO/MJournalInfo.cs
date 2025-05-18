using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public record MJournalInfo(long EventId, DateTime TimeStamp, int UserId, string PartnerCode);
}
