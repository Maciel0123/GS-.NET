using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using WScoreBusiness;
using WScoreDomain.Entities;

namespace WScoreApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        public ActionResult<List<User>> ListarTodos()
            => Ok(_service.ListarTodos());

        [HttpGet("paginado")]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        public ActionResult<List<User>> ListarPaginado(int page = 1, int pageSize = 10)
            => Ok(_service.ListarPaginado(page, pageSize));

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<object> ObterPorId(Guid id)
        {
            var user = _service.ObterPorId(id);
            if (user == null) return NotFound();

            var self = Url.Action(nameof(ObterPorId), new { id, version = "1" });
            var update = Url.Action(nameof(Atualizar), new { version = "1" });
            var delete = Url.Action(nameof(Remover), new { id, version = "1" });

            return Ok(new
            {
                data = user,
                links = new[]
                {
                    new { rel = "self",   href = self,   method = "GET" },
                    new { rel = "update", href = update, method = "PUT" },
                    new { rel = "delete", href = delete, method = "DELETE" }
                }
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
        public ActionResult<User> Criar(User user)
        {
            var criado = _service.Criar(user);
            return CreatedAtAction(nameof(ObterPorId), new { id = criado.Id, version = "1" }, criado);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Atualizar(User user)
        {
            var atualizado = _service.Atualizar(user);
            if (!atualizado) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Remover(Guid id)
        {
            var removido = _service.Remover(id);
            if (!removido) return NotFound();
            return NoContent();
        }
    }
}
