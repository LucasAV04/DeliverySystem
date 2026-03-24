import { useState } from "react";
import { ClienteAPI } from "./api";

function DarVip() {
  const [id, setId] = useState("");
  const [msg, setMsg] = useState("");

  const handleVip = async () => {
    try {
      const result = await ClienteAPI.darVip(id);
      setMsg(result);
    } catch (err) {
      setMsg(err.message);
    }
  };

  return (
    <div>
      <h2>Dar VIP</h2>
      <input value={id} onChange={(e) => setId(e.target.value)} placeholder="Id do cliente" />
      <button onClick={handleVip}>Dar VIP</button>
      {msg && <p>{msg}</p>}
    </div>
  );
}

export default DarVip;