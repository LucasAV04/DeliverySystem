import { useState } from "react";
import { ClienteAPI } from "./api";

function FormAdicionarCliente() {
  const [nome, setNome] = useState("");
  const [cpf, setCpf] = useState("");
  const [email, setEmail] = useState("");
  const [msg, setMsg] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const result = await ClienteAPI.adicionar({ nome, cpf, email });
      setMsg(result);
    } catch (err) {
      setMsg(err.message);
    }
  };

  return (
    <div>
      <h2>Adicionar Cliente</h2>
      <form onSubmit={handleSubmit}>
        <input value={nome} onChange={(e) => setNome(e.target.value)} placeholder="Nome" />
        <input value={cpf} onChange={(e) => setCpf(e.target.value)} placeholder="CPF" />
        <input value={email} onChange={(e) => setEmail(e.target.value)} placeholder="Email" />
        <button type="submit">Cadastrar</button>
      </form>
      {msg && <p>{msg}</p>}
    </div>
  );
}

export default FormAdicionarCliente;