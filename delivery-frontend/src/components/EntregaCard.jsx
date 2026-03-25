import { useState } from "react";
import { EntregaAPI } from "../api";
import "./Card.css";

function EntregaCard({ entrega: inicial }) {
  const [entrega, setEntrega] = useState(inicial);
  const [aberto, setAberto] = useState(false);
  const [acao, setAcao] = useState(null); // "concluir" | "falha" | "cancelar"
  const [obs, setObs] = useState("");
  const [carregando, setCarregando] = useState(null);
  const [msg, setMsg] = useState({ texto: "", tipo: "" });

  const exibirMsg = (texto, tipo = "sucesso") => {
    setMsg({ texto, tipo });
    setTimeout(() => setMsg({ texto: "", tipo: "" }), 3000);
  };

  const atualizarLocal = (dados) => setEntrega((p) => ({ ...p, ...dados }));

  const handleAcao = async (e) => {
    e.preventDefault();
    setCarregando(acao);
    try {
      if (acao === "concluir") {
        await EntregaAPI.concluir(entrega.id, obs);
        atualizarLocal({ status: "Concluida", dataEntrega: new Date().toISOString() });
      } else if (acao === "falha") {
        await EntregaAPI.registrarFalha(entrega.id, obs);
        atualizarLocal({ status: "Falha" });
      } else if (acao === "cancelar") {
        await EntregaAPI.cancelar(entrega.id, obs);
        atualizarLocal({ status: "Cancelada" });
      }
      exibirMsg("Entrega atualizada com sucesso!");
      setAcao(null);
      setObs("");
    } catch (err) {
      exibirMsg(err.message, "erro");
    } finally {
      setCarregando(null);
    }
  };

  const statusColor = {
    Pendente:   "#3b82f6",
    EmAndamento:"#f59e0b",
    Concluida:  "#22c55e",
    Falha:      "#ef4444",
    Cancelada:  "#6b7280",
  };

  const formatarData = (data) =>
    data ? new Date(data).toLocaleDateString("pt-BR", { day: "2-digit", month: "2-digit", year: "numeric", hour: "2-digit", minute: "2-digit" }) : "—";

  const podeAtuar = entrega.status === "EmAndamento";

  return (
    <div className={`card ${aberto ? "aberto" : ""}`}>
      <div className="card-header" onClick={() => { setAberto(!aberto); setAcao(null); }}>
        <div className="card-header-info">
          <div className="card-avatar" style={{ background: "linear-gradient(135deg, #10b981, #06b6d4)" }}>
            🚚
          </div>
          <div>
            <h3 className="card-nome">Entrega #{entrega.id}</h3>
            <span className="card-sub">Pedido #{entrega.pedidoId} · Motorista #{entrega.motoristaId}</span>
          </div>
        </div>
        <div className="card-header-right">
          <span className="status-badge" style={{ backgroundColor: statusColor[entrega.status] || "#6b7280" }}>
            {entrega.status}
          </span>
          <span className="toggle-icon">{aberto ? "▲" : "▼"}</span>
        </div>
      </div>

      {aberto && (
        <div className="card-body">
          {!acao ? (
            <>
              <div className="card-detalhes">
                <div className="detalhe-item"><span className="detalhe-label">ID</span><span className="detalhe-valor">#{entrega.id}</span></div>
                <div className="detalhe-item"><span className="detalhe-label">Veículo</span><span className="detalhe-valor">#{entrega.veiculoId}</span></div>
                <div className="detalhe-item"><span className="detalhe-label">Saída</span><span className="detalhe-valor">{formatarData(entrega.dataSaida)}</span></div>
                <div className="detalhe-item"><span className="detalhe-label">Entrega</span><span className="detalhe-valor">{formatarData(entrega.dataEntrega)}</span></div>
                {entrega.observacoes && (
                  <div className="detalhe-item" style={{ gridColumn: "1 / -1" }}>
                    <span className="detalhe-label">Observações</span>
                    <span className="detalhe-valor">{entrega.observacoes}</span>
                  </div>
                )}
              </div>

              {podeAtuar && (
                <div className="card-acoes">
                  <button className="btn btn-status btn-ativar" onClick={(e) => { e.stopPropagation(); setAcao("concluir"); }}>✅ Concluir</button>
                  <button className="btn btn-status btn-bloquear" onClick={(e) => { e.stopPropagation(); setAcao("falha"); }}>⚠️ Registrar Falha</button>
                  <button className="btn btn-status btn-inativar" onClick={(e) => { e.stopPropagation(); setAcao("cancelar"); }}>✕ Cancelar</button>
                </div>
              )}
            </>
          ) : (
            <form className="form-editar" onSubmit={handleAcao}>
              <h4 className="form-titulo">
                {acao === "concluir" ? "✅ Concluir Entrega" : acao === "falha" ? "⚠️ Registrar Falha" : "✕ Cancelar Entrega"}
              </h4>
              <div className="form-group">
                <label>Observações {acao !== "concluir" && <span style={{ color: "#ef4444" }}>*</span>}</label>
                <input
                  value={obs}
                  onChange={(e) => setObs(e.target.value)}
                  placeholder={acao === "concluir" ? "Opcional..." : "Informe o motivo (obrigatório)"}
                  required={acao !== "concluir"}
                />
              </div>
              <div className="form-acoes">
                <button type="submit" className="btn btn-salvar" disabled={carregando === acao}>{carregando === acao ? "Salvando..." : "Confirmar"}</button>
                <button type="button" className="btn btn-cancelar" onClick={() => { setAcao(null); setObs(""); }}>Voltar</button>
              </div>
            </form>
          )}

          {msg.texto && <div className={`msg ${msg.tipo}`}>{msg.texto}</div>}
        </div>
      )}
    </div>
  );
}

export default EntregaCard;
