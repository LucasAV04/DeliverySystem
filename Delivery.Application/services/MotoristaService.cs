
using Delivery.Domain;

namespace Delivery.Application.services
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
            if (nome == null || telefone == null || cnh == null)
                throw new Exception("Não pode deixar os campos em Branco");

            Motorista motorista = new Motorista
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
                throw new Exception("Motorista não Encontrado");
            motorista.Status = Motorista.StatusMotorista.Bloqueado;
            _motRepo.AtualizarMotorista(motorista);
        }
        public void InativarMotorista(int id)
        {
            var motorista = _motRepo.BuscarPorId(id);
            if (motorista == null)
                throw new Exception("Motorista não Encontrado");

            if (motorista.Status == Motorista.StatusMotorista.EmRota)
                throw new Exception("O Motorista não pode ser inativado pois está em rota");
            motorista.Status = Motorista.StatusMotorista.Inativo;
            _motRepo.AtualizarMotorista(motorista);
        }

        public void AtivarMotorista(int id)
        {
            var motorista = _motRepo.BuscarPorId(id);
            if (motorista == null)
                throw new Exception("Motorista não Encontrado");
            if (motorista.Status == Motorista.StatusMotorista.Bloqueado)
                throw new Exception("Motorista Bloqueado não pode ser ativado");
            motorista.Status = Motorista.StatusMotorista.Ativo;
            _motRepo.AtualizarMotorista(motorista);
        }
            
        public void AtualizarMotorista(int id, string nome, string telefone, string cnh)
        {
            var motorista = _motRepo.BuscarPorId(id);
            if (motorista == null)
                throw new Exception("Motorista não Encontrado");
            motorista.AtualizarDados(nome, telefone, cnh);
            _motRepo.AtualizarMotorista(motorista);
        }
    }
}
