
using Delivery.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
  
    public class MotoristaController : ControllerBase
    {
        private readonly MotoristaService _service;

        public MotoristaController(MotoristaService service)
        {
            _service = service;
        }
        [HttpPost("Adicionar")]

        public IActionResult Adicionar([FromBody] MotoristaRequest request)
        {
            try
            {
                _service.AdicionarMotorista(request.nome, request.telefone, request.cnh);
                return Ok("Motorista Cadastrado com Sucesso");
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

        [HttpGet("ListarMotoristas")]
        public IActionResult ListarMotoristas()
        {
            var motorista = _service.ListarMotoristas();
            if (motorista.Count == 0)
                return NotFound("Nenhum Motorista Cadastrado");
            return Ok(motorista);
        }

        [HttpGet("ListarMotoristaAtivos")]
        public IActionResult ListarMotoristaAtivos()
        {
            var motorista = _service.ListarMotoristaAtivos();
            if (motorista.Count == 0)
                return NotFound("Nenhum Motorista Cadastrado");
            return Ok(motorista);
        }

        [HttpPut("{id}/Bloquear")]
        public IActionResult BloquearMotorista(int id)
        {
            try
            {
                _service.BloquearMotorista(id);
                return Ok("Motorista Bloqueado");
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

        [HttpPut("{id}/Inativar")]
        public IActionResult InativarMotorista(int id)
        {
            try
            {
                _service.InativarMotorista(id);
                return Ok("Motorista Inativado");
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

        [HttpPut("{id}/Ativar")]
        public IActionResult AtivarMotorista(int id)
        {
            try
            {
                _service.AtivarMotorista(id);
                return Ok("Motorista Ativado");
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

        [HttpPut("{id}/Atualizar")]
        public IActionResult Atualizar(int id, [FromBody] MotoristaRequest request)
        {
            try
            {
                _service.AtualizarMotorista(id, request.nome, request.telefone, request.cnh);
                return Ok("Motorista Atualizado com Sucesso");
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
    }
    public record MotoristaRequest(string nome, string telefone, string cnh);
}
