using Common.Models;
using Common.Repositories;
using Microsoft.EntityFrameworkCore;
using TreeService.Data;

namespace TreeService.Repositories
{
    public class TreeRepository : ITreeRepository
    {
        private readonly TreeContext _treeContext;

        public TreeRepository(TreeContext treeContext) => _treeContext = treeContext;

        public async Task<Tree> GetOrCreateTreeAsync(string name)
        {
            var tree = await _treeContext.Trees.Include(x => x.Nodes).FirstOrDefaultAsync(y => y.Name == name);

            if (tree == null)
            {
                tree = new Tree
                {
                    Name = name
                };
                _treeContext.Trees.Add(tree);

                await _treeContext.SaveChangesAsync();
            }

            return tree;
        }

        public async Task<Node> CreateNodeAsync(string treeName, long parentId, string nodeName)
        {
            var tree = await GetOrCreateTreeAsync(treeName);
            var node = new Node
            {
                Name = nodeName,
                TreeId = tree.Id,
                ParentNodeId = parentId
            };
            _treeContext.Nodes.Add(node);

            await _treeContext.SaveChangesAsync();

            return node;
        }

        public async Task<Node> RenameNodeAsync(string treeName, long nodeId, string newName)
        {
            var node = await _treeContext.Nodes.FindAsync(nodeId);
            node.Name = newName;

            await _treeContext.SaveChangesAsync();

            return node;
        }

        public async Task DeleteNodeAsync(string treeName, long nodeId)
        {
            var node = await _treeContext.Nodes.FindAsync(nodeId);
            if (node.Children.Any())
                throw new SecureException("Cannot delete node with children");

            _treeContext.Nodes.Remove(node);
            await _treeContext.SaveChangesAsync();
        }
    }
}
