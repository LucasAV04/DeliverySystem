using Delivery.Domain;
using Delivery.Infrastructure.Interfaces;

namespace Delivery.Application.Services
{
    public class PedidoService
    {
        private readonly IPedidoRepository _pedRepo;
        private readonly IClienteRepository _cliRepo;

        public PedidoService(IPedidoRepository pedidoRepository, IClienteRepository cliRepo)
        {
            _pedRepo = pedidoRepository;
            _cliRepo = cliRepo;
        }

        public void AdicionarPedido(int clienteId, string enderecoEntrega)
        {
            if (string.IsNullOrWhiteSpace(enderecoEntrega))
                throw new ArgumentException("O endereço de entrega é obrigatório");

            var cliente = _cliRepo.BuscarClienteId(clienteId);
            if (cliente == null)
                throw new KeyNotFoundException("Cliente não encontrado");

            var pedido = new Pedido
            {
                ClienteId = clienteId,
                EnderecoEntrega = enderecoEntrega,
                DataSolicitacao = DateTime.Now,
                Status = Pedido.StatusPedido.Criado
            };
            _pedRepo.AdicionarPedido(pedido);
        }

        public List<Pedido> ListarPedidos()
        {
            return _pedRepo.ListarPedidos();
        }

        public List<Pedido> ListarPedidosCancelados()
        {
            return _pedRepo.ListarPedidosCancelados();
        }

        public void AtualizarPedido(int id, int clienteId, string enderecoEntrega)
        {
            if (string.IsNullOrWhiteSpace(enderecoEntrega))
                throw new ArgumentException("O endereço de entrega é obrigatório");

            var pedido = _pedRepo.BuscarPedido(id);
            if (pedido == null)
                throw new KeyNotFoundException("Pedido não encontrado");

            pedido.AtualizarDados(clienteId, enderecoEntrega);
            _pedRepo.AtualizarPedido(pedido);
        }

        public void ConfirmarPedido(int id)
        {
            var pedido = _pedRepo.BuscarPedido(id);
            if (pedido == null)
                throw new KeyNotFoundException("Pedido não encontrado");

            if (pedido.Status != Pedido.StatusPedido.Criado)
                throw new InvalidOperationException("Somente pedidos com status 'Criado' podem ser confirmados");

            pedido.Status = Pedido.StatusPedido.Confirmado;
            _pedRepo.AtualizarPedido(pedido);
        }

        public void CancelarPedido(int id)
        {
            var pedido = _pedRepo.BuscarPedido(id);
            if (pedido == null)
                throw new KeyNotFoundException("Pedido não encontrado");

            if (pedido.Status == Pedido.StatusPedido.EmRota || pedido.Status == Pedido.StatusPedido.Entregue)
                throw new InvalidOperationException($"Pedido com status '{pedido.Status}' não pode ser cancelado");

            pedido.Status = Pedido.StatusPedido.Cancelado;
            _pedRepo.AtualizarPedido(pedido);
        }

        public void EmPreparacao(int id)
        {
            var pedido = _pedRepo.BuscarPedido(id);
            if (pedido == null)
                throw new KeyNotFoundException("Pedido não encontrado");

            if (pedido.Status != Pedido.StatusPedido.Confirmado)
                throw new InvalidOperationException("Somente pedidos 'Confirmados' podem ir para preparação");

            pedido.Status = Pedido.StatusPedido.EmPreparacao;
            _pedRepo.AtualizarPedido(pedido);
        }

        public void ProntoParaEnvio(int id)
        {
            var pedido = _pedRepo.BuscarPedido(id);
            if (pedido == null)
                throw new KeyNotFoundException("Pedido não encontrado");

            if (pedido.Status != Pedido.StatusPedido.EmPreparacao)
                throw new InvalidOperationException("Somente pedidos 'EmPreparacao' podem ser marcados como prontos para envio");

            pedido.Status = Pedido.StatusPedido.ProntoParaEnvio;
            _pedRepo.AtualizarPedido(pedido);
        }
    }
}