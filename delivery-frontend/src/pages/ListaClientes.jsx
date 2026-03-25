import { useEffect, useState } from "react";
import { ClienteAPI } from "../api";
import ClienteCard from "../components/ClienteCard";
import "./Lista.css";

const POR_PAGINA = 6;

function ListaClientes() {
  const [clientes, setClientes] = useState([]);
  const [erro, setErro] = useState("");
  const [carregando, setCarregando] = useState(true);
  const [pagina, setPagina] = useState(1);
  const [busca, setBusca] = useState("");
  const [mostrarForm, setMostrarForm] = useState(false);
  const [form, setForm] = useState({ nome: "", cpf: "", email: "" });
  const [msgForm, setMsgForm] = useState({ texto: "", tipo: "" });
  const [salvando, setSalvando] = useState(false);

  const carregar = () => {
    setCarregando(true);
    ClienteAPI.listarTodos()
      .then((data) => { setClientes(data); setErro(""); })
      .catch((err) => setErro(err.message))
      .finally(() => setCarregando(false));
  };

  useEffect(() => { carregar(); }, []);

  const exibirMsg = (texto, tipo = "sucesso") => {
    setMsgForm({ texto, tipo });
    setTimeout(() => setMsgForm({ texto: "", tipo: "" }), 3000);
  };

  const handleAdicionar = async (e) => {
    e.preventDefault();
    setSalvando(true);
    try {
      await ClienteAPI.adicionar(form);
      exibirMsg("Cliente cadastrado com sucesso!");
      setForm({ nome: "", cpf: "", email: "" });
      setMostrarForm(false);
      carregar();
    } catch (err) {
      exibirMsg(err.message, "erro");
    } finally {
      setSalvando(false);
    }
  };

  // Filtro por busca
  const filtrados = clientes.filter(
    (c) =>
      c.nome.toLowerCase().includes(busca.toLowerCase()) ||
      c.cpf.includes(busca) ||
      c.email.toLowerCase().includes(busca.toLowerCase())
  );

  // Paginação
  const totalPaginas = Math.ceil(filtrados.length / POR_PAGINA);
  const inicio = (pagina - 1) * POR_PAGINA;
  const paginaAtual = filtrados.slice(inicio, inicio + POR_PAGINA);

  return (
    <div className="lista-clientes">
      {/* Cabeçalho */}
      <div className="lista-header">
        <div>
          <h2 className="lista-titulo">Clientes</h2>
          <span className="lista-contagem">{filtrados.length} cadastrados</span>
        </div>
        <button
          className="btn-novo"
          onClick={() => setMostrarForm(!mostrarForm)}
        >
          {mostrarForm ? "✕ Cancelar" : "+ Novo Cliente"}
        </button>
      </div>

      {/* Formulário de adicionar */}
      {mostrarForm && (
        <form className="form-novo" onSubmit={handleAdicionar}>
          <h3 className="form-novo-titulo">Novo Cliente</h3>
          <div className="form-novo-campos">
            <div className="form-group">
              <label>Nome</label>
              <input
                value={form.nome}
                onChange={(e) => setForm({ ...form, nome: e.target.value })}
                placeholder="Nome completo"
                required
              />
            </div>
            <div className="form-group">
              <label>CPF</label>
              <input
                value={form.cpf}
                onChange={(e) => setForm({ ...form, cpf: e.target.value })}
                placeholder="000.000.000-00"
                required
              />
            </div>
            <div className="form-group">
              <label>E-mail</label>
              <input
                value={form.email}
                onChange={(e) => setForm({ ...form, email: e.target.value })}
                placeholder="email@exemplo.com"
                required
                type="email"
              />
            </div>
          </div>
          {msgForm.texto && (
            <div className={`msg ${msgForm.tipo}`}>{msgForm.texto}</div>
          )}
          <button type="submit" className="btn-salvar-novo" disabled={salvando}>
            {salvando ? "Cadastrando..." : "Cadastrar Cliente"}
          </button>
        </form>
      )}

      {/* Busca */}
      <div className="busca-container">
        <input
          className="busca-input"
          value={busca}
          onChange={(e) => { setBusca(e.target.value); setPagina(1); }}
          placeholder="🔍 Buscar por nome, CPF ou e-mail..."
        />
      </div>

      {/* Conteúdo */}
      {carregando ? (
        <div className="estado-vazio">Carregando clientes...</div>
      ) : erro ? (
        <div className="estado-erro">❌ {erro}</div>
      ) : filtrados.length === 0 ? (
        <div className="estado-vazio">
          {busca ? "Nenhum cliente encontrado para essa busca." : "Nenhum cliente cadastrado."}
        </div>
      ) : (
        <>
          <div className="clientes-grid">
            {paginaAtual.map((c) => (
              <ClienteCard key={c.id} cliente={c} onAtualizado={carregar} />
            ))}
          </div>

          {/* Paginação */}
          {totalPaginas > 1 && (
            <div className="paginacao">
              <button
                className="pag-btn"
                onClick={() => setPagina(pagina - 1)}
                disabled={pagina === 1}
              >
                ← Anterior
              </button>
              <div className="pag-numeros">
                {Array.from({ length: totalPaginas }, (_, i) => i + 1).map((n) => (
                  <button
                    key={n}
                    className={`pag-num ${n === pagina ? "ativo" : ""}`}
                    onClick={() => setPagina(n)}
                  >
                    {n}
                  </button>
                ))}
              </div>
              <button
                className="pag-btn"
                onClick={() => setPagina(pagina + 1)}
                disabled={pagina === totalPaginas}
              >
                Próxima →
              </button>
            </div>
          )}
        </>
      )}
    </div>
  );
}

export default ListaClientes;