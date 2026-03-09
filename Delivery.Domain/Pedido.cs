namespace Delivery.Domain;
public class Pedido
{
	public int Id { get; set; }
	public int ClienteId { get; set; }
	public string EnderecoEntrega { get; set; }
	public DateTime DataSolicitacao { get; set; }
	public StatusPedido Status { get; set; }

	public enum StatusPedido 
	{
		Criado,
		Confirmado,
		Cancelado,
		EmPreparacao,
		ProntoParaEnvio
	}

}