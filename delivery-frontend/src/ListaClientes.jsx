import { useEffect, useState } from "react";
import { ClienteAPI } from "./api";
import ClienteCard from "./components/ClienteCard";

function ListaClientes() {
  const [clientes, setClientes] = useState([]);
  const [erro, setErro] = useState("");

  useEffect(() => {
    ClienteAPI.listarTodos()
      .then(setClientes)
      .catch((err) => setErro(err.message));
  }, []);

  return (
    <div>
      <h2>Clientes</h2>
      {erro && <p style={{ color: "red" }}>{erro}</p>}
      <div style={{ display: "flex", flexWrap: "wrap" }}>
        {clientes.map((c) => (
          <ClienteCard key={c.id} cliente={c} />
        ))}
      </div>
    </div>
  );
}

export default ListaClientes;