using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using WScoreBusiness;
using WScoreDomain.Entities;
using WScoreApi.Helpers;

namespace WScoreApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class CheckinsController : ControllerBase
    {
        private readonly ICheckinService _service;
        private readonly IUserService _userService;

        public CheckinsController(ICheckinService service, IUserService userService)
        {
            _service = service;
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<List<Checkin>> ListarTodos()
        {
            var lista = _service.ListarTodos();

            return Ok(new
            {
                data = lista,
                links = HateoasLinkBuilder.BuildPaginatedLinks(
                    Request, 1, lista.Count, 1, "checkins"
                )
            });
        }

        [HttpGet("paginado")]
        public ActionResult<object> ListarPaginado(int page = 1, int pageSize = 10)
        {
            var lista = _service.ListarPaginado(page, pageSize);
            int total = _service.ListarTodos().Count;

            return Ok(new
            {
                data = lista,
                links = HateoasLinkBuilder.BuildPaginatedLinks(
                    Request, page, pageSize,
                    (int)Math.Ceiling((double)total / pageSize),
                    "checkins/paginado"
                )
            });
        }

        [HttpGet("usuario/{userId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<Checkin>> ListarPorUsuario(Guid userId)
        {
            var user = _userService.ObterPorId(userId);
            if (user == null)
                return NotFound(new { message = "Usuário não encontrado." });

            var lista = _service.ListarPorUsuario(userId);
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public ActionResult<object> ObterPorId(Guid id)
        {
            var checkin = _service.ObterPorId(id);
            if (checkin == null) return NotFound();

            return Ok(new
            {
                data = checkin,
                links = HateoasLinkBuilder.ResourceLinks(Request, "checkins", id.ToString())
            });
        }

        [HttpPost]
        public ActionResult<Checkin> Criar(Checkin checkin)
        {
            var criado = _service.Criar(checkin);

            return CreatedAtAction(
                nameof(ObterPorId),
                new { id = criado.Id, version = "1" },
                new
                {
                    data = criado,
                    links = HateoasLinkBuilder.ResourceLinks(Request, "checkins", criado.Id.ToString())
                }
            );
        }

        [HttpPut]
        public IActionResult Atualizar(Checkin checkin)
        {
            var atualizado = _service.Atualizar(checkin);
            if (!atualizado) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Remover(Guid id)
        {
            var removido = _service.Remover(id);
            if (!removido) return NotFound();
            return NoContent();
        }
    }
}
