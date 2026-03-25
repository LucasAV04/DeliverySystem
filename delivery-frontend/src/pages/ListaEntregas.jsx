import { useEffect, useState } from "react";
import { EntregaAPI } from "../api";
import EntregaCard from "../components/EntregaCard";
import "./Lista.css";

const POR_PAGINA = 6;

function ListaEntregas() {
  const [entregas, setEntregas] = useState([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState("");
  const [pagina, setPagina] = useState(1);
  const [busca, setBusca] = useState("");
  const [mostrarForm, setMostrarForm] = useState(false);
  const [form, setForm] = useState({ pedidoId: "", motoristaId: "", veiculoId: "" });
  const [msgForm, setMsgForm] = useState({ texto: "", tipo: "" });
  const [salvando, setSalvando] = useState(false);

  const carregar = () => {
    setCarregando(true);
    EntregaAPI.listarTodas()
      .then((data) => { setEntregas(data); setErro(""); })
      .catch((err) => setErro(err.message))
      .finally(() => setCarregando(false));
  };

  useEffect(() => { carregar(); }, []);

  const exibirMsg = (texto, tipo = "sucesso") => {
    setMsgForm({ texto, tipo });
    setTimeout(() => setMsgForm({ texto: "", tipo: "" }), 3000);
  };

  const handleIniciar = async (e) => {
    e.preventDefault();
    setSalvando(true);
    try {
      await EntregaAPI.iniciar({
        pedidoId: parseInt(form.pedidoId),
        motoristaId: parseInt(form.motoristaId),
        veiculoId: parseInt(form.veiculoId),
      });
      exibirMsg("Entrega iniciada com sucesso!");
      setForm({ pedidoId: "", motoristaId: "", veiculoId: "" });
      setMostrarForm(false);
      carregar();
    } catch (err) {
      exibirMsg(err.message, "erro");
    } finally {
      setSalvando(false);
    }
  };

  const filtrados = entregas.filter((e) =>
    String(e.id).includes(busca) ||
    String(e.pedidoId).includes(busca) ||
    String(e.motoristaId).includes(busca) ||
    e.status.toLowerCase().includes(busca.toLowerCase())
  );

  const totalPaginas = Math.ceil(filtrados.length / POR_PAGINA);
  const paginaAtual = filtrados.slice((pagina - 1) * POR_PAGINA, pagina * POR_PAGINA);

  return (
    <div className="lista-page">
      <div className="lista-header">
        <div>
          <h2 className="lista-titulo">Entregas</h2>
          <span className="lista-contagem">{filtrados.length} registradas</span>
        </div>
        <button className="btn-novo" onClick={() => setMostrarForm(!mostrarForm)}>
          {mostrarForm ? "✕ Cancelar" : "+ Iniciar Entrega"}
        </button>
      </div>

      {mostrarForm && (
        <form className="form-novo" onSubmit={handleIniciar}>
          <h3 className="form-novo-titulo">Iniciar Entrega</h3>
          <div className="form-novo-campos">
            <div className="form-group"><label>ID do Pedido</label><input type="number" value={form.pedidoId} onChange={(e) => setForm({ ...form, pedidoId: e.target.value })} placeholder="ID do pedido" required /></div>
            <div className="form-group"><label>ID do Motorista</label><input type="number" value={form.motoristaId} onChange={(e) => setForm({ ...form, motoristaId: e.target.value })} placeholder="ID do motorista" required /></div>
            <div className="form-group"><label>ID do Veículo</label><input type="number" value={form.veiculoId} onChange={(e) => setForm({ ...form, veiculoId: e.target.value })} placeholder="ID do veículo" required /></div>
          </div>
          {msgForm.texto && <div className={`msg ${msgForm.tipo}`}>{msgForm.texto}</div>}
          <button type="submit" className="btn-salvar-novo" disabled={salvando}>{salvando ? "Iniciando..." : "Iniciar Entrega"}</button>
        </form>
      )}

      <input className="busca-input" value={busca} onChange={(e) => { setBusca(e.target.value); setPagina(1); }} placeholder="🔍 Buscar por ID, pedido, motorista ou status..." />

      {carregando ? <div className="estado-vazio">Carregando entregas...</div>
        : erro ? <div className="estado-erro">❌ {erro}</div>
        : filtrados.length === 0 ? <div className="estado-vazio">{busca ? "Nenhuma entrega encontrada." : "Nenhuma entrega registrada."}</div>
        : (
          <>
            <div className="items-grid">
              {paginaAtual.map((e) => <EntregaCard key={e.id} entrega={e} />)}
            </div>
            {totalPaginas > 1 && (
              <div className="paginacao">
                <button className="pag-btn" onClick={() => setPagina(pagina - 1)} disabled={pagina === 1}>← Anterior</button>
                <div className="pag-numeros">
                  {Array.from({ length: totalPaginas }, (_, i) => i + 1).map((n) => (
                    <button key={n} className={`pag-num ${n === pagina ? "ativo" : ""}`} onClick={() => setPagina(n)}>{n}</button>
                  ))}
                </div>
                <button className="pag-btn" onClick={() => setPagina(pagina + 1)} disabled={pagina === totalPaginas}>Próxima →</button>
              </div>
            )}
          </>
        )}
    </div>
  );
}

export default ListaEntregas;
