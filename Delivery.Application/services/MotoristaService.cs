using Delivery.Domain;
using Delivery.Infrastructure.Interfaces;

namespace Delivery.Application.Services
{
    public class MotoristaService
    {
        private readonly IMotoristaRepository _motRepo;

        public MotoristaService(IMotoristaRepository motRepo)
        {
            _motRepo = motRepo;
        }

        public void AdicionarMotorista(string nome, string telefone, string cnh)
        {
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(telefone) || string.IsNullOrWhiteSpace(cnh))
                throw new ArgumentException("Nome, telefone e CNH são obrigatórios");

            var motorista = new Motorista
            {
                Nome = nome,
                Telefone = telefone,
                Cnh = cnh,
                Status = Motorista.StatusMotorista.Ativo
            };
            _motRepo.AdicionarMotorista(motorista);
        }

        public List<Motorista> ListarMotoristas()
        {
            return _motRepo.ListarMotorista();
        }

        public List<Motorista> ListarMotoristaAtivos()
        {
            return _motRepo.ListarMotoristaAtivos();
        }

        public void BloquearMotorista(int id)
        {
            var motorista = _motRepo.BuscarPorId(id);
            if (motorista == null)
                throw new KeyNotFoundException("Motorista não encontrado");

            if (motorista.Status == Motorista.StatusMotorista.Bloqueado)
                throw new InvalidOperationException("O motorista já está bloqueado");

            motorista.Status = Motorista.StatusMotorista.Bloqueado;
            _motRepo.AtualizarMotorista(motorista);
        }

        public void InativarMotorista(int id)
        {
            var motorista = _motRepo.BuscarPorId(id);
            if (motorista == null)
                throw new KeyNotFoundException("Motorista não encontrado");

            if (motorista.Status == Motorista.StatusMotorista.EmRota)
                throw new InvalidOperationException("O motorista não pode ser inativado pois está em rota");

            motorista.Status = Motorista.StatusMotorista.Inativo;
            _motRepo.AtualizarMotorista(motorista);
        }

        public void AtivarMotorista(int id)
        {
            var motorista = _motRepo.BuscarPorId(id);
            if (motorista == null)
                throw new KeyNotFoundException("Motorista não encontrado");

            if (motorista.Status == Motorista.StatusMotorista.Bloqueado)
                throw new InvalidOperationException("Motorista bloqueado não pode ser ativado diretamente");

            motorista.Status = Motorista.StatusMotorista.Ativo;
            _motRepo.AtualizarMotorista(motorista);
        }

        public void AtualizarMotorista(int id, string nome, string telefone, string cnh)
        {
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(telefone) || string.IsNullOrWhiteSpace(cnh))
                throw new ArgumentException("Nome, telefone e CNH são obrigatórios");

            var motorista = _motRepo.BuscarPorId(id);
            if (motorista == null)
                throw new KeyNotFoundException("Motorista não encontrado");

            motorista.AtualizarDados(nome, telefone, cnh);
            _motRepo.AtualizarMotorista(motorista);
        }
    }
}