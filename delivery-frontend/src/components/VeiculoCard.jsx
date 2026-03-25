import { useState } from "react";
import { VeiculoAPI } from "../api";
import "./Card.css";

function VeiculoCard({ veiculo: inicial, onAtualizado }) {
  const [veiculo, setVeiculo] = useState(inicial);
  const [aberto, setAberto] = useState(false);
  const [editando, setEditando] = useState(false);
  const [carregando, setCarregando] = useState(null);
  const [msg, setMsg] = useState({ texto: "", tipo: "" });

  const [form, setForm] = useState({
    placa: veiculo.placa,
    modelo: veiculo.modelo,
    ano: veiculo.ano,
    capacidadeCarga: veiculo.capacidadeCarga,
  });

  const exibirMsg = (texto, tipo = "sucesso") => {
    setMsg({ texto, tipo });
    setTimeout(() => setMsg({ texto: "", tipo: "" }), 3000);
  };

  const atualizarLocal = (dados) => setVeiculo((p) => ({ ...p, ...dados }));

  const handleAtualizar = async (e) => {
    e.preventDefault();
    setCarregando("editar");
    try {
      await VeiculoAPI.atualizar(veiculo.id, form);
      atualizarLocal(form);
      exibirMsg("Veículo atualizado com sucesso!");
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
      await VeiculoAPI[acao](veiculo.id);
      atualizarLocal({ status: novoStatus });
      exibirMsg(`Status alterado para ${novoStatus}!`);
    } catch (err) {
      exibirMsg(err.message, "erro");
    } finally {
      setCarregando(null);
    }
  };

  const statusColor = {
    Disponivel:   "#22c55e",
    EmUso:        "#3b82f6",
    EmManutencao: "#f59e0b",
    Inativo:      "#6b7280",
  };

  const botoesStatus = {
    Disponivel:   [{ acao: "manutencao", label: "🔧 Manutenção", novoStatus: "EmManutencao" },
                   { acao: "inativar",   label: "⏸ Inativar",   novoStatus: "Inativo"      }],
    EmManutencao: [{ acao: "ativar",     label: "✅ Disponível", novoStatus: "Disponivel"   },
                   { acao: "inativar",   label: "⏸ Inativar",   novoStatus: "Inativo"      }],
    Inativo:      [{ acao: "ativar",     label: "✅ Disponível", novoStatus: "Disponivel"   }],
    EmUso:        [],
  };

  return (
    <div className={`card ${aberto ? "aberto" : ""}`}>
      <div className="card-header" onClick={() => { setAberto(!aberto); setEditando(false); }}>
        <div className="card-header-info">
          <div className="card-avatar" style={{ background: "linear-gradient(135deg, #06b6d4, #3b82f6)" }}>
            🚛
          </div>
          <div>
            <h3 className="card-nome">{veiculo.modelo}</h3>
            <span className="card-sub">Placa: {veiculo.placa}</span>
          </div>
        </div>
        <div className="card-header-right">
          <span className="status-badge" style={{ backgroundColor: statusColor[veiculo.status] || "#6b7280" }}>
            {veiculo.status}
          </span>
          <span className="toggle-icon">{aberto ? "▲" : "▼"}</span>
        </div>
      </div>

      {aberto && (
        <div className="card-body">
          {!editando ? (
            <>
              <div className="card-detalhes">
                <div className="detalhe-item"><span className="detalhe-label">ID</span><span className="detalhe-valor">#{veiculo.id}</span></div>
                <div className="detalhe-item"><span className="detalhe-label">Placa</span><span className="detalhe-valor">{veiculo.placa}</span></div>
                <div className="detalhe-item"><span className="detalhe-label">Ano</span><span className="detalhe-valor">{veiculo.ano}</span></div>
                <div className="detalhe-item"><span className="detalhe-label">Carga</span><span className="detalhe-valor">{veiculo.capacidadeCarga} kg</span></div>
              </div>
              <div className="card-acoes">
                <button className="btn btn-editar" onClick={(e) => { e.stopPropagation(); setEditando(true); }}>✏️ Editar</button>
                {(botoesStatus[veiculo.status] || []).map(({ acao, label, novoStatus }) => (
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
              <h4 className="form-titulo">Editar Veículo</h4>
              <div className="form-group"><label>Placa</label><input value={form.placa} onChange={(e) => setForm({ ...form, placa: e.target.value })} placeholder="ABC-1234" /></div>
              <div className="form-group"><label>Modelo</label><input value={form.modelo} onChange={(e) => setForm({ ...form, modelo: e.target.value })} placeholder="Modelo do veículo" /></div>
              <div className="form-group"><label>Ano</label><input value={form.ano} onChange={(e) => setForm({ ...form, ano: e.target.value })} placeholder="2024" /></div>
              <div className="form-group"><label>Capacidade de Carga (kg)</label><input type="number" value={form.capacidadeCarga} onChange={(e) => setForm({ ...form, capacidadeCarga: parseFloat(e.target.value) })} placeholder="1000" /></div>
              <div className="form-acoes">
                <button type="submit" className="btn btn-salvar" disabled={carregando === "editar"}>{carregando === "editar" ? "Salvando..." : "💾 Salvar"}</button>
                <button type="button" className="btn btn-cancelar" onClick={() => { setEditando(false); setForm({ placa: veiculo.placa, modelo: veiculo.modelo, ano: veiculo.ano, capacidadeCarga: veiculo.capacidadeCarga }); }}>Cancelar</button>
              </div>
            </form>
          )}
          {msg.texto && <div className={`msg ${msg.tipo}`}>{msg.texto}</div>}
        </div>
      )}
    </div>
  );
}

export default VeiculoCard;
