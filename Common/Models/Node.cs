using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Node
    {
        public long Id { get; set; }
        public long TreeId { get; set; }
        public Tree Tree { get; set; }
        public long? ParentNodeId { get; set; }
        public Node ParentNode { get; set; }
        public string Name { get; set; }
        public ICollection<Node> Children { get; set; } = new List<Node>();
    }
}
