using Delivery.Domain;
using Delivery.Infrastructure.Data;
using Delivery.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Infrastructure.Repositories
{
    public class MotoristaRepositoryPostgres : IMotoristaRepository
    {
        private readonly DeliveryDbContext _context;

        public MotoristaRepositoryPostgres(DeliveryDbContext context)
        {
            _context = context;
        }

        public void AdicionarMotorista(Motorista motorista)
        {
            _context.Motoristas.Add(motorista);
            _context.SaveChanges();
        }

        public List<Motorista> ListarMotorista()
        {
            return _context.Motoristas.ToList();
        }

        public List<Motorista> ListarMotoristaAtivos()
        {
            return _context.Motoristas
                .Where(m => m.Status == Motorista.StatusMotorista.Ativo)
                .ToList();
        }

        public Motorista? BuscarPorId(int id)
        {
            return _context.Motoristas.FirstOrDefault(m => m.Id == id);
        }

        public void AtualizarMotorista(Motorista motorista)
        {
            _context.Motoristas.Update(motorista);
            _context.SaveChanges();
        }
    }
}
