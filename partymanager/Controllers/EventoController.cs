using Microsoft.AspNetCore.Mvc;
using partymanager.Application.Dto;
using partymanager.Domain.Entidades;
using partymanager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using partymanager.Infrastructure.Data.Context;
using Microsoft.Extensions.Logging;

namespace partymanager.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly SqlServerContext _context;
        private readonly ILogger<EventoController> _logger;

        public EventoController(SqlServerContext context, ILogger<EventoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Criar evento
        [HttpPost]
        public async Task<IActionResult> CreateEvento([FromBody] EventoDto eventoDto)
        {
            if (eventoDto == null)
            {
                _logger.LogError("EventoDto é nulo.");
                return BadRequest("Dados inválidos.");
            }

            var evento = new Evento
            {
                nome = eventoDto.nome,
                data = eventoDto.data,
                numeroAdultos = eventoDto.numeroAdultos,
                numeroCriancas = eventoDto.numeroCriancas,
                idTipoEvento = eventoDto.idTipoEvento,
                idUsuario = eventoDto.idUsuario,
                localidade = eventoDto.localidade
            };

            _context.evento.Add(evento);

            try
            {
                await _context.SaveChangesAsync(); // Salva o evento primeiro para gerar o ID
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao salvar o evento no banco de dados: {ex.Message}");
                return StatusCode(500, "Erro ao salvar o evento no banco de dados.");
            }

            // Gerar os itens calculados para o evento
            var tipoEventoItens = await _context.tipoeventoitem
                .Include(tei => tei.item)
                .Where(tei => tei.idTipoEvento == eventoDto.idTipoEvento)
                .ToListAsync();

            var itensCalculados = new List<ItemCalculado>();

            foreach (var tipoEventoItem in tipoEventoItens)
            {
                // Calcular a quantidade total para o evento (adultos + crianças)
                var quantidadeTotal = (eventoDto.numeroAdultos + eventoDto.numeroCriancas) * tipoEventoItem.quantidadePorPessoa;

                if (quantidadeTotal <= 0)
                {
                    _logger.LogWarning($"A quantidade calculada para o item {tipoEventoItem.item.nome} é inválida.");
                    continue;
                }

                var itemCalculado = new ItemCalculado
                {
                    idEvento = evento.id,
                    idItem = tipoEventoItem.idItem,
                    quantidade = quantidadeTotal
                };

                itensCalculados.Add(itemCalculado);
            }

            if (itensCalculados.Any())
            {
                _context.itemcalculado.AddRange(itensCalculados);

                try
                {
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Itens calculados para o evento {evento.nome} foram salvos com sucesso.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Erro ao salvar os itens calculados: {ex.Message}");
                    return StatusCode(500, "Erro ao salvar os itens calculados no banco de dados.");
                }
            }

            // Montar a resposta final
            var response = new
            {
                evento.id,
                evento.nome,
                evento.localidade,
                evento.data,
                evento.numeroAdultos,
                evento.numeroCriancas,
                itensCalculados = itensCalculados.Select(ic => new
                {
                    ItemNome = _context.item.Where(i => i.id == ic.idItem).Select(i => i.nome).FirstOrDefault(),
                    Quantidade = ic.quantidade
                })
            };

            return CreatedAtAction(nameof(GetEventoById), new { id = evento.id }, response);
        }

        // Listar todos os eventos
        [HttpGet]
        public async Task<IActionResult> GetAllEventos()
        {
            var eventos = await _context.evento
                .Include(e => e.itemcalculado)
                .ThenInclude(ic => ic.item)
                .ToListAsync();

            return Ok(eventos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventoById(int id)
        {
            var evento = await _context.evento
                .Include(e => e.itemcalculado)
                .ThenInclude(ic => ic.item)
                .FirstOrDefaultAsync(e => e.id == id);

            if (evento == null)
            {
                _logger.LogWarning($"Evento ID: {id} não encontrado.");
                return NotFound("Evento não encontrado.");
            }

            _logger.LogInformation($"Evento encontrado: {evento.nome}, ID: {evento.id}");

            // Montar a resposta com os itens calculados
            var response = new
            {
                evento.id,
                evento.nome,
                evento.localidade,
                evento.data,
                evento.numeroAdultos,
                evento.numeroCriancas,
                itensCalculados = evento.itemcalculado.Select(ic => new
                {
                    ItemNome = ic.item.nome,
                    Quantidade = ic.quantidade
                })
            };

            return Ok(response);
        }

        // Atualizar um evento
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvento(int id, [FromBody] EventoDto eventoDto)
        {
            var eventoExistente = await _context.evento.FindAsync(id);
            if (eventoExistente == null)
            {
                return NotFound("Evento não encontrado.");
            }

            eventoExistente.nome = eventoDto.nome;
            eventoExistente.data = eventoDto.data;
            eventoExistente.numeroAdultos = eventoDto.numeroAdultos;
            eventoExistente.numeroCriancas = eventoDto.numeroCriancas;
            eventoExistente.localidade = eventoDto.localidade;
            eventoExistente.idTipoEvento = eventoDto.idTipoEvento;
            eventoExistente.idUsuario = eventoDto.idUsuario;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Evento ID {id} atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao atualizar o evento: {ex.Message}");
                return StatusCode(500, "Erro ao atualizar o evento no banco de dados.");
            }

            return Ok(eventoExistente);
        }

        // Deletar evento
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            var evento = await _context.evento.FindAsync(id);
            if (evento == null)
            {
                _logger.LogWarning($"Evento ID: {id} não encontrado.");
                return NotFound("Evento não encontrado.");
            }

            _context.evento.Remove(evento);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Evento ID {id} deletado com sucesso.");
                return NoContent(); // Sucesso na deleção
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao deletar o evento: {ex.Message}");
                return StatusCode(500, "Erro ao deletar o evento no banco de dados.");
            }
        }

        // Novo método para listar os itens relacionados a um tipo de evento específico
        [HttpGet("{idTipoEvento}/itens")]
        public async Task<IActionResult> GetItensByTipoEvento(int idTipoEvento)
        {
            var itensRelacionados = await _context.tipoeventoitem
                .Include(tei => tei.item) // Inclui os detalhes do item
                .Where(tei => tei.idTipoEvento == idTipoEvento)
                .Select(tei => new
                {
                    tei.item.id,
                    tei.item.nome,
                    tei.quantidadePorPessoa // Quantidade padrão por pessoa
                })
                .ToListAsync();

            if (!itensRelacionados.Any())
            {
                _logger.LogWarning($"Nenhum item encontrado para o TipoEvento ID: {idTipoEvento}.");
                return NotFound("Nenhum item encontrado para este tipo de evento.");
            }

            return Ok(itensRelacionados);
        }

        // Listar eventos de um usuário específico
        [HttpGet("usuario/{idUsuario}")]
        public async Task<IActionResult> GetEventosByUsuario(int idUsuario)
        {
            var eventos = await _context.evento
                .Include(e => e.itemcalculado)
                .ThenInclude(ic => ic.item)
                .Where(e => e.idUsuario == idUsuario)
                .ToListAsync();

            if (eventos == null || !eventos.Any())
            {
                _logger.LogWarning($"Nenhum evento encontrado para o usuário ID: {idUsuario}.");
                return NotFound("Nenhum evento encontrado para este usuário.");
            }

            var response = eventos.Select(evento => new
            {
                evento.id,
                evento.nome,
                evento.localidade,
                evento.data,
                evento.numeroAdultos,
                evento.numeroCriancas,
                itensCalculados = evento.itemcalculado.Select(ic => new
                {
                    ItemNome = ic.item.nome,
                    Quantidade = ic.quantidade
                })
            });

            return Ok(response);
        }
    }
}
