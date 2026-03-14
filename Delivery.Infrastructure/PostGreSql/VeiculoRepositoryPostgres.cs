using Delivery.Domain;
using Delivery.Infrastructure.Data;
using Delivery.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Infrastructure.Repositories
{
    public class VeiculoRepositoryPostgres : IVeiculoRepository
    {
        private readonly DeliveryDbContext _context;

        public VeiculoRepositoryPostgres(DeliveryDbContext context)
        {
            _context = context;
        }

        public void AdicionarVeiculo(Veiculo veiculo)
        {
            _context.Veiculos.Add(veiculo);
            _context.SaveChanges();
        }

        public Veiculo? BuscarVeiculo(int id)
        {
            return _context.Veiculos.FirstOrDefault(v => v.Id == id);
        }

        public List<Veiculo> ListarVeiculos()
        {
            return _context.Veiculos.ToList();
        }

        public List<Veiculo> ListarVeiculoDisponivel()
        {
            return _context.Veiculos
                .Where(v => v.Status == Veiculo.StatusVeiculo.Disponivel)
                .ToList();
        }

        public void AtualizarVeiculo(Veiculo veiculo)
        {
            _context.Veiculos.Update(veiculo);
            _context.SaveChanges();
        }
    }
}
