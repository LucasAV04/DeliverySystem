using Delivery.Domain;

public interface IMotoristaRepository
{
	void AdicionarMotorista(Motorista motorista);
	List<Motorista> ListarMotorista();
	List<Motorista> ListarMotoristaAtivos();
	Motorista BuscarPorId(int id);
	void AtualizarMotorista(Motorista motorista);
	
}