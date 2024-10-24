using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partymanager.Application.Models;
using partymanager.Domain.Entidades;
using partymanager.Domain.Interfaces;
using partymanager.Infrastructure.Data.Context;
using partymanager.Service.Validators;
using System.Linq;

namespace partymanager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IBaseService<Usuario> _baseService;
        private readonly IBaseService<Usuario> _usuarioService;
        private readonly SqlServerContext _context;  // Injeção do contexto

        public UsuarioController(IBaseService<Usuario> baseService, IBaseService<Usuario> usuarioService, SqlServerContext context)
        {
            _baseService = baseService;
            _usuarioService = usuarioService; // Injeção para consultar os usuários
            _context = context; // Injeção do contexto para acessar o banco de dados diretamente
        }

        // Método auxiliar para lidar com erros
        private IActionResult Execute(Func<object> func)
        {
            try
            {
                var result = func();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Criar um novo usuário
        [HttpPost]
        public IActionResult Create(UsuarioModel usuario)
        {
            if (usuario == null)
                return BadRequest("Dados inválidos.");

            // Verificar se o email já existe
            var usuarioExistente = _context.usuario.AsNoTracking().FirstOrDefault(u => u.email == usuario.email);
            if (usuarioExistente != null)
            {
                return Conflict("Já existe um usuário com este email."); // Retorna 409 para conflito de email
            }

            try
            {
                // Validação com o UsuarioValidator
                _baseService.Add<UsuarioModel, UsuarioValidator>(usuario);
                return Ok("Usuário criado com sucesso."); // Retorna 200 ou 201 para criação bem-sucedida
            }
            catch (Exception ex)
            {
                // Capturar e retornar qualquer erro que ocorra durante o processo
                return BadRequest($"Erro ao criar usuário: {ex.Message}");
            }
        }


        // Atualizar um usuário existente
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UsuarioModel usuarioModel)
        {
            // Verificar se os dados do usuário são válidos
            if (usuarioModel == null)
                return BadRequest("Dados inválidos.");

            if (id <= 0)
                return BadRequest("ID inválido.");

            try
            {
                // Buscar o usuário existente no banco de dados usando a entidade correta 'Usuario'
                var usuarioDb = _context.usuario.AsNoTracking().FirstOrDefault(u => u.id == id);

                if (usuarioDb == null)
                {
                    return NotFound("Usuário não encontrado.");
                }

                // Verificar se o email já existe para outro usuário
                var usuarioExistente = _context.usuario
                    .AsNoTracking()
                    .FirstOrDefault(u => u.email == usuarioModel.email && u.id != id);

                if (usuarioExistente != null)
                {
                    return Conflict("Este email já está sendo usado por outro usuário.");
                }

                // Atualizar os campos necessários no objeto da entidade 'Usuario'
                usuarioDb.nome = usuarioModel.nome;
                usuarioDb.email = usuarioModel.email;
                usuarioDb.senha = usuarioModel.senha; // Certifique-se de que a senha está sendo tratada corretamente

                // Desanexar qualquer instância rastreada
                _context.Entry(usuarioDb).State = EntityState.Detached;

                // Anexar a nova versão do usuário e marcar como modificada
                _context.Entry(usuarioDb).State = EntityState.Modified;

                // Salvar alterações no banco de dados
                _context.SaveChanges();

                return Ok(usuarioDb);
            }
            catch (Exception ex)
            {
                // Capturar e retornar qualquer erro que ocorra durante o processo
                return BadRequest($"Erro ao atualizar usuário: {ex.Message}");
            }
        }




        // Deletar um usuário por ID
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("ID inválido.");

            Execute(() =>
            {
                _baseService.Delete(id);
                return true;
            });
            return NoContent();
        }

        // Obter um usuário por ID
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id <= 0)
                return BadRequest("ID inválido.");

            return Execute(() => _baseService.GetById<UsuarioModel>(id));
        }

        // Listar todos os usuários
        [HttpGet]
        public IActionResult GetAll()
        {
            return Execute(() => _baseService.Get<UsuarioModel>());
        }
    }
}
