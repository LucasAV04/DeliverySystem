using Delivery.Domain;

namespace Delivery.Infrastructure.Memory
{
    public class MotoristaRepositoryMemory:IMotoristaRepository
    {
        private readonly List<Motorista> _motoristas = new();
        int proximoId = 1;

        public void AdicionarMotorista(Motorista motorista)
        {
            motorista.Id = proximoId++;
            _motoristas.Add(motorista);
        }
        public List<Motorista> ListarMotorista()
        {
            return _motoristas;
        }
        public List<Motorista> ListarMotoristaAtivos()
        {
            return _motoristas.Where(m => m.Status == Motorista.StatusMotorista.Ativo).ToList();
        }
        public Motorista BuscarPorId(int id)
        {
            return _motoristas.FirstOrDefault(m => m.Id == id);
        }
        public void AtualizarMotorista(Motorista motorista)
        {
            var index = _motoristas.FindIndex(m => m.Id == motorista.Id);
            if (index == -1)
                throw new Exception("Não foi possivel Atualizar as informações do Motorista");
            _motoristas[index] = motorista;
        }

    }
}
