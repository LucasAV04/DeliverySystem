import { useEffect, useState } from "react";
import { VeiculoAPI } from "../api";
import VeiculoCard from "../components/VeiculoCard";
import "./Lista.css";

const POR_PAGINA = 6;

function ListaVeiculos() {
  const [veiculos, setVeiculos] = useState([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState("");
  const [pagina, setPagina] = useState(1);
  const [busca, setBusca] = useState("");
  const [mostrarForm, setMostrarForm] = useState(false);
  const [form, setForm] = useState({ placa: "", modelo: "", ano: "", capacidadeCarga: "" });
  const [msgForm, setMsgForm] = useState({ texto: "", tipo: "" });
  const [salvando, setSalvando] = useState(false);

  const carregar = () => {
    setCarregando(true);
    VeiculoAPI.listarTodos()
      .then((data) => { setVeiculos(data); setErro(""); })
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
      await VeiculoAPI.adicionar({ ...form, capacidadeCarga: parseFloat(form.capacidadeCarga) });
      exibirMsg("Veículo cadastrado com sucesso!");
      setForm({ placa: "", modelo: "", ano: "", capacidadeCarga: "" });
      setMostrarForm(false);
      carregar();
    } catch (err) {
      exibirMsg(err.message, "erro");
    } finally {
      setSalvando(false);
    }
  };

  const filtrados = veiculos.filter((v) =>
    v.modelo.toLowerCase().includes(busca.toLowerCase()) ||
    v.placa.toLowerCase().includes(busca.toLowerCase())
  );

  const totalPaginas = Math.ceil(filtrados.length / POR_PAGINA);
  const paginaAtual = filtrados.slice((pagina - 1) * POR_PAGINA, pagina * POR_PAGINA);

  return (
    <div className="lista-page">
      <div className="lista-header">
        <div>
          <h2 className="lista-titulo">Veículos</h2>
          <span className="lista-contagem">{filtrados.length} cadastrados</span>
        </div>
        <button className="btn-novo" onClick={() => setMostrarForm(!mostrarForm)}>
          {mostrarForm ? "✕ Cancelar" : "+ Novo Veículo"}
        </button>
      </div>

      {mostrarForm && (
        <form className="form-novo" onSubmit={handleAdicionar}>
          <h3 className="form-novo-titulo">Novo Veículo</h3>
          <div className="form-novo-campos">
            <div className="form-group"><label>Placa</label><input value={form.placa} onChange={(e) => setForm({ ...form, placa: e.target.value })} placeholder="ABC-1234" required /></div>
            <div className="form-group"><label>Modelo</label><input value={form.modelo} onChange={(e) => setForm({ ...form, modelo: e.target.value })} placeholder="Modelo do veículo" required /></div>
            <div className="form-group"><label>Ano</label><input value={form.ano} onChange={(e) => setForm({ ...form, ano: e.target.value })} placeholder="2024" required /></div>
            <div className="form-group"><label>Capacidade de Carga (kg)</label><input type="number" value={form.capacidadeCarga} onChange={(e) => setForm({ ...form, capacidadeCarga: e.target.value })} placeholder="1000" required /></div>
          </div>
          {msgForm.texto && <div className={`msg ${msgForm.tipo}`}>{msgForm.texto}</div>}
          <button type="submit" className="btn-salvar-novo" disabled={salvando}>{salvando ? "Cadastrando..." : "Cadastrar Veículo"}</button>
        </form>
      )}

      <input className="busca-input" value={busca} onChange={(e) => { setBusca(e.target.value); setPagina(1); }} placeholder="🔍 Buscar por modelo ou placa..." />

      {carregando ? <div className="estado-vazio">Carregando veículos...</div>
        : erro ? <div className="estado-erro">❌ {erro}</div>
        : filtrados.length === 0 ? <div className="estado-vazio">{busca ? "Nenhum veículo encontrado." : "Nenhum veículo cadastrado."}</div>
        : (
          <>
            <div className="items-grid">
              {paginaAtual.map((v) => <VeiculoCard key={v.id} veiculo={v} onAtualizado={carregar} />)}
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

export default ListaVeiculos;
