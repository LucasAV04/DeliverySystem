

using Delivery.Domain;
using Delivery.Infrastructure.Interfaces;

namespace Delivery.Infrastructure.Memory
{
    public class PedidoRepositoryMemory:IPedidoRepository
    {
        private readonly List<Pedido> _pedidos = new();
        int id = 1;

        public void AdicionarPedido(Pedido pedido)
        {
            _pedidos.Add(pedido);
            pedido.Id = id++;
        }
        public List<Pedido> ListarPedidos()
        {
            return _pedidos;
        }
        public List<Pedido> ListarPedidosCancelados()
        {
            return _pedidos.Where(p => p.Status == Pedido.StatusPedido.Cancelado).ToList();
        }
        public Pedido BuscarPedido(int id)
        {
            return _pedidos.FirstOrDefault(p => p.Id == id);
        }
        public void AtualizarPedido(Pedido pedido)
        {
            var index = _pedidos.FindIndex(p => p.Id == id);
            if (index == -1)
                throw new Exception("Não foi possivel Atualizar as informações do Pedido");
            _pedidos[index] = pedido;
        }
    }
}
