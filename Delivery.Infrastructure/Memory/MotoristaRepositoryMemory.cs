using Delivery.Domain;
using Delivery.Infrastructure.Interfaces;

namespace Delivery.Infrastructure.Memory
{
    public class MotoristaRepositoryMemory : IMotoristaRepository
    {
        private readonly List<Motorista> _motoristas = new();
        private int _proximoId = 1;

        public void AdicionarMotorista(Motorista motorista)
        {
            motorista.Id = _proximoId++;
            _motoristas.Add(motorista);
        }

        public List<Motorista> ListarMotorista()
        {
            return _motoristas;
        }

        public List<Motorista> ListarMotoristaAtivos()
        {
            return _motoristas
                .Where(m => m.Status == Motorista.StatusMotorista.Ativo)
                .ToList();
        }

        public Motorista? BuscarPorId(int id)
        {
            return _motoristas.FirstOrDefault(m => m.Id == id);
        }

        public void AtualizarMotorista(Motorista motorista)
        {
            var index = _motoristas.FindIndex(m => m.Id == motorista.Id);
            if (index == -1)
                throw new KeyNotFoundException("Não foi possível atualizar: motorista não encontrado");
            _motoristas[index] = motorista;
        }
    }
}