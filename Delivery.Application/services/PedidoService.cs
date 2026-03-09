using Delivery.Domain;

namespace Delivery.Application.services
{
    public class PedidoService
    {
        public void AdicionarPedido(int clienteId,string enderecoEntrega)
        {
            Pedido pedido = new Pedido
            {
                ClienteId = clienteId,
                EnderecoEntrega = enderecoEntrega,
                DataSolicitacao = DateTime.Now,
                Status = Pedido.StatusPedido.Criado
            };
        }

    }
}
