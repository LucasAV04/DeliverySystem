
using Delivery.Domain;

namespace Delivery.Infrastructure.Interfaces
{
    public interface IPedidoRepository
    {
        void AdicionarPedido(Pedido pedido);
        List<Pedido> ListarPedidos();
        List<Pedido> ListarPedidosCancelados();
        Pedido BuscarPedido(int id);
        void AtualizarPedido(Pedido pedido);

    }
}
