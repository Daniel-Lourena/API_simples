using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ModuloCadastro.Entity.Cadastro.Usuario;
using ModuloCadastro.Service.Cadastro.Usuario;

namespace API_simples.Controllers.Usuario;

[AllowAnonymous]
[ApiController]
[Route("v1/auth")]
public class AuthController : ControllerBase
{
    private readonly UsuarioService _service;
    private readonly JwtOptions _jwt;

    public AuthController(IOptions<JwtOptions> options, UsuarioService service)
    {
        _service = service;
        _jwt = options.Value;
    }

    [HttpPost("login")]
    public IActionResult Login(string email,string senha)
    {
        var user = _service.ObterPorEmail(email);

        if (user == null)
            return Unauthorized("Usuário inválido");

        if (!BCrypt.Net.BCrypt.Verify(senha, user.Senha))
            return Unauthorized("Senha inválida");

        var token = new Services.TokenService(_jwt).GerarToken(user);

        return Ok(new { token });
    }
}
