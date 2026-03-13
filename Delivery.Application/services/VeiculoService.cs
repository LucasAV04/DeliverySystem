using Delivery.Domain;
using Delivery.Infrastructure.Interfaces;

namespace Delivery.Application.Services
{
    public class VeiculoService
    {
        private readonly IVeiculoRepository _veiRepo;

        public VeiculoService(IVeiculoRepository veiculoRepo)
        {
            _veiRepo = veiculoRepo;
        }

        public void AdicionarVeiculo(string placa, string modelo, string ano, decimal capacidadeCarga)
        {
            if (string.IsNullOrWhiteSpace(placa) || string.IsNullOrWhiteSpace(modelo) || string.IsNullOrWhiteSpace(ano))
                throw new ArgumentException("Placa, modelo e ano são obrigatórios");

            if (capacidadeCarga <= 0)
                throw new ArgumentException("A capacidade de carga deve ser maior que zero");

            var veiculo = new Veiculo
            {
                Placa = placa,
                Modelo = modelo,
                Ano = ano,
                CapacidadeCarga = capacidadeCarga,
                Status = Veiculo.StatusVeiculo.Disponivel
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
            if (string.IsNullOrWhiteSpace(placa) || string.IsNullOrWhiteSpace(modelo) || string.IsNullOrWhiteSpace(ano))
                throw new ArgumentException("Placa, modelo e ano são obrigatórios");

            if (capacidadeCarga <= 0)
                throw new ArgumentException("A capacidade de carga deve ser maior que zero");

            var veiculo = _veiRepo.BuscarVeiculo(id);
            if (veiculo == null)
                throw new KeyNotFoundException("Veículo não encontrado");

            veiculo.AtualizarDados(placa, modelo, ano, capacidadeCarga);
            _veiRepo.AtualizarVeiculo(veiculo);
        }

        public void ManutencaoVeiculo(int id)
        {
            var veiculo = _veiRepo.BuscarVeiculo(id);
            if (veiculo == null)
                throw new KeyNotFoundException("Veículo não encontrado");

            if (veiculo.Status == Veiculo.StatusVeiculo.EmUso)
                throw new InvalidOperationException("O veículo não pode ir para manutenção pois está em uso");

            veiculo.Status = Veiculo.StatusVeiculo.EmManutencao;
            _veiRepo.AtualizarVeiculo(veiculo);
        }

        public void InativarVeiculo(int id)
        {
            var veiculo = _veiRepo.BuscarVeiculo(id);
            if (veiculo == null)
                throw new KeyNotFoundException("Veículo não encontrado");

            if (veiculo.Status == Veiculo.StatusVeiculo.EmUso)
                throw new InvalidOperationException("O veículo não pode ser inativado pois está em uso");

            veiculo.Status = Veiculo.StatusVeiculo.Inativo;
            _veiRepo.AtualizarVeiculo(veiculo);
        }

        public void AtivarVeiculo(int id)
        {
            var veiculo = _veiRepo.BuscarVeiculo(id);
            if (veiculo == null)
                throw new KeyNotFoundException("Veículo não encontrado");

            if (veiculo.Status == Veiculo.StatusVeiculo.EmUso)
                throw new InvalidOperationException("O veículo já está em uso");

            veiculo.Status = Veiculo.StatusVeiculo.Disponivel;
            _veiRepo.AtualizarVeiculo(veiculo);
        }
    }
}