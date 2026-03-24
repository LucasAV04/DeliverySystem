import { useState } from "react";
import { ClienteAPI } from "../api";

function ClienteCard({ cliente }) {
  const [aberto, setAberto] = useState(false);
  const [msg, setMsg] = useState("");

  const toggleCard = () => setAberto(!aberto);

  const handleDarVip = async () => {
    try {
      const result = await ClienteAPI.darVip(cliente.id);
      setMsg(result);
    } catch (err) {
      setMsg(err.message);
    }
  };

  const handleAtualizar = async () => {
    try {
      const result = await ClienteAPI.atualizar(cliente.id, {
        nome: cliente.nome,
        cpf: cliente.cpf,
        email: cliente.email,
      });
      setMsg(result);
    } catch (err) {
      setMsg(err.message);
    }
  };

  return (
    <div style={{ border: "1px solid #ccc", margin: "10px", padding: "10px" }}>
      <h3 onClick={toggleCard} style={{ cursor: "pointer" }}>
        {cliente.nome} ({aberto ? "▲" : "▼"})
      </h3>

      {aberto && (
        <div>
          <p><strong>Id:</strong> {cliente.id}</p>
          <p><strong>CPF:</strong> {cliente.cpf}</p>
          <p><strong>Email:</strong> {cliente.email}</p>
          <p><strong>Status:</strong> {cliente.status}</p>

          <button onClick={handleDarVip}>Dar VIP</button>
          <button onClick={handleAtualizar}>Atualizar</button>

          {msg && <p style={{ color: "green" }}>{msg}</p>}
        </div>
      )}
    </div>
  );
}

export default ClienteCard;