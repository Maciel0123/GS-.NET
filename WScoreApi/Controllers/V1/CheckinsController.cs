using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp.Versioning;
using WScoreInfrastructure.Data;
using WScoreDomain.Entities;
using WScoreApi.Helpers;
using WScoreDomain.Common;

namespace WScoreApi.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/checkins")]
    [ApiVersion(1.0)]
    public class CheckinsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CheckinsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<ResourceWrapper<PagedResponse<Checkin>>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _context.Checkins.Include(c => c.User).AsNoTracking();
            var total = await query.CountAsync();

            var data = await query
                .OrderByDescending(c => c.DataCheckin)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var resp = new PagedResponse<Checkin>
            {
                Items = data,
                Page = page,
                PageSize = pageSize,
                TotalItems = total
            };

            var links = HateoasLinkBuilder.BuildPaginatedLinks(Request, page, pageSize, resp.TotalPages, "checkins").ToList();

            return Ok(new ResourceWrapper<PagedResponse<Checkin>> { Data = resp, Links = links });
        }

        [HttpGet("{id}", Name = "GetCheckinById")]
        public async Task<ActionResult<ResourceWrapper<Checkin>>> GetById(Guid id)
        {
            var checkin = await _context.Checkins
                .Include(c => c.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (checkin == null) return NotFound();

            var links = new List<LinkDto>
            {
                new("self", $"{Request.Scheme}://{Request.Host}/api/v1/checkins/{id}", "GET"),
                new("list", $"{Request.Scheme}://{Request.Host}/api/v1/checkins?page=1&pageSize=10", "GET")
            };

            return Ok(new ResourceWrapper<Checkin> { Data = checkin, Links = links });
        }

        [HttpPost]
        public async Task<ActionResult<Checkin>> Create(Checkin checkin)
        {
            // calcula score simples (ex: 0–10) * 4 * 10 => 0–400 ⇒ ajuste conforme sua regra
            checkin.Score = (checkin.Humor + checkin.Sono + checkin.Foco + checkin.Energia) * 10;

            // garante referência de usuário existente (evita INSERT indevido de User)
            _context.Attach(new User { Id = checkin.UserId });

            _context.Checkins.Add(checkin);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetCheckinById", new { id = checkin.Id, version = "1.0" }, checkin);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Checkin update)
        {
            var checkin = await _context.Checkins.FindAsync(id);
            if (checkin == null) return NotFound();

            checkin.DataCheckin = update.DataCheckin;
            checkin.Humor = update.Humor;
            checkin.Sono = update.Sono;
            checkin.Foco = update.Foco;
            checkin.Energia = update.Energia;
            checkin.CargaTrabalho = update.CargaTrabalho;

            checkin.Score = (update.Humor + update.Sono + update.Foco + update.Energia) * 10;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var checkin = await _context.Checkins.FindAsync(id);
            if (checkin == null) return NotFound();

            _context.Checkins.Remove(checkin);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
