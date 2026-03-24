using Delivery.Domain;
using Delivery.Infrastructure.Interfaces;

namespace Delivery.Application.Services
{
    public class EntregaService
    {
        private readonly IEntregaRepository _entregaRepo;
        private readonly IPedidoRepository _pedidoRepo;
        private readonly IMotoristaRepository _motoristaRepo;
        private readonly IVeiculoRepository _veiculoRepo;

        public EntregaService(
            IEntregaRepository entregaRepo,
            IPedidoRepository pedidoRepo,
            IMotoristaRepository motoristaRepo,
            IVeiculoRepository veiculoRepo)
        {
            _entregaRepo = entregaRepo;
            _pedidoRepo = pedidoRepo;
            _motoristaRepo = motoristaRepo;
            _veiculoRepo = veiculoRepo;
        }

        
        public void IniciarEntrega(int pedidoId, int motoristaId, int veiculoId)
        {
            var pedido = _pedidoRepo.BuscarPedido(pedidoId);
            if (pedido == null)
                throw new Exception("Pedido não encontrado");

            if (pedido.Status != Pedido.StatusPedido.ProntoParaEnvio)
                throw new Exception("O pedido precisa estar com status 'ProntoParaEnvio' para iniciar a entrega");

            var motorista = _motoristaRepo.BuscarPorId(motoristaId);
            if (motorista == null)
                throw new Exception("Motorista não encontrado");

            if (motorista.Status != Motorista.StatusMotorista.Ativo)
                throw new Exception("O motorista precisa estar com status 'Ativo' para realizar entregas");

            var veiculo = _veiculoRepo.BuscarVeiculo(veiculoId);
            if (veiculo == null)
                throw new Exception("Veículo não encontrado");

            if (veiculo.Status != Veiculo.StatusVeiculo.Disponivel)
                throw new Exception("O veículo precisa estar com status 'Disponivel' para ser utilizado");

         
            pedido.Status = Pedido.StatusPedido.EmRota;
            _pedidoRepo.AtualizarPedido(pedido);

          
            motorista.Status = Motorista.StatusMotorista.EmRota;
            _motoristaRepo.AtualizarMotorista(motorista);

            
            veiculo.Status = Veiculo.StatusVeiculo.EmUso;
            _veiculoRepo.AtualizarVeiculo(veiculo);

           
            var entrega = new Entrega
            {
                PedidoId = pedidoId,
                MotoristaId = motoristaId,
                VeiculoId = veiculoId,
                DataSaida = DateTime.UtcNow,
                Status = Entrega.StatusEntrega.EmAndamento
            };

            _entregaRepo.AdicionarEntrega(entrega);
        }

     
        public void ConcluirEntrega(int entregaId, string? observacoes = null)
        {
            var entrega = _entregaRepo.BuscarEntrega(entregaId);
            if (entrega == null)
                throw new Exception("Entrega não encontrada");

            if (entrega.Status != Entrega.StatusEntrega.EmAndamento)
                throw new Exception("Somente entregas em andamento podem ser concluídas");

            entrega.Status = Entrega.StatusEntrega.Concluida;
            entrega.DataEntrega = DateTime.UtcNow;
            entrega.Observacoes = observacoes;
            _entregaRepo.AtualizarEntrega(entrega);

        
            var motorista = _motoristaRepo.BuscarPorId(entrega.MotoristaId);
            if (motorista != null)
            {
                motorista.Status = Motorista.StatusMotorista.Ativo;
                _motoristaRepo.AtualizarMotorista(motorista);
            }

            var veiculo = _veiculoRepo.BuscarVeiculo(entrega.VeiculoId);
            if (veiculo != null)
            {
                veiculo.Status = Veiculo.StatusVeiculo.Disponivel;
                _veiculoRepo.AtualizarVeiculo(veiculo);
            }

           
            var pedido = _pedidoRepo.BuscarPedido(entrega.PedidoId);
            if (pedido != null)
            {
                pedido.Status = Pedido.StatusPedido.Entregue;
                _pedidoRepo.AtualizarPedido(pedido);
            }
        }

        
        public void RegistrarFalha(int entregaId, string observacoes)
        {
            var entrega = _entregaRepo.BuscarEntrega(entregaId);
            if (entrega == null)
                throw new Exception("Entrega não encontrada");

            if (entrega.Status != Entrega.StatusEntrega.EmAndamento)
                throw new Exception("Somente entregas em andamento podem ter falha registrada");

            if (string.IsNullOrWhiteSpace(observacoes))
                throw new Exception("Informe o motivo da falha nas observações");

            entrega.Status = Entrega.StatusEntrega.Falha;
            entrega.DataEntrega = DateTime.UtcNow;
            entrega.Observacoes = observacoes;
            _entregaRepo.AtualizarEntrega(entrega);

           
            var motorista = _motoristaRepo.BuscarPorId(entrega.MotoristaId);
            if (motorista != null)
            {
                motorista.Status = Motorista.StatusMotorista.Ativo;
                _motoristaRepo.AtualizarMotorista(motorista);
            }

            var veiculo = _veiculoRepo.BuscarVeiculo(entrega.VeiculoId);
            if (veiculo != null)
            {
                veiculo.Status = Veiculo.StatusVeiculo.Disponivel;
                _veiculoRepo.AtualizarVeiculo(veiculo);
            }
        }

        
        public void CancelarEntrega(int entregaId, string observacoes)
        {
            var entrega = _entregaRepo.BuscarEntrega(entregaId);
            if (entrega == null)
                throw new Exception("Entrega não encontrada");

            if (entrega.Status == Entrega.StatusEntrega.Concluida)
                throw new Exception("Entregas concluídas não podem ser canceladas");

            if (entrega.Status == Entrega.StatusEntrega.Cancelada)
                throw new Exception("Esta entrega já foi cancelada");

            entrega.Status = Entrega.StatusEntrega.Cancelada;
            entrega.Observacoes = observacoes;
            _entregaRepo.AtualizarEntrega(entrega);

          
            var motorista = _motoristaRepo.BuscarPorId(entrega.MotoristaId);
            if (motorista != null && motorista.Status == Motorista.StatusMotorista.EmRota)
            {
                motorista.Status = Motorista.StatusMotorista.Ativo;
                _motoristaRepo.AtualizarMotorista(motorista);
            }

            var veiculo = _veiculoRepo.BuscarVeiculo(entrega.VeiculoId);
            if (veiculo != null && veiculo.Status == Veiculo.StatusVeiculo.EmUso)
            {
                veiculo.Status = Veiculo.StatusVeiculo.Disponivel;
                _veiculoRepo.AtualizarVeiculo(veiculo);
            }

         
            var pedido = _pedidoRepo.BuscarPedido(entrega.PedidoId);
            if (pedido != null)
            {
                pedido.Status = Pedido.StatusPedido.ProntoParaEnvio;
                _pedidoRepo.AtualizarPedido(pedido);
            }
        }

   

        public List<Entrega> ListarEntregas()
        {
            return _entregaRepo.ListarEntregas();
        }

        public List<Entrega> ListarEntregasPendentes()
        {
            return _entregaRepo.ListarEntregasPendentes();
        }

        public List<Entrega> ListarEntregasPorMotorista(int motoristaId)
        {
            var motorista = _motoristaRepo.BuscarPorId(motoristaId);
            if (motorista == null)
                throw new Exception("Motorista não encontrado");

            return _entregaRepo.ListarEntregasPorMotorista(motoristaId);
        }

        public List<Entrega> ListarEntregasPorPeriodo(DateTime inicio, DateTime fim)
        {
            if (inicio > fim)
                throw new Exception("A data de início não pode ser maior que a data de fim");

            return _entregaRepo.ListarEntregasPorPeriodo(inicio, fim);
        }

        
        public List<(int VeiculoId, int TotalEntregas)> VeiculosMaisUtilizados()
        {  
            return _entregaRepo.ListarEntregas()
                .GroupBy(e => e.VeiculoId)
                .Select(g => (VeiculoId: g.Key, TotalEntregas: g.Count()))
                .OrderByDescending(v => v.TotalEntregas)
                .ToList();
        }
    }
}