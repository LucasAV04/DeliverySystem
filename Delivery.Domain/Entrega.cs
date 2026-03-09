namespace Delivery.Domain;
public class Entrega 
{
    public int Id { get; set; }
    public int PedidoId {  get; set; }
    public int MotoristaId {  get; set; }
    public int VeiculoId { get; set; }
    public string DataSaida { get; set; }
    public string DataEntrega {  get; set; }
    public bool Status {  get; set; }

    public enum StatusEntrega
    {
        Pendenete,
        EmAndamento,
        Concluida,
        Falha,
        Cancelada
    }
}
