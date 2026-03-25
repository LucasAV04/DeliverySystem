import { useState } from "react";
import { PedidoAPI } from "../api";
import "./Card.css";

function PedidoCard({ pedido: inicial, onAtualizado }) {
  const [pedido, setPedido] = useState(inicial);
  const [aberto, setAberto] = useState(false);
  const [editando, setEditando] = useState(false);
  const [carregando, setCarregando] = useState(null);
  const [msg, setMsg] = useState({ texto: "", tipo: "" });

  const [form, setForm] = useState({
    clienteId: pedido.clienteId,
    enderecoEntrega: pedido.enderecoEntrega,
  });

  const exibirMsg = (texto, tipo = "sucesso") => {
    setMsg({ texto, tipo });
    setTimeout(() => setMsg({ texto: "", tipo: "" }), 3000);
  };

  const atualizarLocal = (dados) => setPedido((p) => ({ ...p, ...dados }));

  const handleAtualizar = async (e) => {
    e.preventDefault();
    setCarregando("editar");
    try {
      await PedidoAPI.atualizar(pedido.id, form);
      atualizarLocal(form);
      exibirMsg("Pedido atualizado com sucesso!");
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
      await PedidoAPI[acao](pedido.id);
      atualizarLocal({ status: novoStatus });
      exibirMsg(`Pedido: ${novoStatus}!`);
    } catch (err) {
      exibirMsg(err.message, "erro");
    } finally {
      setCarregando(null);
    }
  };

  const statusColor = {
    Criado:          "#3b82f6",
    Confirmado:      "#8b5cf6",
    EmPreparacao:    "#f59e0b",
    ProntoParaEnvio: "#06b6d4",
    EmRota:          "#6366f1",
    Entregue:        "#22c55e",
    Cancelado:       "#ef4444",
  };

  const botoesStatus = {
    Criado:          [{ acao: "confirmar",       label: "✅ Confirmar",         novoStatus: "Confirmado"      },
                      { acao: "cancelar",        label: "✕ Cancelar",           novoStatus: "Cancelado"       }],
    Confirmado:      [{ acao: "emPreparacao",    label: "🍳 Em Preparação",     novoStatus: "EmPreparacao"    },
                      { acao: "cancelar",        label: "✕ Cancelar",           novoStatus: "Cancelado"       }],
    EmPreparacao:    [{ acao: "prontoParaEnvio", label: "📦 Pronto para Envio", novoStatus: "ProntoParaEnvio" },
                      { acao: "cancelar",        label: "✕ Cancelar",           novoStatus: "Cancelado"       }],
    ProntoParaEnvio: [{ acao: "cancelar",        label: "✕ Cancelar",           novoStatus: "Cancelado"       }],
    EmRota:          [],
    Entregue:        [],
    Cancelado:       [],
  };

  const formatarData = (data) =>
    data ? new Date(data).toLocaleDateString("pt-BR", { day: "2-digit", month: "2-digit", year: "numeric", hour: "2-digit", minute: "2-digit" }) : "—";

  return (
    <div className={`card ${aberto ? "aberto" : ""}`}>
      <div className="card-header" onClick={() => { setAberto(!aberto); setEditando(false); }}>
        <div className="card-header-info">
          <div className="card-avatar" style={{ background: "linear-gradient(135deg, #8b5cf6, #6366f1)" }}>
            📦
          </div>
          <div>
            <h3 className="card-nome">Pedido #{pedido.id}</h3>
            <span className="card-sub">{pedido.enderecoEntrega}</span>
          </div>
        </div>
        <div className="card-header-right">
          <span className="status-badge" style={{ backgroundColor: statusColor[pedido.status] || "#6b7280" }}>
            {pedido.status}
          </span>
          <span className="toggle-icon">{aberto ? "▲" : "▼"}</span>
        </div>
      </div>

      {aberto && (
        <div className="card-body">
          {!editando ? (
            <>
              <div className="card-detalhes">
                <div className="detalhe-item"><span className="detalhe-label">ID</span><span className="detalhe-valor">#{pedido.id}</span></div>
                <div className="detalhe-item"><span className="detalhe-label">Cliente ID</span><span className="detalhe-valor">#{pedido.clienteId}</span></div>
                <div className="detalhe-item"><span className="detalhe-label">Data</span><span className="detalhe-valor">{formatarData(pedido.dataSolicitacao)}</span></div>
                <div className="detalhe-item"><span className="detalhe-label">Endereço</span><span className="detalhe-valor">{pedido.enderecoEntrega}</span></div>
              </div>
              <div className="card-acoes">
                {pedido.status === "Criado" || pedido.status === "Confirmado" ? (
                  <button className="btn btn-editar" onClick={(e) => { e.stopPropagation(); setEditando(true); }}>✏️ Editar</button>
                ) : null}
                {(botoesStatus[pedido.status] || []).map(({ acao, label, novoStatus }) => (
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
              <h4 className="form-titulo">Editar Pedido</h4>
              <div className="form-group"><label>ID do Cliente</label><input type="number" value={form.clienteId} onChange={(e) => setForm({ ...form, clienteId: parseInt(e.target.value) })} /></div>
              <div className="form-group"><label>Endereço de Entrega</label><input value={form.enderecoEntrega} onChange={(e) => setForm({ ...form, enderecoEntrega: e.target.value })} placeholder="Rua, número, bairro..." /></div>
              <div className="form-acoes">
                <button type="submit" className="btn btn-salvar" disabled={carregando === "editar"}>{carregando === "editar" ? "Salvando..." : "💾 Salvar"}</button>
                <button type="button" className="btn btn-cancelar" onClick={() => { setEditando(false); setForm({ clienteId: pedido.clienteId, enderecoEntrega: pedido.enderecoEntrega }); }}>Cancelar</button>
              </div>
            </form>
          )}
          {msg.texto && <div className={`msg ${msg.tipo}`}>{msg.texto}</div>}
        </div>
      )}
    </div>
  );
}

export default PedidoCard;
