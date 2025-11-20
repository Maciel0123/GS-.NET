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
    public class CheckinsController : ControllerBase
    {
        private readonly ICheckinService _service;

        public CheckinsController(ICheckinService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Checkin>), StatusCodes.Status200OK)]
        public ActionResult<List<Checkin>> ListarTodos()
            => Ok(_service.ListarTodos());

        [HttpGet("paginado")]
        [ProducesResponseType(typeof(List<Checkin>), StatusCodes.Status200OK)]
        public ActionResult<List<Checkin>> ListarPaginado(int page = 1, int pageSize = 10)
            => Ok(_service.ListarPaginado(page, pageSize));

        [HttpGet("usuario/{userId}")]
        [ProducesResponseType(typeof(List<Checkin>), StatusCodes.Status200OK)]
        public ActionResult<List<Checkin>> ListarPorUsuario(Guid userId)
            => Ok(_service.ListarPorUsuario(userId));

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<object> ObterPorId(Guid id)
        {
            var checkin = _service.ObterPorId(id);
            if (checkin == null) return NotFound();

            var self = Url.Action(nameof(ObterPorId), new { id, version = "1" });
            var update = Url.Action(nameof(Atualizar), new { version = "1" });
            var delete = Url.Action(nameof(Remover), new { id, version = "1" });

            return Ok(new
            {
                data = checkin,
                links = new[]
                {
                    new { rel = "self",   href = self,   method = "GET" },
                    new { rel = "update", href = update, method = "PUT" },
                    new { rel = "delete", href = delete, method = "DELETE" }
                }
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(Checkin), StatusCodes.Status201Created)]
        public ActionResult<Checkin> Criar(Checkin checkin)
        {
            var criado = _service.Criar(checkin);
            return CreatedAtAction(nameof(ObterPorId), new { id = criado.Id, version = "1" }, criado);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Atualizar(Checkin checkin)
        {
            var atualizado = _service.Atualizar(checkin);
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
