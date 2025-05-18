using AuthService.Data;
using Common.Models;

namespace AuthService.Repositories
{
    public class RefreshTokenRepository
    {
        private readonly AuthDbContext _context;

        public RefreshTokenRepository(AuthDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(int userId,  string token, DateTime expiresAt)
        {
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = token,
                ExpiresAt = expiresAt,
                IsRevoked = false
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
        }
    }
}
