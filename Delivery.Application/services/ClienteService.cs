using Delivery.Domain;
using Delivery.Infrastructure.Interfaces;

namespace Delivery.Application.services
{
    public class ClienteService
    {
        private readonly IClienteRepository _cliRepo;

        public ClienteService(IClienteRepository cliRepo)
        {
            _cliRepo = cliRepo;
        }

        public void AdicionarCliente(string nome, string cpf ,string email)
        {
            var clienteCpf = _cliRepo.BuscarClienteCpf(cpf);
            if (clienteCpf != null)
                throw new Exception("Cliente com esse Cpf já foi Cadastrado");
            var cliente = new Cliente
            {
                Nome = nome,
                Cpf = cpf,
                Email = email,
                Status = Cliente.StatusCliente.Novo
            };
            _cliRepo.AdicionarCliente(cliente);
        }
        public List<Cliente> ListarClientes()
        {
            return _cliRepo.ListarClientes();
        }
        public List<Cliente> ListarClientesVip()
        {
            return _cliRepo.ListarClientesVip();
        }
        public void AtualizarCliente(int id, string nome, string cpf, string email)
        {
            var cliente = _cliRepo.BuscarClienteId(id);
            if (cliente == null)
                throw new Exception("Cliente não Encontrado");
            cliente.AtualizarDados(nome, cpf, email);
            _cliRepo.AtualizarCliente(cliente);
        }

    }
}
