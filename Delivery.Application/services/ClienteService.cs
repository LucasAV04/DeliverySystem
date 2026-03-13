using Delivery.Domain;
using Delivery.Infrastructure.Interfaces;

namespace Delivery.Application.Services
{
    public class ClienteService
    {
        private readonly IClienteRepository _cliRepo;

        public ClienteService(IClienteRepository cliRepo)
        {
            _cliRepo = cliRepo;
        }

        public void AdicionarCliente(string nome, string cpf, string email)
        {
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(cpf) || string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Nome, CPF e e-mail são obrigatórios");

            var clienteExistente = _cliRepo.BuscarClienteCpf(cpf);
            if (clienteExistente != null)
                throw new InvalidOperationException("Já existe um cliente cadastrado com esse CPF");

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
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(cpf) || string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Nome, CPF e e-mail são obrigatórios");

            var cliente = _cliRepo.BuscarClienteId(id);
            if (cliente == null)
                throw new KeyNotFoundException("Cliente não encontrado");

            cliente.AtualizarDados(nome, cpf, email);
            _cliRepo.AtualizarCliente(cliente);
        }
    }
}