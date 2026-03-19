
using Delivery.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
  
    public class PedidoController : ControllerBase
    {
        private readonly PedidoService _service;

        public PedidoController(PedidoService service)
        {
            _service = service;
        }

        [HttpPost("Adicionar")]
        public IActionResult AdicionarPedido([FromBody] PedidoRequest request)
        {
            try
            {
                _service.AdicionarPedido(request.clienteId, request.enderecoEntrega);
                return Ok("Pedido Adiconado");
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

        [HttpGet("ListarPedidos")]
        public IActionResult ListarPedidos()
        {
            var pedido = _service.ListarPedidos();
            if (pedido.Count == 0)
                return NotFound("Pedidos não encontrados");
            return Ok(pedido);
        }

        [HttpPut("ListarPedidosCancelados")]
        public IActionResult ListarPedidosCancelados()
        {
            var pedido = _service.ListarPedidosCancelados();
            if (pedido.Count == 0)
                return NotFound("Pedidos cancelados não encontrados");
            return Ok(pedido);
        }

        [HttpPut("{id}/AtualizarPedido")]
        public IActionResult AtualizarPedido(int id, [FromBody] PedidoRequest request)
        {
            try
            {
                _service.AtualizarPedido(id, request.clienteId, request.enderecoEntrega);
                return Ok("Pedido atualizado com sucesso");
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}/ConfirmarPedido")]
        public IActionResult ConfirmaPedido(int id)
        {
            try
            {
                _service.ConfirmarPedido(id);
                return Ok("Pedido confirmado");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{id}/CancelarPedido")]
        public IActionResult CancelarPedido(int id)
        {
            try
            {
                _service.CancelarPedido(id);
                return Ok("Pedido Cancelado");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}/EmPreparacao")]
        public IActionResult EmPreparacao(int id)
        {
            try
            {
                _service.EmPreparacao(id);
                return Ok("Pedido em Preparação");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}/ProntoParaEnvio")]
        public IActionResult ProntoParaEnvio(int id)
        {
            try
            {
                _service.ProntoParaEnvio(id);
                return Ok("Pedido pronto para envio");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

    }
    public record PedidoRequest(int clienteId, string enderecoEntrega);
}
