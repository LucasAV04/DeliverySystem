import { useEffect, useState } from "react";
import { PedidoAPI } from "../api";
import PedidoCard from "../components/PedidoCard";
import "./Lista.css";

const POR_PAGINA = 6;

function ListaPedidos() {
  const [pedidos, setPedidos] = useState([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState("");
  const [pagina, setPagina] = useState(1);
  const [busca, setBusca] = useState("");
  const [mostrarForm, setMostrarForm] = useState(false);
  const [form, setForm] = useState({ clienteId: "", enderecoEntrega: "" });
  const [msgForm, setMsgForm] = useState({ texto: "", tipo: "" });
  const [salvando, setSalvando] = useState(false);

  const carregar = () => {
    setCarregando(true);
    PedidoAPI.listarTodos()
      .then((data) => { setPedidos(data); setErro(""); })
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
      await PedidoAPI.adicionar({ ...form, clienteId: parseInt(form.clienteId) });
      exibirMsg("Pedido criado com sucesso!");
      setForm({ clienteId: "", enderecoEntrega: "" });
      setMostrarForm(false);
      carregar();
    } catch (err) {
      exibirMsg(err.message, "erro");
    } finally {
      setSalvando(false);
    }
  };

  const filtrados = pedidos.filter((p) =>
    p.enderecoEntrega.toLowerCase().includes(busca.toLowerCase()) ||
    String(p.id).includes(busca) ||
    String(p.clienteId).includes(busca)
  );

  const totalPaginas = Math.ceil(filtrados.length / POR_PAGINA);
  const paginaAtual = filtrados.slice((pagina - 1) * POR_PAGINA, pagina * POR_PAGINA);

  return (
    <div className="lista-page">
      <div className="lista-header">
        <div>
          <h2 className="lista-titulo">Pedidos</h2>
          <span className="lista-contagem">{filtrados.length} registrados</span>
        </div>
        <button className="btn-novo" onClick={() => setMostrarForm(!mostrarForm)}>
          {mostrarForm ? "✕ Cancelar" : "+ Novo Pedido"}
        </button>
      </div>

      {mostrarForm && (
        <form className="form-novo" onSubmit={handleAdicionar}>
          <h3 className="form-novo-titulo">Novo Pedido</h3>
          <div className="form-novo-campos">
            <div className="form-group"><label>ID do Cliente</label><input type="number" value={form.clienteId} onChange={(e) => setForm({ ...form, clienteId: e.target.value })} placeholder="ID do cliente" required /></div>
            <div className="form-group"><label>Endereço de Entrega</label><input value={form.enderecoEntrega} onChange={(e) => setForm({ ...form, enderecoEntrega: e.target.value })} placeholder="Rua, número, bairro..." required /></div>
          </div>
          {msgForm.texto && <div className={`msg ${msgForm.tipo}`}>{msgForm.texto}</div>}
          <button type="submit" className="btn-salvar-novo" disabled={salvando}>{salvando ? "Criando..." : "Criar Pedido"}</button>
        </form>
      )}

      <input className="busca-input" value={busca} onChange={(e) => { setBusca(e.target.value); setPagina(1); }} placeholder="🔍 Buscar por ID, cliente ou endereço..." />

      {carregando ? <div className="estado-vazio">Carregando pedidos...</div>
        : erro ? <div className="estado-erro">❌ {erro}</div>
        : filtrados.length === 0 ? <div className="estado-vazio">{busca ? "Nenhum pedido encontrado." : "Nenhum pedido registrado."}</div>
        : (
          <>
            <div className="items-grid">
              {paginaAtual.map((p) => <PedidoCard key={p.id} pedido={p} onAtualizado={carregar} />)}
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

export default ListaPedidos;
