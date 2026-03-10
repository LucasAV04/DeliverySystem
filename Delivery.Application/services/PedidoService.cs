using Delivery.Domain;
using Delivery.Infrastructure.Interfaces;

namespace Delivery.Application.services
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
        public void AdicionarPedido(int clienteId,string enderecoEntrega)
        {
            var cliente = _cliRepo.BuscarClienteId(clienteId);
            if (cliente == null)
                throw new Exception("Cliente não Encontrado");
            Pedido pedido = new Pedido
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
            var pedido = _pedRepo.BuscarPedido(id);
            if (pedido == null)
                throw new Exception("Pedido não Encontrado");
            pedido.AtualizarDados(clienteId,enderecoEntrega);
            _pedRepo.AtualizarPedido(pedido);
        }

    }
}
