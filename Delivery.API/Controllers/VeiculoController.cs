using Delivery.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class VeiculoController:ControllerBase
    {
        private readonly VeiculoService _service;

        public VeiculoController(VeiculoService service)
        {
            _service = service;
        }

        [HttpPost("Adicionar")]
        public IActionResult AdicionarVeiculo([FromBody]VeiculoRequest request)
        {
            try
            {
                _service.AdicionarVeiculo(request.placa, request.modelo, request.ano, request.capacidadeCarga);
                return Ok("Veiculo Adicionado");
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

        [HttpGet("ListarVeiculos")]
        public IActionResult ListarVeiculos()
        {
            var veiculo = _service.ListarVeiculos();
            if (veiculo.Count == 0)
                return NotFound("Veículos não Encontrados");
            return Ok(veiculo);
        }

        [HttpGet("ListarVeiculosDisponivel")]
        public IActionResult ListarVeiculosDisponivel()
        {
            var veiculo = _service.ListarVeiculoDisponivel();
            if (veiculo.Count == 0)
                return NotFound("Veiculos não Encontrados");
            return Ok(veiculo);
        }

        [HttpPut("{id}/Atualizar")]
        public IActionResult Atualizar(int id,[FromBody] VeiculoRequest request)
        {
            try
            {
                _service.AtualizarVeiculo(id, request.placa, request.modelo, request.ano, request.capacidadeCarga);
                return Ok("Veiculo Atualizado com Sucesso");
            }
            catch(KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}/Manutencao")]
        public IActionResult ManutencaoVeiculo(int id)
        {
            try
            {
                _service.ManutencaoVeiculo(id);
                return Ok("Veiculo foi para a Manutenção");
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}/Inativar")]
        public IActionResult InativarVeiculo(int id)
        {
            try
            {
                _service.InativarVeiculo(id);
                return Ok("Veiculo foi Inativado");
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}/Ativar")]
        public IActionResult AtivarVeiculo(int id)
        {
            try
            {
                _service.AtivarVeiculo(id);
                return Ok("Veiculo Ativado");
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
    public record VeiculoRequest(string placa, string modelo, string ano, decimal capacidadeCarga);
}
