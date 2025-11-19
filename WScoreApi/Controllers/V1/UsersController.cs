using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WScoreApi.DTOs;
using WScoreApi.Helpers;
using WScoreDomain.Common;
using WScoreInfrastructure.Data;

namespace WScoreApi.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/users")]
    [ApiVersion(1.0)]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET /api/v1/users?page=1&pageSize=10
        [HttpGet(Name = "GetUsers")]
        public async Task<ActionResult<WScoreApi.DTOs.PagedResponse<UserReadDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var totalItems = await _context.Users.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var items = await _context.Users
                .OrderBy(u => u.Nome)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserReadDto(u.Id, u.Nome, u.Email))
                .ToListAsync();

            var links = HateoasLinkBuilder
                .BuildPaginatedLinks(Request, page, pageSize, totalPages, route: "users")
                .ToList();

            var resp = new WScoreApi.DTOs.PagedResponse<UserReadDto>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Links = links
            };

            return Ok(resp);
        }

        // GET /api/v1/users/{id}
        [HttpGet("{id:guid}", Name = "GetUserById")]
        public async Task<ActionResult<UserReadDto>> GetById(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            return Ok(new UserReadDto(user.Id, user.Nome, user.Email));
        }

        // POST /api/v1/users
        [HttpPost]
        public async Task<ActionResult<UserReadDto>> Create([FromBody] UserCreateDto dto)
        {
            var entity = new WScoreDomain.Entities.User
            {
                Id = Guid.NewGuid(),
                Nome = dto.Nome,
                Email = dto.Email
            };

            _context.Users.Add(entity);
            await _context.SaveChangesAsync();

            var read = new UserReadDto(entity.Id, entity.Nome, entity.Email);

            return CreatedAtRoute("GetUserById", new { id = entity.Id, version = "1.0" }, read);
        }

        // PUT /api/v1/users/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.Nome = dto.Nome;
            user.Email = dto.Email;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE /api/v1/users/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
