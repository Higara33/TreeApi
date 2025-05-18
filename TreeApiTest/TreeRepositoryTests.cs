using Microsoft.EntityFrameworkCore;                                       
using Common.Models;                         
using TreeService.Data;                      
using TreeService.Repositories;

namespace TreeApiTest
{
    public class TreeRepositoryTests
    {
        private readonly TreeContext _ctx;
        private readonly TreeRepository _repo;

        public TreeRepositoryTests()
        {
            var opts = new DbContextOptionsBuilder<TreeContext>()
                .UseInMemoryDatabase("TreeTests")
                .Options;
            _ctx = new TreeContext(opts);
            _repo = new TreeRepository(_ctx);
        }

        [Fact]
        public async Task CreateAndGetTree_ShouldReturnSameTree()
        {
            var tree = await _repo.GetOrCreateTreeAsync("MyTree");
            Assert.Equal("MyTree", tree.Name);

            var same = await _repo.GetOrCreateTreeAsync("MyTree");
            Assert.Equal(tree.Id, same.Id);
        }

        [Fact]
        public async Task CreateNode_Rename_Delete_WorksCorrectly()
        {
            var tree = await _repo.GetOrCreateTreeAsync("T");
            var node = await _repo.CreateNodeAsync("T", tree.Nodes.FirstOrDefault()?.Id ?? 0, "N1");
            Assert.Equal("N1", node.Name);

            var renamed = await _repo.RenameNodeAsync("T", node.Id, "N2");
            Assert.Equal("N2", renamed.Name);

            // Дочерний удалять без очистки нельзя
            var child = await _repo.CreateNodeAsync("T", renamed.Id, "C");
            await Assert.ThrowsAsync<SecureException>(
                () => _repo.DeleteNodeAsync("T", renamed.Id));

            // Удаляем сначала потомка, потом родителя
            await _repo.DeleteNodeAsync("T", child.Id);
            await _repo.DeleteNodeAsync("T", renamed.Id);
        }
    }
}
