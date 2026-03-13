namespace Delivery.Domain;
public class Entrega 
{
    public int Id { get; set; }
    public int PedidoId {  get; set; }
    public int MotoristaId {  get; set; }
    public int VeiculoId { get; set; }
    public DateTime DataSaida { get; set; }
    public DateTime? DataEntrega {  get; set; }
    public string? Observacoes { get; set; }
    public StatusEntrega Status {  get; set; }

    public enum StatusEntrega
    {
        Pendente,
        EmAndamento,
        Concluida,
        Falha,
        Cancelada
    }
}
