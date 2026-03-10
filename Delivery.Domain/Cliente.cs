namespace Delivery.Domain
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public StatusCliente Status { get; set; }

        public void AtualizarDados(string nome, string cpf, string email)
        {
            if (Status == StatusCliente.Bloqueado)
                throw new Exception("Clientes bloqueados não podem atualizar os dados");
            Nome = nome;
            Cpf = cpf;
            Email = email;
        }
        public enum StatusCliente
        {
            Ativo,
            Inativo,
            Novo,
            Bloqueado,
            Vip
        }
    }
}
