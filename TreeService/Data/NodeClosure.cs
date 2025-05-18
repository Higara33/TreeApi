namespace TreeService.Data
{
    public class NodeClosure
    {
        public long AncestorId { get; set; }
        public long DescendantId { get; set; }
        public int Depth { get; set; }
    }
}
