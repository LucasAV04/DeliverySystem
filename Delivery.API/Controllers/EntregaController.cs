
using Delivery.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
 
    public class EntregaController : ControllerBase
    {
        private readonly EntregaService _service;

        public EntregaController(EntregaService service)
        {
            _service = service;
        }

        [HttpPost("IniciarEntrega")]
        public IActionResult IniciarEntrega([FromBody] EntregaRequest request)
        {
            try
            {
                _service.IniciarEntrega(request.pedidoId, request.motoristaId, request.veiculoId);
                return Ok("Entrega Iniciada");
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
        [HttpPut("{id}/ConcluirEntrega")]
        public IActionResult ConcluirEntrega(int id, [FromBody] ObservacaoRequest? request = null)
        {
            try
            {
                _service.ConcluirEntrega(id, request?.Observacoes);
                return Ok("Entrega Concluida");
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

        [HttpPut("{id}/RegistrarFalha")]
        public IActionResult RegistrarFalha(int id, [FromBody] ObservacaoRequest request)
        {
            try
            {
                _service.RegistrarFalha(id, request.Observacoes);
                return Ok("A Falha foi Registrada");
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

        [HttpDelete("{id}/CancelarEntrega")]
        public IActionResult CancelarEntrega(int id, [FromBody] ObservacaoRequest request)
        {
            try
            {
                _service.CancelarEntrega(id, request.Observacoes);
                return Ok("Entrega Cancelada");
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

        [HttpGet("ListarEntregas")]
        public IActionResult ListarEntregas()
        {
            var entrega = _service.ListarEntregas();
            if (entrega.Count == 0)
                return NotFound("Não têm entregas Cadastradas");
            return Ok(entrega);
        }

        [HttpGet("Relatorio/Pendentes")]
        public IActionResult Pendentes()
        {
            var entrega = _service.ListarEntregasPendentes();
            if (entrega.Count == 0)
                return NotFound("Não têm entregas pendentes");
            return Ok(entrega);
        }

        [HttpGet("{id}/Relatorio/PorMotorista")]
        public IActionResult PorMotorista(int motoristaId)
        {
            try
            {
                var entrega = _service.ListarEntregasPorMotorista(motoristaId);
                if (entrega.Count == 0)
                    return NotFound("Nenhuma entrega encontrada para este motorista");
                return Ok(entrega);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("/Relatorio/PorPeriodo")]
        public IActionResult PorPeriodo([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
        {
            try
            {
                var entregas = _service.ListarEntregasPorPeriodo(inicio, fim);
                if (entregas.Count == 0)
                    return NotFound("Nenhuma entrega encontrada nesse período.");
                return Ok(entregas);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Relatorio/VeiculosMaisUtilizados")]
        public IActionResult VeiculosMaisUtilizados()
        {
            var ranking = _service.VeiculosMaisUtilizados();
            if (ranking.Count == 0)
                return NotFound("Nenhuma entrega registrada.");
            return Ok(ranking.Select(r => new { r.VeiculoId, r.TotalEntregas }));
        }
    }
    public record EntregaRequest(int pedidoId, int motoristaId, int veiculoId);
    public record ObservacaoRequest(string Observacoes);
}
