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
  try {
    return JSON.parse(text);
  } catch {
    return text;
  }
}

export const ClienteAPI = {
  adicionar: (cliente) =>
    request("Cliente/Adicionar", {
      method: "POST",
      body: JSON.stringify(cliente),
    }),

  listarTodos: () => request("Cliente/ListarTodos"),

  listarVip: () => request("Cliente/ListarVip"),

  atualizar: (id, cliente) =>
    request(`Cliente/${id}/Atualizar`, {
      method: "PUT",
      body: JSON.stringify(cliente),
    }),

  ativar: (id) => request(`Cliente/${id}/Ativar`, { method: "PUT" }),

  inativar: (id) => request(`Cliente/${id}/Inativar`, { method: "PUT" }),

  bloquear: (id) => request(`Cliente/${id}/Bloquear`, { method: "PUT" }),

  darVip: (id) => request(`Cliente/${id}/DarVip`, { method: "PUT" }),
};  