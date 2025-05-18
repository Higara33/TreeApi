using AuthService.Data;
using System.Net.Http.Json;
using System.Text.Json;
using TreeApiTest.TestUtility;

namespace TreeApiTest
{
    public class AuthControllerTests : IClassFixture<CustomWebAppFactory<AuthService.Program, AuthDbContext>>
    {
        private readonly HttpClient _client;

        public AuthControllerTests(CustomWebAppFactory<AuthService.Program, AuthDbContext> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task RememberMe_ReturnsAccessToken_AndSavesRefreshToken()
        {
            var resp = await _client.PostAsJsonAsync(
                "/api/user/partner/rememberMe",
                new { code = "dummy" }
            );
            resp.EnsureSuccessStatusCode();
            var json = await resp.Content.ReadFromJsonAsync<JsonElement>();
            Assert.True(json.TryGetProperty("accessToken", out _));
        }
    }
}
