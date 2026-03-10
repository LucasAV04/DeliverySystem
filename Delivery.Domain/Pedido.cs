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
		if (Status == StatusPedido.Cancelado)
			throw new Exception("Pedidos Cancelados não podem ser alterados");
		ClienteId = clienteId;
		EnderecoEntrega = enderecoEntrega;
	}
	public enum StatusPedido 
	{
		Criado,
		Confirmado,
		Cancelado,
		EmPreparacao,
		ProntoParaEnvio
	}

}