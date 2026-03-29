using Microsoft.AspNetCore.Mvc;
using ModuloCadastro.DTO.API;
using ModuloCadastro.Service;

namespace API_simples.Controllers
{
    [ApiController]
    [Route(template: "v1")]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly UsuarioService _service;

        public UsuarioController(UsuarioService service, ILogger<UsuarioController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetUsuarios")]
        public ActionResult<IEnumerable<UsuarioDTO>> GetList()
        {
            _logger.LogInformation("Buscando usuários");

            try
            {
                return Ok(_service.GetList().Select(x => new UsuarioDTO { Id = x.Id, Nome = x.Nome }).ToList());

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar usuários");
                return StatusCode(500, "Erro interno");
            }
        }
    }
}
