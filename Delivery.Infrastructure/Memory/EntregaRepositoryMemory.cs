using Delivery.Domain;
using Delivery.Infrastructure.Interfaces;

namespace Delivery.Infrastructure.Memory
{
    public class EntregaRepositoryMemory : IEntregaRepository
    {
        private readonly List<Entrega> _entregas = new();
        int proximoId = 1;

        public void AdicionarEntrega(Entrega entrega)
        {
            entrega.Id = proximoId++;
            _entregas.Add(entrega);
        }

        public Entrega? BuscarEntrega(int id)
        {
            return _entregas.FirstOrDefault(e => e.Id == id);
        }

        public List<Entrega> ListarEntregas()
        {
            return _entregas;
        }

        public List<Entrega> ListarEntregasPendentes()
        {
            return _entregas
                .Where(e => e.Status == Entrega.StatusEntrega.Pendente)
                .ToList();
        }

        public List<Entrega> ListarEntregasPorMotorista(int motoristaId)
        {
            return _entregas
                .Where(e => e.MotoristaId == motoristaId)
                .ToList();
        }

        public List<Entrega> ListarEntregasPorPeriodo(DateTime inicio, DateTime fim)
        {
            return _entregas
                .Where(e => e.DataSaida >= inicio && e.DataSaida <= fim)
                .ToList();
        }

        public List<Entrega> ListarEntregasPorVeiculo(int veiculoId)
        {
            return _entregas
                .Where(e => e.VeiculoId == veiculoId)
                .ToList();
        }

        public void AtualizarEntrega(Entrega entrega)
        {
            var index = _entregas.FindIndex(e => e.Id == entrega.Id);
            if (index == -1)
                throw new Exception("Não foi possível atualizar as informações da Entrega");
            _entregas[index] = entrega;
        }
    }
}
