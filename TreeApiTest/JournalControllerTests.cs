using AuthService;
using JournalService.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Text.Json;
using TreeApiTest.TestUtility;

namespace TreeApiTest
{
    public class JournalControllerTests : IClassFixture<CustomWebAppFactory<JournalService.Program, JournalDbContext>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebAppFactory<JournalService.Program, JournalDbContext> _factory;

        public JournalControllerTests(CustomWebAppFactory<JournalService.Program, JournalDbContext> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetRange_ReturnsFilteredResults()
        {
            var scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<JournalDbContext>();
                db.JournalEntries.Add(new Common.Models.Journal
                {
                    EventId = 1,
                    TimeStamp = DateTime.UtcNow,
                    UserId = 1,
                    PartnerCode = "TEST",
                    QueryParams = "{}",
                    BodyParams = "{}",
                    StackTrace = "Test"
                });
                db.SaveChanges();
            }

            var response = await _client.GetAsync(
                "/api/user/journal/getRange?skip=0&take=10&userId=1&partnerCode=TEST&from=2025-01-01T00:00:00&to=2025-12-31T23:59:59");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            Assert.True(json.TryGetProperty("items", out _));
        }

        [Fact]
        public async Task GetSingle_ReturnsNotFound_IfMissing()
        {
            var response = await _client.GetAsync("/api/user/journal/getSingle?id=9999");

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
