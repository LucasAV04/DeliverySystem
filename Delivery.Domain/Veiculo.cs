namespace Delivery.Domain;
public class Veiculo 
{
    public int Id { get; set; }
    public string Placa { get; set; }
    public string Modelo { get; set; }
    public string Ano { get; set; }
    public decimal CapacidadeCarga { get; set; }
    public StatusVeiculo Status { get; set; }

        
    public void AtualizarDados(string placa, string modelo, string ano, decimal capacidadeCarga)
    {
        if (Status != StatusVeiculo.Disponivel)
            throw new Exception("O Status do Veiculo tem que estar Ativo para poder ser Atualizado");
        Placa = placa;
        Modelo = modelo;
        Ano = ano;
        CapacidadeCarga = capacidadeCarga;
    }

    public enum StatusVeiculo
    {
       Disponivel,
       EmManutencao,
       EmUso,
       Inativo
    }
}

