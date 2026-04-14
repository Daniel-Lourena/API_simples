using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ModuloCadastro.Entity.Cadastro.Usuario;
using ModuloCadastro.Enum.Usuario.Perfil;
using ModuloConfiguracoes.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_simples.Services
{
    public class TokenService
    {
        private readonly JwtOptions _jwt;

        public TokenService(JwtOptions options)
        {
            _jwt = options;
        }

        public string GerarToken(UsuarioEntity usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));

            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var handlerJWT = new JwtSecurityTokenHandler();
            var token = handlerJWT.CreateToken(new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(GerarClaims(usuario)),
                SigningCredentials = credenciais,
                Expires = DateTime.UtcNow.AddHours(1),
            });

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private List<Claim> GerarClaims(UsuarioEntity usuario)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.PerfilAcesso.GetDescription() ?? EPerfilAcesso.USUARIO_COMUM.GetDescription())
            };

            foreach (var perm in usuario.ListaPermissoes)
            {
                claims.Add(new Claim("permissao", ((int)perm.PermissaoId).ToString()));
            }
            return claims;
        }
    }
}
