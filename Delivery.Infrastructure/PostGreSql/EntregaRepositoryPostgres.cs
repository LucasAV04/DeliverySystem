using Delivery.Domain;
using Delivery.Infrastructure.Data;
using Delivery.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Infrastructure.Repositories
{
    public class EntregaRepositoryPostgres : IEntregaRepository
    {
        private readonly DeliveryDbContext _context;

        public EntregaRepositoryPostgres(DeliveryDbContext context)
        {
            _context = context;
        }

        public void AdicionarEntrega(Entrega entrega)
        {
            _context.Entregas.Add(entrega);
            _context.SaveChanges();
        }

        public Entrega? BuscarEntrega(int id)
        {
            return _context.Entregas.FirstOrDefault(e => e.Id == id);
        }

        public List<Entrega> ListarEntregas()
        {
            return _context.Entregas.ToList();
        }

        public List<Entrega> ListarEntregasPendentes()
        {
            return _context.Entregas
                .Where(e => e.Status == Entrega.StatusEntrega.Pendente)
                .ToList();
        }

        public List<Entrega> ListarEntregasPorMotorista(int motoristaId)
        {
            return _context.Entregas
                .Where(e => e.MotoristaId == motoristaId)
                .ToList();
        }

        public List<Entrega> ListarEntregasPorPeriodo(DateTime inicio, DateTime fim)
        {
            return _context.Entregas
                .Where(e => e.DataSaida >= inicio && e.DataSaida <= fim)
                .ToList();
        }

        public List<Entrega> ListarEntregasPorVeiculo(int veiculoId)
        {
            return _context.Entregas
                .Where(e => e.VeiculoId == veiculoId)
                .ToList();
        }

        public void AtualizarEntrega(Entrega entrega)
        {
            _context.Entregas.Update(entrega);
            _context.SaveChanges();
        }
    }
}
