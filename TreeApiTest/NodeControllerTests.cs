using System.Net.Http.Json;
using System.Text.Json;
using TreeApiTest.TestUtility;
using TreeService.Data;

namespace TreeApiTest
{
    public class NodeControllerTests : IClassFixture<CustomWebAppFactory<TreeService.Program, TreeContext>>
    {
        private readonly HttpClient _client;

        public NodeControllerTests(CustomWebAppFactory<TreeService.Program, TreeContext> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task RenameNode_ReturnsUpdatedName()
        {
            var createResp = await _client.PostAsync("/api/user/tree/node/create?treeName=T&parentNodeId=0&nodeName=Old", null);
            createResp.EnsureSuccessStatusCode();
            var created = await createResp.Content.ReadFromJsonAsync<JsonElement>();
            var nodeId = created.GetProperty("id").GetInt64();

            var renameResp = await _client.PatchAsync($"/api/user/tree/node/rename?treeName=T&nodeId={nodeId}&newNodeName=New", null);
            renameResp.EnsureSuccessStatusCode();
            var renamed = await renameResp.Content.ReadFromJsonAsync<JsonElement>();

            Assert.Equal("New", renamed.GetProperty("name").GetString());
        }

        [Fact]
        public async Task DeleteNode_WithoutChildren_Succeeds()
        {
            var createParent = await _client.PostAsync("/api/user/tree/node/create?treeName=T&parentNodeId=0&nodeName=P", null);
            createParent.EnsureSuccessStatusCode();
            var parentId = (await createParent.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("id").GetInt64();

            var createChild = await _client.PostAsync($"/api/user/tree/node/create?treeName=T&parentNodeId={parentId}&nodeName=C", null);
            createChild.EnsureSuccessStatusCode();
            var childId = (await createChild.Content.ReadFromJsonAsync<JsonElement>()).GetProperty("id").GetInt64();

            var deleteParent = await _client.DeleteAsync($"/api/user/tree/node/delete?treeName=T&nodeId={parentId}");
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, deleteParent.StatusCode);

            var deleteChild = await _client.DeleteAsync($"/api/user/tree/node/delete?treeName=T&nodeId={childId}");
            deleteChild.EnsureSuccessStatusCode();

            var deleteParent2 = await _client.DeleteAsync($"/api/user/tree/node/delete?treeName=T&nodeId={parentId}");
            deleteParent2.EnsureSuccessStatusCode();
        }
    }
}
