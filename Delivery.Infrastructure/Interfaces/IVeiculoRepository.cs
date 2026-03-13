using Delivery.Domain;

namespace Delivery.Infrastructure.Interfaces
{
    public interface IVeiculoRepository
    {
        void AdicionarVeiculo(Veiculo veiculo);
        Veiculo? BuscarVeiculo(int id);
        List<Veiculo> ListarVeiculos();
        List<Veiculo> ListarVeiculoDisponivel();
        void AtualizarVeiculo(Veiculo veiculo);
    }
}