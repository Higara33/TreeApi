using Common.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace TreeService.Controllers
{
    [ApiController]
    [Route("api/user/tree/node")]
    public class NodeController : ControllerBase
    {
        private readonly ITreeRepository _treeRepository;

        public NodeController(ITreeRepository treeRepository) => _treeRepository = treeRepository;

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromQuery] string treeName, [FromQuery] long parentNodeId, [FromQuery] string nodeName)
        {
            var node = await _treeRepository.CreateNodeAsync(treeName, parentNodeId, nodeName);

            return Ok(new
            {
                id = node.Id
            });
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] string treeName, [FromQuery] long nodeId)
        {
            await _treeRepository.DeleteNodeAsync(treeName, nodeId);

            return Ok();
        }

        [HttpPatch("Rename")]
        public async Task<IActionResult> Rename([FromQuery] string treeName, [FromQuery] long nodeId, [FromQuery] string newNodeName)
        {
            var node = await _treeRepository.RenameNodeAsync(treeName, nodeId, newNodeName);

            return Ok(new
            {
                name = node.Name
            });
        }
    }
}
