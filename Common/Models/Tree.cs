using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Tree
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<Node> Nodes { get; set; } = new List<Node>();
    }
}
