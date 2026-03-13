using Delivery.Domain;
using Delivery.Infrastructure.Interfaces;

namespace Delivery.Infrastructure.Memory
{
    public class ClienteRepositoryMemory : IClienteRepository
    {
        private int _proximoId = 1;
        private readonly List<Cliente> _clientes = new();

        public void AdicionarCliente(Cliente cliente)
        {
            cliente.Id = _proximoId++;
            _clientes.Add(cliente);
        }

        public List<Cliente> ListarClientes()
        {
            return _clientes;
        }

        public List<Cliente> ListarClientesVip()
        {
            return _clientes
                .Where(c => c.Status == Cliente.StatusCliente.Vip)
                .ToList();
        }

        public Cliente? BuscarClienteId(int id)
        {
            return _clientes.FirstOrDefault(c => c.Id == id);
        }

        public Cliente? BuscarClienteCpf(string cpf)
        {
            return _clientes.FirstOrDefault(c => c.Cpf == cpf);
        }

        public void AtualizarCliente(Cliente cliente)
        {
            var index = _clientes.FindIndex(c => c.Id == cliente.Id);
            if (index == -1)
                throw new KeyNotFoundException("Não foi possível atualizar: cliente não encontrado");
            _clientes[index] = cliente;
        }
    }
}