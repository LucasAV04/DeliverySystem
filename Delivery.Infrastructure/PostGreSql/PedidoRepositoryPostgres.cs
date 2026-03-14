using Delivery.Domain;
using Delivery.Infrastructure.Data;
using Delivery.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Infrastructure.Repositories
{
    public class PedidoRepositoryPostgres : IPedidoRepository
    {
        private readonly DeliveryDbContext _context;

        public PedidoRepositoryPostgres(DeliveryDbContext context)
        {
            _context = context;
        }

        public void AdicionarPedido(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            _context.SaveChanges();
        }

        public List<Pedido> ListarPedidos()
        {
            return _context.Pedidos.ToList();
        }

        public List<Pedido> ListarPedidosCancelados()
        {
            return _context.Pedidos
                .Where(p => p.Status == Pedido.StatusPedido.Cancelado)
                .ToList();
        }

        public Pedido? BuscarPedido(int id)
        {
            return _context.Pedidos.FirstOrDefault(p => p.Id == id);
        }

        public void AtualizarPedido(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
            _context.SaveChanges();
        }
    }
}
