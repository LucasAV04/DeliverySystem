import { useEffect, useState } from "react";
import { ClienteAPI } from "./api";
import ClienteCard from "./components/ClienteCard";

function ListaClientesVip() {
  const [clientes, setClientes] = useState([]);
  const [erro, setErro] = useState("");

  useEffect(() => {
    ClienteAPI.listarVip()
      .then(setClientes)
      .catch((err) => setErro(err.message));
  }, []);

  return (
    <div>
      <h2>Clientes VIP</h2>
      {erro && <p style={{ color: "red" }}>{erro}</p>}
      <div style={{ display: "flex", flexWrap: "wrap" }}>
        {clientes.map((c) => (
          <ClienteCard key={c.id} cliente={c} />
        ))}
      </div>
    </div>
  );
}

export default ListaClientesVip;