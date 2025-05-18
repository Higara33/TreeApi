using Common.DTO;
using Common.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace JournalService.Controllers
{
    [ApiController]
    [Route("api/user/journal")]
    public class JournalController : ControllerBase
    {
        private readonly IJournalRepository _journalRepository;

        public JournalController(IJournalRepository journalRepository)
        {
            _journalRepository = journalRepository;
        }

        [HttpGet("GetRange")]
        public async Task<IActionResult> GetRange(
            [FromQuery] int skip,
            [FromQuery] int take,
            [FromQuery] int? userId = null,
            [FromQuery] string partnerCode = null,
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null)
        {
            var filter = new VJournalFilter(userId, partnerCode, from, to);
            var entries = await _journalRepository.QueryAsync(skip, take, filter);
            var infos = entries.Select(x => new MJournalInfo(x.EventId, x.TimeStamp, x.UserId, x.PartnerCode));

            return Ok(new
            {
                skip,
                count = entries.Count,
                items = infos
            });
        }

        [HttpGet("GetSingle")]
        public async Task<IActionResult> GetSingle([FromQuery] long id)
        {
            var entry = await _journalRepository.GetByIdAsync(id);
            if (entry == null)
                return NotFound();

            var dto = new MJournal(
                entry.EventId,
                entry.TimeStamp,
                entry.UserId,
                entry.PartnerCode,
                entry.QueryParams,
                entry.BodyParams);

            return Ok(dto);
        }
    }
}
