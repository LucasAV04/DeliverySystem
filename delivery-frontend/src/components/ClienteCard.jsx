import { useState } from "react";
import { ClienteAPI } from "../api";
import "./Card.css";

// Dados locais do cliente ficam aqui para atualizar sem precisar recarregar
function ClienteCard({ cliente: clienteInicial, onAtualizado }) {
  const [cliente, setCliente] = useState(clienteInicial);
  const [aberto, setAberto] = useState(false);
  const [editando, setEditando] = useState(false);
  const [msg, setMsg] = useState({ texto: "", tipo: "" });
  const [carregando, setCarregando] = useState(null); // guarda qual ação está carregando

  const [form, setForm] = useState({
    nome: cliente.nome,
    cpf: cliente.cpf,
    email: cliente.email,
  });

  const exibirMsg = (texto, tipo = "sucesso") => {
    setMsg({ texto, tipo });
    setTimeout(() => setMsg({ texto: "", tipo: "" }), 3000);
  };

  // Atualiza dados locais imediatamente sem precisar recarregar a lista
  const atualizarLocal = (novosDados) => {
    setCliente((prev) => ({ ...prev, ...novosDados }));
  };

  const handleAtualizar = async (e) => {
    e.preventDefault();
    setCarregando("editar");
    try {
      await ClienteAPI.atualizar(cliente.id, form);
      atualizarLocal(form); // atualiza localmente sem precisar recarregar
      exibirMsg("Cliente atualizado com sucesso!");
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
      await ClienteAPI[acao](cliente.id);
      atualizarLocal({ status: novoStatus }); // atualiza o badge imediatamente
      exibirMsg(`Status alterado para ${novoStatus}!`);
    } catch (err) {
      exibirMsg(err.message, "erro");
    } finally {
      setCarregando(null);
    }
  };

  const statusColor = {
    Novo: "#3b82f6",
    Ativo: "#22c55e",
    Inativo: "#6b7280",
    Bloqueado: "#ef4444",
    Vip: "#f59e0b",
  };

  // Quais botões de status mostrar dependendo do status atual
  const botoesStatus = {
    Novo:      [{ acao: "ativar",   label: "✅ Ativar",   novoStatus: "Ativo"    },
                { acao: "bloquear", label: "🚫 Bloquear", novoStatus: "Bloqueado"}],
    Ativo:     [{ acao: "darVip",   label: "⭐ Dar VIP",  novoStatus: "Vip"      },
                { acao: "inativar", label: "⏸ Inativar",  novoStatus: "Inativo"  },
                { acao: "bloquear", label: "🚫 Bloquear", novoStatus: "Bloqueado"}],
    Inativo:   [{ acao: "ativar",   label: "✅ Ativar",   novoStatus: "Ativo"    },
                { acao: "bloquear", label: "🚫 Bloquear", novoStatus: "Bloqueado"}],
    Bloqueado: [],
    Vip:       [{ acao: "inativar", label: "⏸ Inativar",  novoStatus: "Inativo"  },
                { acao: "bloquear", label: "🚫 Bloquear", novoStatus: "Bloqueado"}],
  };

  return (
    <div className={`cliente-card ${aberto ? "aberto" : ""}`}>
      <div
        className="card-header"
        onClick={() => { setAberto(!aberto); setEditando(false); }}
      >
        <div className="card-header-info">
          <div className="cliente-avatar">
            {cliente.nome.charAt(0).toUpperCase()}
          </div>
          <div>
            <h3 className="cliente-nome">{cliente.nome}</h3>
            <span className="cliente-email">{cliente.email}</span>
          </div>
        </div>
        <div className="card-header-right">
          <span
            className="status-badge"
            style={{ backgroundColor: statusColor[cliente.status] || "#6b7280" }}
          >
            {cliente.status}
          </span>
          <span className="toggle-icon">{aberto ? "▲" : "▼"}</span>
        </div>
      </div>

      {aberto && (
        <div className="card-body">
          {!editando ? (
            <>
              <div className="card-detalhes">
                <div className="detalhe-item">
                  <span className="detalhe-label">ID</span>
                  <span className="detalhe-valor">#{cliente.id}</span>
                </div>
                <div className="detalhe-item">
                  <span className="detalhe-label">CPF</span>
                  <span className="detalhe-valor">{cliente.cpf}</span>
                </div>
                <div className="detalhe-item">
                  <span className="detalhe-label">E-mail</span>
                  <span className="detalhe-valor">{cliente.email}</span>
                </div>
                <div className="detalhe-item">
                  <span className="detalhe-label">Status</span>
                  <span className="detalhe-valor">{cliente.status}</span>
                </div>
              </div>

              <div className="card-acoes">
                <button
                  className="btn btn-editar"
                  onClick={(e) => { e.stopPropagation(); setEditando(true); }}
                >
                  ✏️ Editar
                </button>

                {(botoesStatus[cliente.status] || []).map(({ acao, label, novoStatus }) => (
                  <button
                    key={acao}
                    className={`btn btn-status btn-${acao}`}
                    onClick={(e) => { e.stopPropagation(); handleStatus(acao, novoStatus); }}
                    disabled={carregando === acao}
                  >
                    {carregando === acao ? "..." : label}
                  </button>
                ))}
              </div>
            </>
          ) : (
            <form className="form-editar" onSubmit={handleAtualizar}>
              <h4 className="form-titulo">Editar Cliente</h4>

              <div className="form-group">
                <label>Nome</label>
                <input
                  value={form.nome}
                  onChange={(e) => setForm({ ...form, nome: e.target.value })}
                  placeholder="Nome completo"
                />
              </div>

              <div className="form-group">
                <label>CPF</label>
                <input
                  value={form.cpf}
                  onChange={(e) => setForm({ ...form, cpf: e.target.value })}
                  placeholder="000.000.000-00"
                />
              </div>

              <div className="form-group">
                <label>E-mail</label>
                <input
                  value={form.email}
                  onChange={(e) => setForm({ ...form, email: e.target.value })}
                  placeholder="email@exemplo.com"
                />
              </div>

              <div className="form-acoes">
                <button
                  type="submit"
                  className="btn btn-salvar"
                  disabled={carregando === "editar"}
                >
                  {carregando === "editar" ? "Salvando..." : "💾 Salvar"}
                </button>
                <button
                  type="button"
                  className="btn btn-cancelar"
                  onClick={() => {
                    setEditando(false);
                    setForm({ nome: cliente.nome, cpf: cliente.cpf, email: cliente.email });
                  }}
                >
                  Cancelar
                </button>
              </div>
            </form>
          )}

          {msg.texto && (
            <div className={`msg ${msg.tipo}`}>{msg.texto}</div>
          )}
        </div>
      )}
    </div>
  );
}

export default ClienteCard;