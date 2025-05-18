using Common.Utilities;
using JournalService.Data;
using JournalService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Moq;

namespace TreeApiTest
{
    public class JournalRepositoryTests
    {
        private readonly JournalDbContext _context;
        private readonly Mock<ITimeProvider> _timeProvider;
        private readonly JournalRepository _repo;

        public JournalRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<JournalDbContext>()
                .UseInMemoryDatabase("JournalTests")
                .Options;
            _context = new JournalDbContext(options);
            _timeProvider = new Mock<ITimeProvider>();
            _timeProvider.Setup(tp => tp.Now).Returns(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
            _repo = new JournalRepository(_context, _timeProvider.Object);
        }

        [Fact]
        public async Task LogExceptionAsync_ShouldPersistJournalEntry()
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues>
            {
                ["partnerCode"] = "TEST_CODE"
            });

            var ex = new Exception("test");

            var entry = await _repo.LogExceptionAsync(httpContext, ex);

            var saved = await _context.JournalEntries.FindAsync(entry.EventId);
            Assert.NotNull(saved);
            Assert.Equal("TEST_CODE", saved.PartnerCode);
            Assert.Equal(_timeProvider.Object.Now.UtcDateTime, saved.TimeStamp);
        }
    }
}
