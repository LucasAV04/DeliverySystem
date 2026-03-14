using Delivery.Domain;
using Delivery.Infrastructure.Data;
using Delivery.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Infrastructure.Repositories
{
    public class ClienteRepositoryPostgres : IClienteRepository
    {
        private readonly DeliveryDbContext _context;

        public ClienteRepositoryPostgres(DeliveryDbContext context)
        {
            _context = context;
        }

        public void AdicionarCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
        }

        public List<Cliente> ListarClientes()
        {
            return _context.Clientes.ToList();
        }

        public List<Cliente> ListarClientesVip()
        {
            return _context.Clientes
                .Where(c => c.Status == Cliente.StatusCliente.Vip)
                .ToList();
        }

        public Cliente? BuscarClienteId(int id)
        {
            return _context.Clientes.FirstOrDefault(c => c.Id == id);
        }

        public Cliente? BuscarClienteCpf(string cpf)
        {
            return _context.Clientes.FirstOrDefault(c => c.Cpf == cpf);
        }

        public void AtualizarCliente(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            _context.SaveChanges();
        }
    }
}
