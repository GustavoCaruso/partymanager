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
    public class ItemController : ControllerBase
    {
        private readonly SqlServerContext _context;

        public ItemController(SqlServerContext context)
        {
            _context = context;
        }

        // Criação de um novo item
        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] ItemDto itemDto)
        {
            if (itemDto == null)
                return BadRequest("Dados inválidos.");

            var item = new Item
            {
                nome = itemDto.nome,
                idTipoItem = itemDto.idTipoItem
            };

            _context.item.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItemById), new { id = item.id }, item);
        }

        // Obter um item pelo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var item = await _context.item.FindAsync(id);

            if (item == null)
                return NotFound("Item não encontrado.");

            return Ok(item);
        }

        // Listar todos os itens
        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _context.item.ToListAsync();
            return Ok(items);
        }

        // Atualizar um item
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] ItemDto itemDto)
        {
            var itemExistente = await _context.item.FindAsync(id);

            if (itemExistente == null)
                return NotFound("Item não encontrado.");

            itemExistente.nome = itemDto.nome;
            itemExistente.idTipoItem = itemDto.idTipoItem;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Erro ao atualizar o item.");
            }

            return Ok(itemExistente);
        }

        // Deletar um item
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.item.FindAsync(id);
            if (item == null)
                return NotFound("Item não encontrado.");

            _context.item.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent(); // Sucesso ao deletar
        }
    }
}
