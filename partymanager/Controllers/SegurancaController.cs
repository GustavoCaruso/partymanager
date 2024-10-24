using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using partymanager.Application.Dto;
using partymanager.Domain.Entidades;
using partymanager.Infrastructure.Data.Context;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace partymanager.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SegurancaController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly SqlServerContext _context;  // Acesso ao contexto do banco de dados

        public SegurancaController(IConfiguration config, SqlServerContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto loginDetalhes)
        {
            // Validar o usuário através do email e senha no banco de dados
            var usuario = await ValidarUsuarioAsync(loginDetalhes.email, loginDetalhes.senha);

            if (usuario != null)
            {
                // Gerar o token JWT se o usuário for válido
                var tokenString = GerarTokenJWT(usuario);
                // Agora retorna o ID do usuário junto com o token JWT
                return Ok(new { token = tokenString, id = usuario.id, nomeUsuario = usuario.nome });
            }
            else
            {
                return Unauthorized("Email ou senha incorretos.");
            }
        }


        private string GerarTokenJWT(Usuario usuario)
        {
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.email),
                new Claim(JwtRegisteredClaimNames.Email, usuario.email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("nome", usuario.nome) // O nome será adicionado ao token JWT
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(120), // Token válido por 120 minutos
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private async Task<Usuario> ValidarUsuarioAsync(string email, string senha)
        {
            // Busca o usuário no banco de dados através do email
            var usuario = await _context.usuario.FirstOrDefaultAsync(u => u.email == email);

            // Comparação direta de senhas (em texto simples)
            if (usuario != null && usuario.senha == senha) // Verifica se a senha está correta
            {
                return usuario; // Retorna o usuário se o email e a senha forem válidos
            }

            return null; // Retorna null se a validação falhar
        }
    }
}
