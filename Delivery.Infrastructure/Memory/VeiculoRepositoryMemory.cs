using Delivery.Domain;
using Delivery.Infrastructure.Interfaces;

namespace Delivery.Infrastructure.Memory
{
    public class VeiculoRepositoryMemory : IVeiculoRepository
    {
        private readonly List<Veiculo> _veiculos = new();
        private int _proximoId = 1;

        public void AdicionarVeiculo(Veiculo veiculo)
        {
            veiculo.Id = _proximoId++;
            _veiculos.Add(veiculo);
        }

        public Veiculo? BuscarVeiculo(int id)
        {
            return _veiculos.FirstOrDefault(v => v.Id == id);
        }

        public List<Veiculo> ListarVeiculos()
        {
            return _veiculos;
        }

        public List<Veiculo> ListarVeiculoDisponivel()
        {
            return _veiculos
                .Where(v => v.Status == Veiculo.StatusVeiculo.Disponivel)
                .ToList();
        }

        public void AtualizarVeiculo(Veiculo veiculo)
        {
            var index = _veiculos.FindIndex(v => v.Id == veiculo.Id);
            if (index == -1)
                throw new KeyNotFoundException("Não foi possível atualizar: veículo não encontrado");
            _veiculos[index] = veiculo;
        }
    }
}