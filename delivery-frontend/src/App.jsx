import { useState } from "react";
import ListaClientes from "./pages/ListaClientes";
import ListaMotoristas from "./pages/ListaMotoristas";
import ListaVeiculos from "./pages/ListaVeiculos";
import ListaPedidos from "./pages/ListaPedidos";
import ListaEntregas from "./pages/ListaEntregas";
import "./App.css";

const PAGINAS = [
  { id: "clientes",   label: "👤 Clientes"   },
  { id: "motoristas", label: "🧑‍✈️ Motoristas" },
  { id: "veiculos",   label: "🚛 Veículos"    },
  { id: "pedidos",    label: "📦 Pedidos"     },
  { id: "entregas",   label: "🚚 Entregas"    },
];

function App() {
  const [pagina, setPagina] = useState("clientes");

  return (
    <div className="app">
      <nav className="navbar">
        <span className="navbar-logo">🏭 DataFleet</span>
        <div className="navbar-links">
          {PAGINAS.map((p) => (
            <button
              key={p.id}
              className={`nav-btn ${pagina === p.id ? "ativo" : ""}`}
              onClick={() => setPagina(p.id)}
            >
              {p.label}
            </button>
          ))}
        </div>
      </nav>

      <main className="main-content">
        {pagina === "clientes"   && <ListaClientes />}
        {pagina === "motoristas" && <ListaMotoristas />}
        {pagina === "veiculos"   && <ListaVeiculos />}
        {pagina === "pedidos"    && <ListaPedidos />}
        {pagina === "entregas"   && <ListaEntregas />}
      </main>
    </div>
  );
}

export default App;