using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace TreeService.Data
{
    public class TreeContext : DbContext
    {
        public TreeContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Tree> Trees => Set<Tree>();
        public DbSet<Node> Nodes => Set<Node>();
        public DbSet<NodeClosure> NodeClosures => Set<NodeClosure>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Node>()
                .HasIndex(x => new
                {
                    x.TreeId,
                    x.ParentNodeId,
                    x.Name
                })
                .IsUnique()
                .HasDatabaseName("IX_Nodes_Tree_Parent_Name");

            modelBuilder.Entity<NodeClosure>()
                .HasKey(x => new
                {
                    x.AncestorId,
                    x.DescendantId
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
