using Delivery.Domain;

namespace Delivery.Infrastructure.Interfaces
{
    public interface IClienteRepository
    {
        void AdicionarCliente(Cliente cliente);
        List<Cliente> ListarClientes();
        List<Cliente> ListarClientesVip();
        Cliente? BuscarClienteId(int id);
        Cliente? BuscarClienteCpf(string cpf);
        void AtualizarCliente(Cliente cliente);
    }
}