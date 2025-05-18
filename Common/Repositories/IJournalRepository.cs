using Common.DTO;
using Common.Models;
using Microsoft.AspNetCore.Http;

namespace Common.Repositories
{
    public interface IJournalRepository
    {
        Task<Journal> LogExceptionAsync(HttpContext context, Exception exception);
        Task<List<Journal>> QueryAsync(int skip, int take, VJournalFilter filter);
        Task<Journal?> GetByIdAsync(long id);
    }
}
