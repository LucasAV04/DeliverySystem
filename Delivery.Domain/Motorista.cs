namespace Delivery.Domain;

public class Motorista
{
    public int Id { get;  set; }
    public string Nome { get; set; }
    public string Telefone { get;  set; }
    public string Cnh { get;  set; }
    public StatusMotorista Status { get;  set; }


    public void AtualizarDados(string nome,string telefone,string cnh)
    {
        if (Status != StatusMotorista.Ativo)
            throw new Exception("O status do Motorista tem que estar Ativo para poder ser Atualizado");
        Nome = nome;
        Telefone = telefone;
        Cnh = cnh;
    }
    public enum StatusMotorista
    {
        Ativo,
        Inativo,
        EmRota,
        Bloqueado
    } 
}
  