using Delivery.Domain;

namespace Delivery.Infrastructure.Interfaces
{
    public interface IEntregaRepository
    {
        void AdicionarEntrega(Entrega entrega);
        Entrega? BuscarEntrega(int id);
        List<Entrega> ListarEntregas();
        List<Entrega> ListarEntregasPendentes();
        List<Entrega> ListarEntregasPorMotorista(int motoristaId);
        List<Entrega> ListarEntregasPorPeriodo(DateTime inicio, DateTime fim);
        List<Entrega> ListarEntregasPorVeiculo(int veiculoId);
        void AtualizarEntrega(Entrega entrega);
    }
}