using Delivery.Domain;

namespace Delivery.Infrastructure.Interfaces
{
    public interface IMotoristaRepository
    {
        void AdicionarMotorista(Motorista motorista);
        List<Motorista> ListarMotorista();
        List<Motorista> ListarMotoristaAtivos();
        Motorista? BuscarPorId(int id);
        void AtualizarMotorista(Motorista motorista);
    }
}