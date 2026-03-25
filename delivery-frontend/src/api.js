const API_URL = import.meta.env.VITE_API_URL;
const API_KEY = import.meta.env.VITE_API_KEY;

async function request(endpoint, options = {}) {
  const url = `${API_URL}/${endpoint}`;
  const headers = {
    "Content-Type": "application/json",
    "X-Api-Key": API_KEY,
    ...options.headers,
  };

  const response = await fetch(url, { ...options, headers });
  if (!response.ok) throw new Error(await response.text());

  const text = await response.text();
  try { return JSON.parse(text); }
  catch { return text; }
}

// ── Cliente ──────────────────────────────────────────
export const ClienteAPI = {
  adicionar:   (c)     => request("Cliente/Adicionar", { method: "POST", body: JSON.stringify(c) }),
  listarTodos: ()      => request("Cliente/ListarTodos"),
  listarVip:   ()      => request("Cliente/ListarVip"),
  atualizar:   (id, c) => request(`Cliente/${id}/Atualizar`, { method: "PUT", body: JSON.stringify(c) }),
  ativar:      (id)    => request(`Cliente/${id}/Ativar`,    { method: "PUT" }),
  inativar:    (id)    => request(`Cliente/${id}/Inativar`,  { method: "PUT" }),
  bloquear:    (id)    => request(`Cliente/${id}/Bloquear`,  { method: "PUT" }),
  darVip:      (id)    => request(`Cliente/${id}/DarVip`,    { method: "PUT" }),
};

// ── Motorista ─────────────────────────────────────────
export const MotoristaAPI = {
  adicionar:    (m)     => request("Motorista/Adicionar", { method: "POST", body: JSON.stringify(m) }),
  listarTodos:  ()      => request("Motorista/ListarMotoristas"),
  listarAtivos: ()      => request("Motorista/ListarMotoristaAtivos"),
  atualizar:    (id, m) => request(`Motorista/${id}/Atualizar`, { method: "PUT", body: JSON.stringify(m) }),
  ativar:       (id)    => request(`Motorista/${id}/AtivarMotorista`,    { method: "PUT" }),
  inativar:     (id)    => request(`Motorista/${id}/InativarMotorista`,  { method: "PUT" }),
  bloquear:     (id)    => request(`Motorista/${id}/BloquearMotorista`,  { method: "PUT" }),
};

// ── Veículo ───────────────────────────────────────────
export const VeiculoAPI = {
  adicionar:         (v)     => request("Veiculo/Adicionar", { method: "POST", body: JSON.stringify(v) }),
  listarTodos:       ()      => request("Veiculo/ListarVeiculos"),
  listarDisponiveis: ()      => request("Veiculo/ListarVeiculosDisponivel"),
  atualizar:         (id, v) => request(`Veiculo/${id}/Atualizar`,  { method: "PUT", body: JSON.stringify(v) }),
  ativar:            (id)    => request(`Veiculo/${id}/Ativar`,     { method: "PUT" }),
  inativar:          (id)    => request(`Veiculo/${id}/Inativar`,   { method: "PUT" }),
  manutencao:        (id)    => request(`Veiculo/${id}/Manutencao`, { method: "PUT" }),
};

// ── Pedido ────────────────────────────────────────────
export const PedidoAPI = {
  adicionar:        (p)     => request("Pedido/Adicionar", { method: "POST", body: JSON.stringify(p) }),
  listarTodos:      ()      => request("Pedido/ListarPedidos"),
  listarCancelados: ()      => request("Pedido/ListarPedidosCancelados"),
  atualizar:        (id, p) => request(`Pedido/${id}/AtualizarPedido`,       { method: "PUT", body: JSON.stringify(p) }),
  confirmar:        (id)    => request(`Pedido/${id}/ConfirmarPedido`,        { method: "PUT" }),
  emPreparacao:     (id)    => request(`Pedido/${id}/EmPreparacao`,     { method: "PUT" }),
  prontoParaEnvio:  (id)    => request(`Pedido/${id}/ProntoParaEnvio`,  { method: "PUT" }),
  cancelar:         (id)    => request(`Pedido/${id}/CancelarPedido`,         { method: "PUT" }),
};

// ── Entrega ───────────────────────────────────────────
export const EntregaAPI = {
  iniciar:       (e)       => request("Entrega/IniciarEntrega", { method: "POST", body: JSON.stringify(e) }),
  listarTodas:   ()        => request("Entrega/ListarEntregas"),
  concluir:      (id, obs) => request(`Entrega/${id}/ConcluirEntrega`,        { method: "PUT", body: JSON.stringify({ observacoes: obs || "" }) }),
  registrarFalha:(id, obs) => request(`Entrega/${id}/RegistrarFalha`,  { method: "PUT", body: JSON.stringify({ observacoes: obs }) }),
  cancelar:      (id, obs) => request(`Entrega/${id}/CancelarEntrega`,        { method: "PUT", body: JSON.stringify({ observacoes: obs }) }),
};