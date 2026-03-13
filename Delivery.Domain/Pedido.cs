namespace Delivery.Domain;
public class Pedido
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string EnderecoEntrega { get; set; }
    public DateTime DataSolicitacao { get; set; }
    public StatusPedido Status { get; set; }

    public void AtualizarDados(int clienteId, string enderecoEntrega)
    {
        if (Status != StatusPedido.Criado && Status != StatusPedido.Confirmado)
            throw new Exception("O pedido só pode ser atualizado nos status 'Criado' ou 'Confirmado'");

        ClienteId = clienteId;
        EnderecoEntrega = enderecoEntrega;
    }

    public enum StatusPedido
    {
        Criado,
        Confirmado,
        EmPreparacao,
        ProntoParaEnvio,
        EmRota,
        Entregue,
        Cancelado
    }
}