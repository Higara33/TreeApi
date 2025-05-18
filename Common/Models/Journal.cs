using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Journal
    {
        public long EventId { get; set; }
        public DateTime TimeStamp { get; set; }
        public int UserId { get; set; }
        public string PartnerCode { get; set; }
        public string QueryParams { get; set; }
        public string BodyParams { get; set; }
        public string StackTrace { get; set; }
    }
}
