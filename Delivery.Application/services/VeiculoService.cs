using Delivery.Domain;
using Delivery.Infrastructure.Interfaces;

namespace Delivery.Application.services
{
    public class VeiculoService
    {
        private readonly IVeiculoRepository _veiRepo;

        public VeiculoService(IVeiculoRepository veiculoRepo)
        {
            _veiRepo = veiculoRepo;
        }

        public void AdicionarVeiculo(string placa,string modelo,string ano,decimal capacidadeCarga)
        {
            if (placa == null || modelo == null || ano == null || capacidadeCarga == 0)
                throw new Exception("Não pode deixar os campos em Branco");
            Veiculo veiculo = new Veiculo
            {
                Placa = placa,
                Modelo = modelo,
                Ano = ano,
                CapacidadeCarga = capacidadeCarga
            };
            _veiRepo.AdicionarVeiculo(veiculo);
        }

        public List<Veiculo> ListarVeiculos()
        {
            return _veiRepo.ListarVeiculos();
        }
        public List<Veiculo> ListarVeiculoDisponivel()
        {
            return _veiRepo.ListarVeiculoDisponivel();
        }

        public void AtualizarVeiculo(int id, string placa, string modelo, string ano, decimal capacidadeCarga)
        {
            var veiculo = _veiRepo.BuscarVeiculo(id);
            if (veiculo == null)
                throw new Exception("Veiculo não Encontrado");
            veiculo.AtualizarDados(placa,modelo, ano,capacidadeCarga);
            _veiRepo.AtualizarVeiculo(veiculo);
        }

        public void ManutencaoVeiculo(int id)
        {
            var veiculo = _veiRepo.BuscarVeiculo(id);
            if (veiculo == null)
                throw new Exception("Veiculo não encontrado");
            veiculo.Status = Veiculo.StatusVeiculo.EmManutencao;
            _veiRepo.AtualizarVeiculo(veiculo);
        }

        public void InativarVeiculo(int id)
        {
            var veiculo = _veiRepo.BuscarVeiculo(id);
            if (veiculo == null)
                throw new Exception("Veiculo não encontrado");

            if (veiculo.Status == Veiculo.StatusVeiculo.EmUso)
                throw new Exception("O Veiculo não pode ser inativado pois está em uso");
            veiculo.Status = Veiculo.StatusVeiculo.Inativo;
            _veiRepo.AtualizarVeiculo(veiculo);
        }
        public void AtivarVeiculo(int id)
        {
            var veiculo = _veiRepo.BuscarVeiculo(id);
            if (veiculo == null)
                throw new Exception("Veiculo não encontrado");
            veiculo.Status = Veiculo.StatusVeiculo.Disponivel;
            _veiRepo.AtualizarVeiculo(veiculo);
        }
    }
}
