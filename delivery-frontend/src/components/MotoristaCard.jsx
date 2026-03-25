import { useState } from "react";
import { MotoristaAPI } from "../api";
import "./Card.css";

function MotoristaCard({ motorista: inicial, onAtualizado }) {
  const [motorista, setMotorista] = useState(inicial);
  const [aberto, setAberto] = useState(false);
  const [editando, setEditando] = useState(false);
  const [carregando, setCarregando] = useState(null);
  const [msg, setMsg] = useState({ texto: "", tipo: "" });

  const [form, setForm] = useState({
    nome: motorista.nome,
    telefone: motorista.telefone,
    cnh: motorista.cnh,
  });

  const exibirMsg = (texto, tipo = "sucesso") => {
    setMsg({ texto, tipo });
    setTimeout(() => setMsg({ texto: "", tipo: "" }), 3000);
  };

  const atualizarLocal = (dados) => setMotorista((p) => ({ ...p, ...dados }));

  const handleAtualizar = async (e) => {
    e.preventDefault();
    setCarregando("editar");
    try {
      await MotoristaAPI.atualizar(motorista.id, form);
      atualizarLocal(form);
      exibirMsg("Motorista atualizado com sucesso!");
      setEditando(false);
    } catch (err) {
      exibirMsg(err.message, "erro");
    } finally {
      setCarregando(null);
    }
  };

  const handleStatus = async (acao, novoStatus) => {
    setCarregando(acao);
    try {
      await MotoristaAPI[acao](motorista.id);
      atualizarLocal({ status: novoStatus });
      exibirMsg(`Status alterado para ${novoStatus}!`);
    } catch (err) {
      exibirMsg(err.message, "erro");
    } finally {
      setCarregando(null);
    }
  };

  const statusColor = {
    Ativo:     "#22c55e",
    Inativo:   "#6b7280",
    EmRota:    "#3b82f6",
    Bloqueado: "#ef4444",
  };

  const botoesStatus = {
    Ativo:     [{ acao: "inativar", label: "⏸ Inativar",  novoStatus: "Inativo"   },
                { acao: "bloquear", label: "🚫 Bloquear", novoStatus: "Bloqueado" }],
    Inativo:   [{ acao: "ativar",   label: "✅ Ativar",   novoStatus: "Ativo"     },
                { acao: "bloquear", label: "🚫 Bloquear", novoStatus: "Bloqueado" }],
    EmRota:    [],
    Bloqueado: [],
  };

  return (
    <div className={`card ${aberto ? "aberto" : ""}`}>
      <div className="card-header" onClick={() => { setAberto(!aberto); setEditando(false); }}>
        <div className="card-header-info">
          <div className="card-avatar" style={{ background: "linear-gradient(135deg, #f59e0b, #ef4444)" }}>
            {motorista.nome.charAt(0).toUpperCase()}
          </div>
          <div>
            <h3 className="card-nome">{motorista.nome}</h3>
            <span className="card-sub">CNH: {motorista.cnh}</span>
          </div>
        </div>
        <div className="card-header-right">
          <span className="status-badge" style={{ backgroundColor: statusColor[motorista.status] || "#6b7280" }}>
            {motorista.status}
          </span>
          <span className="toggle-icon">{aberto ? "▲" : "▼"}</span>
        </div>
      </div>

      {aberto && (
        <div className="card-body">
          {!editando ? (
            <>
              <div className="card-detalhes">
                <div className="detalhe-item"><span className="detalhe-label">ID</span><span className="detalhe-valor">#{motorista.id}</span></div>
                <div className="detalhe-item"><span className="detalhe-label">Telefone</span><span className="detalhe-valor">{motorista.telefone}</span></div>
                <div className="detalhe-item"><span className="detalhe-label">CNH</span><span className="detalhe-valor">{motorista.cnh}</span></div>
                <div className="detalhe-item"><span className="detalhe-label">Status</span><span className="detalhe-valor">{motorista.status}</span></div>
              </div>
              <div className="card-acoes">
                <button className="btn btn-editar" onClick={(e) => { e.stopPropagation(); setEditando(true); }}>✏️ Editar</button>
                {(botoesStatus[motorista.status] || []).map(({ acao, label, novoStatus }) => (
                  <button key={acao} className={`btn btn-status btn-${acao}`}
                    onClick={(e) => { e.stopPropagation(); handleStatus(acao, novoStatus); }}
                    disabled={carregando === acao}>
                    {carregando === acao ? "..." : label}
                  </button>
                ))}
              </div>
            </>
          ) : (
            <form className="form-editar" onSubmit={handleAtualizar}>
              <h4 className="form-titulo">Editar Motorista</h4>
              <div className="form-group"><label>Nome</label><input value={form.nome} onChange={(e) => setForm({ ...form, nome: e.target.value })} placeholder="Nome completo" /></div>
              <div className="form-group"><label>Telefone</label><input value={form.telefone} onChange={(e) => setForm({ ...form, telefone: e.target.value })} placeholder="(00) 00000-0000" /></div>
              <div className="form-group"><label>CNH</label><input value={form.cnh} onChange={(e) => setForm({ ...form, cnh: e.target.value })} placeholder="Número da CNH" /></div>
              <div className="form-acoes">
                <button type="submit" className="btn btn-salvar" disabled={carregando === "editar"}>{carregando === "editar" ? "Salvando..." : "💾 Salvar"}</button>
                <button type="button" className="btn btn-cancelar" onClick={() => { setEditando(false); setForm({ nome: motorista.nome, telefone: motorista.telefone, cnh: motorista.cnh }); }}>Cancelar</button>
              </div>
            </form>
          )}
          {msg.texto && <div className={`msg ${msg.tipo}`}>{msg.texto}</div>}
        </div>
      )}
    </div>
  );
}

export default MotoristaCard;
