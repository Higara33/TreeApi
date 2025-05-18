using Common.Models;
using Common.Repositories;
using Common.Utilities;
using System.Text.Json;
using JournalService.Data;
using Common.DTO;
using Microsoft.EntityFrameworkCore;

namespace JournalService.Repositories
{
    public class JournalRepository : IJournalRepository
    {
        private readonly JournalDbContext _journalDbContext;
        private readonly ITimeProvider _timeProvider;

        public JournalRepository(JournalDbContext journalDbContext, ITimeProvider timeProvider)
        {
            _journalDbContext = journalDbContext;
            _timeProvider = timeProvider;
        }

        public async Task<Journal> LogExceptionAsync(HttpContext context, Exception exception)
        {
            var entry = new Journal
            {
                TimeStamp = _timeProvider.Now.UtcDateTime,
                UserId = context.User.Identity.IsAuthenticated
                ? int.Parse(context.User.Identity.Name)
                : 0,
                PartnerCode = context.Request.Query["partnerCode"],
                QueryParams = JsonSerializer.Serialize(context.Request.Query
                .ToDictionary(
                    k => k.Key,
                    v => v.Value.ToString()
                )),
                BodyParams = await new StreamReader(context.Request.Body).ReadToEndAsync(),
                StackTrace = exception.ToString()
            };

            _journalDbContext.JournalEntries.Add(entry);
            await _journalDbContext.SaveChangesAsync();

            return entry;
        }

        async Task<Journal?> IJournalRepository.GetByIdAsync(long id) => await _journalDbContext.JournalEntries.FindAsync(id);

        async Task<List<Journal>> IJournalRepository.QueryAsync(int skip, int take, VJournalFilter filter)
        {
            var query = _journalDbContext.JournalEntries.AsQueryable();

            if(filter.UserId.HasValue)
                query = query.Where(x => x.UserId == filter.UserId.Value);
            if(!string.IsNullOrEmpty(filter.PartnerCode))
                query = query.Where(x => x.PartnerCode == filter.PartnerCode);
            if(filter.From.HasValue)
                query = query.Where(x => x.TimeStamp >= filter.From.Value);
            if (filter.From.HasValue)
                query = query.Where(x => x.TimeStamp <= filter.From.Value);

            return await query.Skip(skip).Take(take).ToListAsync();
        }
    }
}
