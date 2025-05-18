using AuthService.Services;
using Microsoft.Extensions.Configuration;

namespace TreeApiTest
{
    public class TokenServiceTests
    {
        private readonly IConfiguration _configuration;

        public TokenServiceTests()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"Jwt:Secret", "YourSuperSecretKey_More_32_symbols_1234512345"},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"},
                {"Jwt:AccessTokenExpirationMinutes", "1"},
                {"Jwt:RefreshTokenExpirationDays", "1"}
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Fact]
        public void GenerateTokens_ShouldReturnValidTokens()
        {
            var svc = new TokenService(_configuration);
            var result = svc.GenerateTokens(42);

            Assert.NotNull(result.AccessToken);
            Assert.NotNull(result.RefreshToken);
            Assert.True(result.RefreshTokenExpires > DateTime.UtcNow);
        }
    }
}