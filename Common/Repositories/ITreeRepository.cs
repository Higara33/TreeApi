using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Repositories
{
    public interface ITreeRepository
    {
        Task<Tree> GetOrCreateTreeAsync(string name);
        Task<Node> CreateNodeAsync(string treeName, long parentId, string nodeName);
        Task<Node> RenameNodeAsync(string treeName, long nodeId, string newName);
        Task DeleteNodeAsync(string treeName, long nodeId);
    }
}
