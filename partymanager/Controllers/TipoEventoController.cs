using Microsoft.AspNetCore.Mvc;
using partymanager.Application.Dto;
using partymanager.Infrastructure.Data;
using partymanager.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using partymanager.Infrastructure.Data.Context;

namespace partymanager.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoEventoController : ControllerBase
    {
        private readonly SqlServerContext _context;

        public TipoEventoController(SqlServerContext context)
        {
            _context = context;
        }

        // Criar um novo tipo de evento
        [HttpPost]
        public async Task<IActionResult> CreateTipoEvento([FromBody] TipoEventoDto tipoEventoDto)
        {
            if (tipoEventoDto == null)
                return BadRequest("Dados inválidos.");

            // Criar o novo TipoEvento
            var tipoEvento = new TipoEvento
            {
                nome = tipoEventoDto.nome
            };

            _context.tipoevento.Add(tipoEvento);
            await _context.SaveChangesAsync(); // Salvar o TipoEvento para obter o ID

            // Associar itens ao TipoEvento automaticamente (se especificado no DTO)
            if (tipoEventoDto.Itens != null && tipoEventoDto.Itens.Any())
            {
                foreach (var itemDto in tipoEventoDto.Itens)
                {
                    var tipoEventoItem = new TipoEventoItem
                    {
                        idTipoEvento = tipoEvento.id,  // ID do TipoEvento que foi criado
                        idItem = itemDto.idItem,       // ID do Item que está associado
                        quantidadePorPessoa = itemDto.quantidadePorPessoa
                    };
                    _context.tipoeventoitem.Add(tipoEventoItem);
                }

                await _context.SaveChangesAsync(); // Salvar as associações de itens
            }

            return CreatedAtAction(nameof(GetTipoEventoById), new { id = tipoEvento.id }, tipoEvento);
        }

        // Obter um tipo de evento pelo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTipoEventoById(int id)
        {
            var tipoEvento = await _context.tipoevento
                .Include(te => te.tipoeventoitem)
                .ThenInclude(tei => tei.item)
                .FirstOrDefaultAsync(te => te.id == id);

            if (tipoEvento == null)
                return NotFound("Tipo de evento não encontrado.");

            return Ok(tipoEvento);
        }

        // Listar todos os tipos de evento
        [HttpGet]
        public async Task<IActionResult> GetAllTipoEventos()
        {
            var tiposEvento = await _context.tipoevento
                .Include(te => te.tipoeventoitem)
                .ThenInclude(tei => tei.item)
                .ToListAsync();

            return Ok(tiposEvento);
        }

        // Atualizar um tipo de evento
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTipoEvento(int id, [FromBody] TipoEventoDto tipoEventoDto)
        {
            var tipoEventoExistente = await _context.tipoevento
                .Include(te => te.tipoeventoitem)
                .FirstOrDefaultAsync(te => te.id == id);

            if (tipoEventoExistente == null)
                return NotFound("Tipo de evento não encontrado.");

            tipoEventoExistente.nome = tipoEventoDto.nome;

            // Remover associações existentes de itens
            _context.tipoeventoitem.RemoveRange(tipoEventoExistente.tipoeventoitem);

            // Adicionar novas associações de itens
            if (tipoEventoDto.Itens != null && tipoEventoDto.Itens.Any())
            {
                foreach (var itemDto in tipoEventoDto.Itens)
                {
                    var tipoEventoItem = new TipoEventoItem
                    {
                        idTipoEvento = tipoEventoExistente.id,
                        idItem = itemDto.idItem,
                        quantidadePorPessoa = itemDto.quantidadePorPessoa
                    };
                    _context.tipoeventoitem.Add(tipoEventoItem);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(tipoEventoExistente);
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Erro ao atualizar o tipo de evento.");
            }
        }

        // Deletar um tipo de evento
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoEvento(int id)
        {
            var tipoEvento = await _context.tipoevento
                .Include(te => te.tipoeventoitem)
                .FirstOrDefaultAsync(te => te.id == id);

            if (tipoEvento == null)
                return NotFound("Tipo de evento não encontrado.");

            // Remover as associações de itens primeiro
            _context.tipoeventoitem.RemoveRange(tipoEvento.tipoeventoitem);
            _context.tipoevento.Remove(tipoEvento);

            try
            {
                await _context.SaveChangesAsync();
                return NoContent(); // Sucesso ao deletar
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar o tipo de evento: {ex.Message}");
            }
        }
    }
}
