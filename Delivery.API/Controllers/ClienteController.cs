using Delivery.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _service;

        public ClienteController(ClienteService service)
        {
            _service = service;
        }

        [HttpPost("Adicionar")]
        public IActionResult Adicionar([FromBody] ClienteRequest request)
        {
            try
            {
                _service.AdicionarCliente(request.Nome, request.Cpf, request.Email);
                return Ok("Cliente cadastrado com sucesso!");
            }
            catch (ArgumentException ex) 
            { 
                return BadRequest(ex.Message); 
            }
            catch (InvalidOperationException ex) 
            { 
                return Conflict(ex.Message); 
            }
        }

        [HttpGet("ListarTodos")]
        public IActionResult ListarTodos()
        {
            var clientes = _service.ListarClientes();
            if (clientes.Count == 0)
                return NotFound("Nenhum cliente cadastrado.");
            return Ok(clientes);
        }

        [HttpGet("ListarVip")]
        public IActionResult ListarVip()
        {
            var vips = _service.ListarClientesVip();
            if (vips.Count == 0)
                return NotFound("Nenhum cliente VIP cadastrado.");
            return Ok(vips);
        }

        [HttpPut("{id}/Atualizar")]
        public IActionResult Atualizar(int id, [FromBody] ClienteRequest request)
        {
            try
            {
                _service.AtualizarCliente(id, request.Nome, request.Cpf, request.Email);
                return Ok("Cliente atualizado com sucesso!");
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }
    }

    public record ClienteRequest(string Nome, string Cpf, string Email);
}
