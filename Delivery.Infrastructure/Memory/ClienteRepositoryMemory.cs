using Delivery.Domain;
using Delivery.Infrastructure.Interfaces;
using System.Collections.Immutable;

namespace Delivery.Infrastructure.Memory
{
    public class ClienteRepositoryMemory:IClienteRepository
    {
        int id = 1;
        private readonly List<Cliente> _clientes = new();

        public void AdicionarCliente(Cliente cliente)
        {
            cliente.Id = id++;
            _clientes.Add(cliente);
        }
        public List<Cliente> ListarClientes()
        {
            return _clientes;
        }
        public List<Cliente> ListarClientesVip()
        {
            return _clientes.Where(c => c.Status == Cliente.StatusCliente.Vip).ToList();
        }
        public Cliente BuscarClienteId(int id)
        {
            return _clientes.FirstOrDefault(c => c.Id == id);
        }
        public Cliente BuscarClienteCpf(string cpf)
        {
            return _clientes.Find(c => c.Cpf == cpf);
        }
        public void AtualizarCliente(Cliente cliente)
        {
            var index = _clientes.FindIndex(c => c.Id == cliente.Id);
            if (index == -1)
                throw new Exception("Não foi possivel atualizar os dados");
            _clientes[index] = cliente;
        }

    }
}
