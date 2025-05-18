using AuthService.Data;
using AuthService.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TreeApiTest
{
    public class RefreshTokenRepositoryTests
    {
        private readonly AuthDbContext _ctx;
        private readonly RefreshTokenRepository _repo;

        public RefreshTokenRepositoryTests()
        {
            var opts = new DbContextOptionsBuilder<AuthDbContext>()
                .UseInMemoryDatabase("AuthTests")
                .Options;
            _ctx = new AuthDbContext(opts);
            _repo = new RefreshTokenRepository(_ctx);
        }

        [Fact]
        public async Task SaveAsync_ShouldPersistToken()
        {
            await _repo.SaveAsync(7, "token123", DateTime.UtcNow.AddHours(1));
            var stored = await _ctx.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == 7);
            Assert.NotNull(stored);
            Assert.Equal("token123", stored.Token);
            Assert.False(stored.IsRevoked);
        }
    }
}
