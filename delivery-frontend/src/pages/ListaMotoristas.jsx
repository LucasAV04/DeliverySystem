import { useEffect, useState } from "react";
import { MotoristaAPI } from "../api";
import MotoristaCard from "../components/MotoristaCard";
import "./Lista.css";

const POR_PAGINA = 6;

function ListaMotoristas() {
  const [motoristas, setMotoristas] = useState([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState("");
  const [pagina, setPagina] = useState(1);
  const [busca, setBusca] = useState("");
  const [mostrarForm, setMostrarForm] = useState(false);
  const [form, setForm] = useState({ nome: "", telefone: "", cnh: "" });
  const [msgForm, setMsgForm] = useState({ texto: "", tipo: "" });
  const [salvando, setSalvando] = useState(false);

  const carregar = () => {
    setCarregando(true);
    MotoristaAPI.listarTodos()
      .then((data) => { setMotoristas(data); setErro(""); })
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
      await MotoristaAPI.adicionar(form);
      exibirMsg("Motorista cadastrado com sucesso!");
      setForm({ nome: "", telefone: "", cnh: "" });
      setMostrarForm(false);
      carregar();
    } catch (err) {
      exibirMsg(err.message, "erro");
    } finally {
      setSalvando(false);
    }
  };

  const filtrados = motoristas.filter((m) =>
    m.nome.toLowerCase().includes(busca.toLowerCase()) ||
    m.cnh.includes(busca) ||
    m.telefone.includes(busca)
  );

  const totalPaginas = Math.ceil(filtrados.length / POR_PAGINA);
  const paginaAtual = filtrados.slice((pagina - 1) * POR_PAGINA, pagina * POR_PAGINA);

  return (
    <div className="lista-page">
      <div className="lista-header">
        <div>
          <h2 className="lista-titulo">Motoristas</h2>
          <span className="lista-contagem">{filtrados.length} cadastrados</span>
        </div>
        <button className="btn-novo" onClick={() => setMostrarForm(!mostrarForm)}>
          {mostrarForm ? "✕ Cancelar" : "+ Novo Motorista"}
        </button>
      </div>

      {mostrarForm && (
        <form className="form-novo" onSubmit={handleAdicionar}>
          <h3 className="form-novo-titulo">Novo Motorista</h3>
          <div className="form-novo-campos">
            <div className="form-group"><label>Nome</label><input value={form.nome} onChange={(e) => setForm({ ...form, nome: e.target.value })} placeholder="Nome completo" required /></div>
            <div className="form-group"><label>Telefone</label><input value={form.telefone} onChange={(e) => setForm({ ...form, telefone: e.target.value })} placeholder="(00) 00000-0000" required /></div>
            <div className="form-group"><label>CNH</label><input value={form.cnh} onChange={(e) => setForm({ ...form, cnh: e.target.value })} placeholder="Número da CNH" required /></div>
          </div>
          {msgForm.texto && <div className={`msg ${msgForm.tipo}`}>{msgForm.texto}</div>}
          <button type="submit" className="btn-salvar-novo" disabled={salvando}>{salvando ? "Cadastrando..." : "Cadastrar Motorista"}</button>
        </form>
      )}

      <input className="busca-input" value={busca} onChange={(e) => { setBusca(e.target.value); setPagina(1); }} placeholder="🔍 Buscar por nome, telefone ou CNH..." />

      {carregando ? <div className="estado-vazio">Carregando motoristas...</div>
        : erro ? <div className="estado-erro">❌ {erro}</div>
        : filtrados.length === 0 ? <div className="estado-vazio">{busca ? "Nenhum motorista encontrado." : "Nenhum motorista cadastrado."}</div>
        : (
          <>
            <div className="items-grid">
              {paginaAtual.map((m) => <MotoristaCard key={m.id} motorista={m} onAtualizado={carregar} />)}
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

export default ListaMotoristas;
