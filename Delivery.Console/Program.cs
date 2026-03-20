using Delivery.Application.Services;
using Delivery.Infrastructure.Data;
using Delivery.Infrastructure.Interfaces;
using Delivery.Infrastructure.Memory;
using Delivery.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();


var connectionString = config.GetConnectionString("DefaultConnection");

var options = new DbContextOptionsBuilder<DeliveryDbContext>()
    .UseNpgsql(connectionString)
    .Options;
var context = new DeliveryDbContext(options);
context.Database.Migrate();
IClienteRepository clienteRepo = new ClienteRepositoryPostgres(context);
IMotoristaRepository motoristaRepo = new MotoristaRepositoryPostgres(context);
IVeiculoRepository veiculoRepo = new VeiculoRepositoryPostgres(context);
IPedidoRepository pedidoRepo = new PedidoRepositoryPostgres(context);
IEntregaRepository entregaRepo = new EntregaRepositoryPostgres(context);

var clienteService = new ClienteService(clienteRepo);
var motoristaService = new MotoristaService(motoristaRepo);  
var veiculoService = new VeiculoService(veiculoRepo);
var pedidoService  = new PedidoService(pedidoRepo, clienteRepo);
var entregaService = new EntregaService(entregaRepo, pedidoRepo, motoristaRepo, veiculoRepo);

Console.WriteLine("=== DataFleet — Sistema de Gestão de Entregas ===");

while (true)
{
    Console.WriteLine("\n1- Clientes");
    Console.WriteLine("2- Motoristas");
    Console.WriteLine("3- Veículos");
    Console.WriteLine("4- Pedidos");
    Console.WriteLine("5- Entregas");
    Console.WriteLine("6- Relatórios");
    Console.WriteLine("0- Sair");

    string opcao = Console.ReadLine();
    switch (opcao)
    {
        case "1":
            Console.WriteLine("1- Cadastrar Cliente");
            Console.WriteLine("2- Listar Todos");
            Console.WriteLine("3- Listar VIPs");
            Console.WriteLine("4- Atualizar Cliente");
            string opCli = Console.ReadLine();
            switch (opCli)
            {
                case "1": AdicionarCliente(clienteService); break;
                case "2": ListarClientes(clienteService); break;
                case "3": ListarClientesVip(clienteService); break;
                case "4": AtualizarCliente(clienteService); break;
                default: Console.WriteLine("Opção inválida."); break;
            }
            break;

        case "2":
            Console.WriteLine("1- Cadastrar Motorista");
            Console.WriteLine("2- Listar Todos");
            Console.WriteLine("3- Listar Ativos");
            Console.WriteLine("4- Atualizar Motorista");
            Console.WriteLine("5- Ativar Motorista");
            Console.WriteLine("6- Inativar Motorista");
            Console.WriteLine("7- Bloquear Motorista");
            string opMot = Console.ReadLine();
            switch (opMot)
            {
                case "1": AdicionarMotorista(motoristaService); break;
                case "2": ListarMotoristas(motoristaService); break;
                case "3": ListarMotoristasAtivos(motoristaService); break;
                case "4": AtualizarMotorista(motoristaService); break;
                case "5": AtivarMotorista(motoristaService); break;
                case "6": InativarMotorista(motoristaService); break;
                case "7": BloquearMotorista(motoristaService); break;
                default: Console.WriteLine("Opção inválida."); break;
            }
            break;

        case "3":
            Console.WriteLine("1- Cadastrar Veículo");
            Console.WriteLine("2- Listar Todos");
            Console.WriteLine("3- Listar Disponíveis");
            Console.WriteLine("4- Atualizar Veículo");
            Console.WriteLine("5- Ativar Veículo");
            Console.WriteLine("6- Inativar Veículo");
            Console.WriteLine("7- Enviar para Manutenção");
            string opVei = Console.ReadLine();
            switch (opVei)
            {
                case "1": AdicionarVeiculo(veiculoService); break;
                case "2": ListarVeiculos(veiculoService); break;
                case "3": ListarVeiculosDisponiveis(veiculoService); break;
                case "4": AtualizarVeiculo(veiculoService); break;
                case "5": AtivarVeiculo(veiculoService); break;
                case "6": InativarVeiculo(veiculoService); break;
                case "7": ManutencaoVeiculo(veiculoService); break;
                default: Console.WriteLine("Opção inválida."); break;
            }
            break;

        case "4":
            Console.WriteLine("1- Criar Pedido");
            Console.WriteLine("2- Listar Todos");
            Console.WriteLine("3- Listar Cancelados");
            Console.WriteLine("4- Confirmar Pedido");
            Console.WriteLine("5- Em Preparação");
            Console.WriteLine("6- Pronto para Envio");
            Console.WriteLine("7- Cancelar Pedido");
            Console.WriteLine("8- Atualizar Pedido");
            string opPed = Console.ReadLine();
            switch (opPed)
            {
                case "1": AdicionarPedido(pedidoService); break;
                case "2": ListarPedidos(pedidoService); break;
                case "3": ListarPedidosCancelados(pedidoService); break;
                case "4": ConfirmarPedido(pedidoService); break;
                case "5": EmPreparacao(pedidoService); break;
                case "6": ProntoParaEnvio(pedidoService); break;
                case "7": CancelarPedido(pedidoService); break;
                case "8": AtualizarPedido(pedidoService); break;
                default: Console.WriteLine("Opção inválida."); break;
            }
            break;

        case "5":
            Console.WriteLine("1- Iniciar Entrega");
            Console.WriteLine("2- Concluir Entrega");
            Console.WriteLine("3- Registrar Falha");
            Console.WriteLine("4- Cancelar Entrega");
            Console.WriteLine("5- Listar Todas");
            string opEnt = Console.ReadLine();
            switch (opEnt)
            {
                case "1": IniciarEntrega(entregaService); break;
                case "2": ConcluirEntrega(entregaService); break;
                case "3": RegistrarFalha(entregaService); break;
                case "4": CancelarEntrega(entregaService); break;
                case "5": ListarEntregas(entregaService); break;
                default: Console.WriteLine("Opção inválida."); break;
            }
            break;

        case "6":
            Console.WriteLine("1- Entregas Pendentes");
            Console.WriteLine("2- Entregas por Motorista");
            Console.WriteLine("3- Entregas por Período");
            Console.WriteLine("4- Veículos Mais Utilizados");
            string opRel = Console.ReadLine();
            switch (opRel)
            {
                case "1": ListarEntregasPendentes(entregaService); break;
                case "2": ListarEntregasPorMotorista(entregaService); break;
                case "3": ListarEntregasPorPeriodo(entregaService); break;
                case "4": VeiculosMaisUtilizados(entregaService); break;
                default: Console.WriteLine("Opção inválida."); break;
            }
            break;

        case "0":
            Console.WriteLine("Sistema encerrado. Até logo!");
            return;

        default:
            Console.WriteLine("Opção inválida.");
            break;
    }
}

//======================================================CLIENTE==================================================================
static void AdicionarCliente(ClienteService service)
{
    Console.WriteLine("Digite o Nome:");
    string nome = Console.ReadLine();
    Console.WriteLine("Digite o CPF:");
    string cpf = Console.ReadLine();
    Console.WriteLine("Digite o E-mail:");
    string email = Console.ReadLine();
    service.AdicionarCliente(nome, cpf, email);
    Console.WriteLine("Cliente cadastrado com sucesso!");
}

static void ListarClientes(ClienteService service)
{
    var clientes = service.ListarClientes();
    if (clientes.Count == 0) { Console.WriteLine("Nenhum cliente cadastrado."); return; }
    foreach (var c in clientes)
        Console.WriteLine($"Id: {c.Id} | Nome: {c.Nome} | CPF: {c.Cpf} | E-mail: {c.Email} | Status: {c.Status}");
}

static void ListarClientesVip(ClienteService service)
{
    var vips = service.ListarClientesVip();
    if (vips.Count == 0) { Console.WriteLine("Nenhum cliente VIP cadastrado."); return; }
    foreach (var c in vips)
        Console.WriteLine($"Id: {c.Id} | Nome: {c.Nome} | CPF: {c.Cpf} | E-mail: {c.Email}");
}

static void AtualizarCliente(ClienteService service)
{
    Console.WriteLine("Digite o Id do Cliente:");
    int id = int.Parse(Console.ReadLine());
    Console.WriteLine("Digite o Novo Nome:");
    string nome = Console.ReadLine();
    Console.WriteLine("Digite o Novo CPF:");
    string cpf = Console.ReadLine();
    Console.WriteLine("Digite o Novo E-mail:");
    string email = Console.ReadLine();
    service.AtualizarCliente(id, nome, cpf, email);
    Console.WriteLine("Cliente atualizado com sucesso!");
}

//======================================================MOTORISTA==================================================================
static void AdicionarMotorista(MotoristaService service)
{
    Console.WriteLine("Digite o Nome:");
    string nome = Console.ReadLine();
    Console.WriteLine("Digite o Telefone:");
    string telefone = Console.ReadLine();
    Console.WriteLine("Digite a CNH:");
    string cnh = Console.ReadLine();
    service.AdicionarMotorista(nome, telefone, cnh);
    Console.WriteLine("Motorista cadastrado com sucesso!");
}

static void ListarMotoristas(MotoristaService service)
{
    var motoristas = service.ListarMotoristas();
    if (motoristas.Count == 0) { Console.WriteLine("Nenhum motorista cadastrado."); return; }
    foreach (var m in motoristas)
        Console.WriteLine($"Id: {m.Id} | Nome: {m.Nome} | Telefone: {m.Telefone} | CNH: {m.Cnh} | Status: {m.Status}");
}

static void ListarMotoristasAtivos(MotoristaService service)
{
    var ativos = service.ListarMotoristaAtivos();
    if (ativos.Count == 0) { Console.WriteLine("Nenhum motorista ativo."); return; }
    foreach (var m in ativos)
        Console.WriteLine($"Id: {m.Id} | Nome: {m.Nome} | Telefone: {m.Telefone} | CNH: {m.Cnh}");
}

static void AtualizarMotorista(MotoristaService service)
{
    Console.WriteLine("Digite o Id do Motorista:");
    int id = int.Parse(Console.ReadLine());
    Console.WriteLine("Digite o Novo Nome:");
    string nome = Console.ReadLine();
    Console.WriteLine("Digite o Novo Telefone:");
    string telefone = Console.ReadLine();
    Console.WriteLine("Digite a Nova CNH:");
    string cnh = Console.ReadLine();
    service.AtualizarMotorista(id, nome, telefone, cnh);
    Console.WriteLine("Motorista atualizado com sucesso!");
}

static void AtivarMotorista(MotoristaService service)
{
    Console.WriteLine("Digite o Id do Motorista:");
    int id = int.Parse(Console.ReadLine());
    service.AtivarMotorista(id);
    Console.WriteLine("Motorista ativado com sucesso!");
}

static void InativarMotorista(MotoristaService service)
{
    Console.WriteLine("Digite o Id do Motorista:");
    int id = int.Parse(Console.ReadLine());
    service.InativarMotorista(id);
    Console.WriteLine("Motorista inativado com sucesso!");
}

static void BloquearMotorista(MotoristaService service)
{
    Console.WriteLine("Digite o Id do Motorista:");
    int id = int.Parse(Console.ReadLine());
    service.BloquearMotorista(id);
    Console.WriteLine("Motorista bloqueado com sucesso!");
}

//======================================================VEICULO==================================================================
static void AdicionarVeiculo(VeiculoService service)
{
    Console.WriteLine("Digite a Placa:");
    string placa = Console.ReadLine();
    Console.WriteLine("Digite o Modelo:");
    string modelo = Console.ReadLine();
    Console.WriteLine("Digite o Ano:");
    string ano = Console.ReadLine();
    Console.WriteLine("Digite a Capacidade de Carga (kg):");
    decimal capacidade = decimal.Parse(Console.ReadLine());
    service.AdicionarVeiculo(placa, modelo, ano, capacidade);
    Console.WriteLine("Veículo cadastrado com sucesso!");
}

static void ListarVeiculos(VeiculoService service)
{
    var veiculos = service.ListarVeiculos();
    if (veiculos.Count == 0) { Console.WriteLine("Nenhum veículo cadastrado."); return; }
    foreach (var v in veiculos)
        Console.WriteLine($"Id: {v.Id} | Modelo: {v.Modelo} | Placa: {v.Placa} | Ano: {v.Ano} | Carga: {v.CapacidadeCarga}kg | Status: {v.Status}");
}

static void ListarVeiculosDisponiveis(VeiculoService service)
{
    var disponiveis = service.ListarVeiculoDisponivel();
    if (disponiveis.Count == 0) { Console.WriteLine("Nenhum veículo disponível."); return; }
    foreach (var v in disponiveis)
        Console.WriteLine($"Id: {v.Id} | Modelo: {v.Modelo} | Placa: {v.Placa} | Ano: {v.Ano} | Carga: {v.CapacidadeCarga}kg");
}

static void AtualizarVeiculo(VeiculoService service)
{
    Console.WriteLine("Digite o Id do Veículo:");
    int id = int.Parse(Console.ReadLine());
    Console.WriteLine("Digite a Nova Placa:");
    string placa = Console.ReadLine();
    Console.WriteLine("Digite o Novo Modelo:");
    string modelo = Console.ReadLine();
    Console.WriteLine("Digite o Novo Ano:");
    string ano = Console.ReadLine();
    Console.WriteLine("Digite a Nova Capacidade de Carga (kg):");
    decimal capacidade = decimal.Parse(Console.ReadLine());
    service.AtualizarVeiculo(id, placa, modelo, ano, capacidade);
    Console.WriteLine("Veículo atualizado com sucesso!");
}

static void AtivarVeiculo(VeiculoService service)
{
    Console.WriteLine("Digite o Id do Veículo:");
    int id = int.Parse(Console.ReadLine());
    service.AtivarVeiculo(id);
    Console.WriteLine("Veículo ativado com sucesso!");
}

static void InativarVeiculo(VeiculoService service)
{
    Console.WriteLine("Digite o Id do Veículo:");
    int id = int.Parse(Console.ReadLine());
    service.InativarVeiculo(id);
    Console.WriteLine("Veículo inativado com sucesso!");
}

static void ManutencaoVeiculo(VeiculoService service)
{
    Console.WriteLine("Digite o Id do Veículo:");
    int id = int.Parse(Console.ReadLine());
    service.ManutencaoVeiculo(id);
    Console.WriteLine("Veículo enviado para manutenção!");
}

//======================================================PEDIDO==================================================================
static void AdicionarPedido(PedidoService service)
{
    Console.WriteLine("Digite o Id do Cliente:");
    int clienteId = int.Parse(Console.ReadLine());
    Console.WriteLine("Digite o Endereço de Entrega:");
    string endereco = Console.ReadLine();
    service.AdicionarPedido(clienteId, endereco);
    Console.WriteLine("Pedido criado com sucesso!");
}

static void ListarPedidos(PedidoService service)
{
    var pedidos = service.ListarPedidos();
    if (pedidos.Count == 0) { Console.WriteLine("Nenhum pedido cadastrado."); return; }
    foreach (var p in pedidos)
        Console.WriteLine($"Id: {p.Id} | Cliente Id: {p.ClienteId} | Endereço: {p.EnderecoEntrega} | Status: {p.Status} | Data: {p.DataSolicitacao:Dia:dd HH:mm}");
}

static void ListarPedidosCancelados(PedidoService service)
{
    var cancelados = service.ListarPedidosCancelados();
    if (cancelados.Count == 0) { Console.WriteLine("Nenhum pedido cancelado."); return; }
    foreach (var p in cancelados)
        Console.WriteLine($"Id: {p.Id} | Cliente Id: {p.ClienteId} | Endereço: {p.EnderecoEntrega} | Data: {p.DataSolicitacao:Dia:dd HH:mm}");
}

static void ConfirmarPedido(PedidoService service)
{
    Console.WriteLine("Digite o Id do Pedido:");
    int id = int.Parse(Console.ReadLine());
    service.ConfirmarPedido(id);
    Console.WriteLine("Pedido confirmado!");
}

static void EmPreparacao(PedidoService service)
{
    Console.WriteLine("Digite o Id do Pedido:");
    int id = int.Parse(Console.ReadLine());
    service.EmPreparacao(id);
    Console.WriteLine("Pedido em preparação!");
}

static void ProntoParaEnvio(PedidoService service)
{
    Console.WriteLine("Digite o Id do Pedido:");
    int id = int.Parse(Console.ReadLine());
    service.ProntoParaEnvio(id);
    Console.WriteLine("Pedido pronto para envio!");
}

static void CancelarPedido(PedidoService service)
{
    Console.WriteLine("Digite o Id do Pedido:");
    int id = int.Parse(Console.ReadLine());
    service.CancelarPedido(id);
    Console.WriteLine("Pedido cancelado!");
}

static void AtualizarPedido(PedidoService service)
{
    Console.WriteLine("Digite o Id do Pedido:");
    int id = int.Parse(Console.ReadLine());
    Console.WriteLine("Digite o Novo Id do Cliente:");
    int clienteId = int.Parse(Console.ReadLine());
    Console.WriteLine("Digite o Novo Endereço de Entrega:");
    string endereco = Console.ReadLine();
    service.AtualizarPedido(id, clienteId, endereco);
    Console.WriteLine("Pedido atualizado com sucesso!");
}

//======================================================ENTREGA==================================================================
static void IniciarEntrega(EntregaService service)
{
    Console.WriteLine("Digite o Id do Pedido:");
    int pedidoId = int.Parse(Console.ReadLine());
    Console.WriteLine("Digite o Id do Motorista:");
    int motoristaId = int.Parse(Console.ReadLine());
    Console.WriteLine("Digite o Id do Veículo:");
    int veiculoId = int.Parse(Console.ReadLine());
    service.IniciarEntrega(pedidoId, motoristaId, veiculoId);
    Console.WriteLine("Entrega iniciada com sucesso!");
}

static void ConcluirEntrega(EntregaService service)
{
    Console.WriteLine("Digite o Id da Entrega:");
    int id = int.Parse(Console.ReadLine());
    Console.WriteLine("Observações (deixe em branco se não houver):");
    string obs = Console.ReadLine();
    service.ConcluirEntrega(id, string.IsNullOrWhiteSpace(obs) ? null : obs);
    Console.WriteLine("Entrega concluída com sucesso!");
}

static void RegistrarFalha(EntregaService service)
{
    Console.WriteLine("Digite o Id da Entrega:");
    int id = int.Parse(Console.ReadLine());
    Console.WriteLine("Digite o Motivo da Falha:");
    string motivo = Console.ReadLine();
    service.RegistrarFalha(id, motivo);
    Console.WriteLine("Falha registrada.");
}

static void CancelarEntrega(EntregaService service)
{
    Console.WriteLine("Digite o Id da Entrega:");
    int id = int.Parse(Console.ReadLine());
    Console.WriteLine("Digite o Motivo do Cancelamento:");
    string motivo = Console.ReadLine();
    service.CancelarEntrega(id, motivo);
    Console.WriteLine("Entrega cancelada.");
}

static void ListarEntregas(EntregaService service)
{
    var entregas = service.ListarEntregas();
    if (entregas.Count == 0) { Console.WriteLine("Nenhuma entrega registrada."); return; }
    foreach (var e in entregas)
        Console.WriteLine($"Id: {e.Id} | Pedido: {e.PedidoId} | Motorista: {e.MotoristaId} | Veículo: {e.VeiculoId} | Status: {e.Status} | Saída: {e.DataSaida:Dia:dd HH:mm} | Entrega: {(e.DataEntrega.HasValue ? e.DataEntrega.Value.ToString("dd/MM/yyyy HH:mm") : "—")}");
}


static void ListarEntregasPendentes(EntregaService service)
{
    var pendentes = service.ListarEntregasPendentes();
    if (pendentes.Count == 0) { Console.WriteLine("Nenhuma entrega pendente."); return; }
    foreach (var e in pendentes)
        Console.WriteLine($"Id: {e.Id} | Pedido: {e.PedidoId} | Motorista: {e.MotoristaId} | Saída: {e.DataSaida:dd/MM/yyyy HH:mm}");
}

static void ListarEntregasPorMotorista(EntregaService service)
{
    Console.WriteLine("Digite o Id do Motorista:");
    int id = int.Parse(Console.ReadLine());
    var entregas = service.ListarEntregasPorMotorista(id);
    if (entregas.Count == 0) { Console.WriteLine("Nenhuma entrega encontrada para este motorista."); return; }
    foreach (var e in entregas)
        Console.WriteLine($"Id: {e.Id} | Pedido: {e.PedidoId} | Veículo: {e.VeiculoId} | Status: {e.Status} | Saída: {e.DataSaida:dd/MM/yyyy HH:mm}");
}

static void ListarEntregasPorPeriodo(EntregaService service)
{
    Console.WriteLine("Digite a Data de Início (dd/MM/yyyy):");
    DateTime inicio = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", null);
    Console.WriteLine("Digite a Data de Fim (dd/MM/yyyy):");
    DateTime fim = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", null);
    var entregas = service.ListarEntregasPorPeriodo(inicio, fim.AddDays(1).AddSeconds(-1));
    if (entregas.Count == 0) { Console.WriteLine("Nenhuma entrega encontrada nesse período."); return; }
    foreach (var e in entregas)
        Console.WriteLine($"Id: {e.Id} | Pedido: {e.PedidoId} | Motorista: {e.MotoristaId} | Status: {e.Status} | Saída: {e.DataSaida:dd/MM/yyyy HH:mm}");
}

static void VeiculosMaisUtilizados(EntregaService service)
{
    var ranking = service.VeiculosMaisUtilizados();
    if (ranking.Count == 0) { Console.WriteLine("Nenhuma entrega registrada."); return; }
    int pos = 1;
    foreach (var (veiculoId, total) in ranking)
        Console.WriteLine($"{pos++}º - Veículo Id: {veiculoId} | Total de Entregas: {total}");
}
