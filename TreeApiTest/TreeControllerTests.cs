using Common.DTO;
using System.Net.Http.Json;
using TreeApiTest.TestUtility;
using TreeService.Data;

namespace TreeApiTest
{
    public class TreeControllerTests : IClassFixture<CustomWebAppFactory<TreeService.Program, TreeContext>>
    {
        private readonly HttpClient _client;

        public TreeControllerTests(CustomWebAppFactory<TreeService.Program, TreeContext> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetTree_Returns200AndStructure()
        {
            var resp = await _client.GetAsync("/api/user/tree/get?treeName=X");
            resp.EnsureSuccessStatusCode();
            var dto = await resp.Content.ReadFromJsonAsync<TreeDto>();
            Assert.Equal("X", dto.Name);
        }

        [Fact]
        public async Task CreateNode_ReturnsId()
        {
            var resp = await _client.PostAsync(
                "/api/user/tree/node/create?treeName=X&parentNodeId=0&nodeName=Test",
                null
            );
            resp.EnsureSuccessStatusCode();
            var obj = await resp.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
            Assert.True(obj.TryGetProperty("id", out _));
        }
    }
}
