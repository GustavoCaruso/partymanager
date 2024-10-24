using Microsoft.AspNetCore.Mvc;
using partymanager.Application.Models;
using partymanager.Domain.Entidades;
using partymanager.Domain.Interfaces;
using partymanager.Service.Validators;
using System;

namespace partymanager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoItemController : ControllerBase
    {
        private readonly IBaseService<TipoItem> _baseService;

        public TipoItemController(IBaseService<TipoItem> baseService)
        {
            _baseService = baseService;
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

        // Criar um novo TipoItem
        [HttpPost]
        public IActionResult Create(TipoItemModel tipoitem)
        {
            if (tipoitem == null)
                return BadRequest("Dados inválidos.");

            return Execute(() => _baseService.Add<TipoItemModel, TipoItemValidator>(tipoitem));
        }

        // Atualizar um TipoItem existente
        [HttpPut]
        public IActionResult Update(TipoItemModel tipoitem)
        {
            if (tipoitem == null)
                return BadRequest("Dados inválidos.");

            return Execute(() => _baseService.Update<TipoItemModel, TipoItemValidator>(tipoitem));
        }

        // Deletar um TipoItem por ID
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

        // Obter um TipoItem por ID
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id <= 0)
                return BadRequest("ID inválido.");

            return Execute(() => _baseService.GetById<TipoItemModel>(id));
        }

        // Listar todos os TipoItem
        [HttpGet]
        public IActionResult GetAll()
        {
            return Execute(() => _baseService.Get<TipoItemModel>());
        }
    }
}
