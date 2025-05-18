using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class NodeClosure
    {
        public long AncestorId { get; set; }
        public long DescendantId { get; set; }
        public int Depth { get; set; }
    }
}
