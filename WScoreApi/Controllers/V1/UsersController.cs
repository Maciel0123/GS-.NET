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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        public ActionResult<object> ListarTodos()
        {
            var lista = _service.ListarTodos();

            return Ok(new
            {
                data = lista,
                links = HateoasLinkBuilder.BuildPaginatedLinks(
                    Request, 1, lista.Count, 1, "users"
                )
            });
        }

        [HttpGet("paginado")]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
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
                    "users/paginado"
                )
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<object> ObterPorId(Guid id)
        {
            var user = _service.ObterPorId(id);
            if (user == null) return NotFound();

            return Ok(new
            {
                data = user,
                links = HateoasLinkBuilder.ResourceLinks(Request, "users", id.ToString())
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
        public ActionResult<User> Criar(User user)
        {
            var criado = _service.Criar(user);

            return CreatedAtAction(
                nameof(ObterPorId),
                new { id = criado.Id, version = "1" },
                new
                {
                    data = criado,
                    links = HateoasLinkBuilder.ResourceLinks(Request, "users", criado.Id.ToString())
                }
            );
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
