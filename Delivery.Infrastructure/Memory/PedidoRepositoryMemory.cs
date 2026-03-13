using Delivery.Domain;
using Delivery.Infrastructure.Interfaces;

namespace Delivery.Infrastructure.Memory
{
    public class PedidoRepositoryMemory : IPedidoRepository
    {
        private readonly List<Pedido> _pedidos = new();
        private int _proximoId = 1;

        public void AdicionarPedido(Pedido pedido)
        {
            pedido.Id = _proximoId++;
            _pedidos.Add(pedido);
        }

        public List<Pedido> ListarPedidos()
        {
            return _pedidos;
        }

        public List<Pedido> ListarPedidosCancelados()
        {
            return _pedidos
                .Where(p => p.Status == Pedido.StatusPedido.Cancelado)
                .ToList();
        }

        public Pedido? BuscarPedido(int id)
        {
            return _pedidos.FirstOrDefault(p => p.Id == id);
        }

        public void AtualizarPedido(Pedido pedido)
        {
            var index = _pedidos.FindIndex(p => p.Id == pedido.Id); // bug corrigido
            if (index == -1)
                throw new KeyNotFoundException("Não foi possível atualizar: pedido não encontrado");
            _pedidos[index] = pedido;
        }
    }
}