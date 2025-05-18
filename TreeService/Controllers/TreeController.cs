using Common.DTO;
using Common.Models;
using Common.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace TreeService.Controllers
{
    [ApiController]
    [Route("api/user/tree")]
    public class TreeController : ControllerBase
    {
        private readonly ITreeRepository _treeRepository;

        public TreeController(ITreeRepository treeRepository) => _treeRepository = treeRepository;

        [HttpGet("Get")]
        public async Task<IActionResult> Get([FromQuery] string treeName)
        {
            var tree = await _treeRepository.GetOrCreateTreeAsync(treeName);
            var rootNodes = tree.Nodes
                .Where(x => x.ParentNodeId == null)
                .Select(x => new NodeDto(
                x.Id,
                x.Name,
                x.Children.Select(y => ToDto(y)).ToList()))
                .ToList();

            return Ok(new TreeDto(tree.Id, tree.Name, rootNodes));
        }

        private NodeDto ToDto(Node n) => new(n.Id, n.Name, n.Children.Select(c => ToDto(c)).ToList());
    }
}
