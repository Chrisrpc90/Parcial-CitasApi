function getBaseUrl() {
  return window.location.origin;
}

async function fetchJson(path) {
  const res = await fetch(getBaseUrl() + path);
  if (!res.ok) {
    const text = await res.text();
    throw new Error(`Error ${res.status}: ${text}`);
  }
  return res.json();
}

function setText(id, value) {
  const el = document.getElementById(id);
  if (el) el.textContent = value;
}

function setHtml(id, value) {
  const el = document.getElementById(id);
  if (el) el.innerHTML = value;
}

// Normalizador
function norm(s) {
  return (s ?? "").toString().trim().toLowerCase();
}

// ===== FORMATO FECHA =====
function formatDate(iso) {
  const d = new Date(iso);
  if (Number.isNaN(d.getTime())) return iso ?? "—";
  return d.toLocaleDateString("es-PE");
}

// ===== FORMATO HORA =====
function formatTime(iso) {
  const d = new Date(iso);
  if (Number.isNaN(d.getTime())) return "—";
  return d.toLocaleTimeString("es-PE", { hour: "2-digit", minute: "2-digit" });
}

// ---- Dashboard KPIs ----
async function loadDashboard() {
  try {
    const [pacientes, medicos, citas] = await Promise.all([
      fetchJson("/api/pacientes"),
      fetchJson("/api/medicos"),
      fetchJson("/api/citas"),
    ]);

    setText("kpiPacientes", pacientes.length);
    setText("kpiMedicos", medicos.length);
    setText("kpiCitas", citas.length);

    const confirmadas = citas.filter(c => norm(c.estado) === "confirmada").length;
    const programadas = citas.filter(c => norm(c.estado) === "programada").length;
    const canceladas = citas.filter(c => ["cancelada", "anulada"].includes(norm(c.estado))).length;

    setText("kpiConfirmadas", confirmadas);
    setText("kpiProgramadas", programadas);
    setText("kpiCanceladas", canceladas);

  } catch (err) {
    console.error(err);
    setText("dashboardError", "No se pudo cargar datos. Verifica que la API esté corriendo.");
  }
}

// ---- Pacientes ----
async function loadPacientes() {
  const tbody = document.getElementById("pacientesBody");
  try {
    const data = await fetchJson("/api/pacientes");
    setText("countPacientes", data.length);

    if (tbody) {
      tbody.innerHTML = data.map(p => `
        <tr>
          <td>${p.id}</td>
          <td>
            <strong>${p.nombres} ${p.apellidos}</strong>
            <div class="muted">DNI: ${p.dni}</div>
          </td>
          <td>${p.telefono ?? "<span class='muted'>—</span>"}</td>
          <td>${p.email ?? "<span class='muted'>—</span>"}</td>
        </tr>
      `).join("");
    }
  } catch (err) {
    console.error(err);
    if (tbody) tbody.innerHTML = `<tr><td colspan="4" class="text-danger">Error cargando pacientes</td></tr>`;
  }
}

// ---- Médicos ----
    async function loadMedicos() {
      const tbody = document.getElementById("medicosBody");
      try {
        const data = await fetchJson("/api/medicos");
        setText("countMedicos", data.length);

        if (tbody) {
          tbody.innerHTML = data.map(m => `
            <tr>
              <td>${m.id}</td>
              <td><strong>${m.nombres} ${m.apellidos}</strong></td>
              <td><span class="badge text-bg-primary">${m.especialidadNombre ?? "—"}</span></td>
            </tr>
          `).join("");
        }
      } catch (err) {
        console.error(err);
        if (tbody) tbody.innerHTML = `<tr><td colspan="3" class="text-danger">Error cargando médicos</td></tr>`;
      }
    }

// ---- Citas ----
function estadoBadge(estado) {
  const e = norm(estado);

  if (e === "confirmada") {
    return `<span class="badge-status badge-confirmada">Confirmada</span>`;
  }
  if (e === "cancelada" || e === "anulada") {
    return `<span class="badge-status badge-cancelada">Cancelada</span>`;
  }
  return `<span class="badge-status badge-programada">Programada</span>`;
}

// Se llama desde el BODY: onload="loadCitasFromQuery()"
function loadCitasFromQuery() {
  const params = new URLSearchParams(window.location.search);

  // valores válidos: programada | confirmada | cancelada | todas
  let estado = norm(params.get("estado") || "todas");

  // normalizar alias
  if (estado === "programadas") estado = "programada";
  if (estado === "confirmadas") estado = "confirmada";
  if (estado === "canceladas") estado = "cancelada";
  if (estado === "anulada") estado = "cancelada";

  const allowed = ["todas", "programada", "confirmada", "cancelada"];
  if (!allowed.includes(estado)) estado = "todas";

  loadCitas(estado);
}

function setCitasLabels(estado) {
  const title = document.getElementById("citasTitle");
  const label = document.getElementById("citasFiltroLabel");

  const map = {
    "todas": "Todas",
    "programada": "Programadas",
    "confirmada": "Confirmadas",
    "cancelada": "Canceladas"
  };

  const txt = map[estado] ?? "Todas";

  // si tu HTML tiene estos IDs, los actualiza; si no, no pasa nada
  if (title) title.textContent = "Citas";
  if (label) label.textContent = txt;
}

async function loadCitas(estado = "todas") {
  const tbody = document.getElementById("citasBody");

  try {
    setHtml("citasError", "");
    setCitasLabels(estado);

    const data = await fetchJson("/api/citas");

    // filtro
    let filtered = data;

    if (estado !== "todas") {
      if (estado === "cancelada") {
        filtered = data.filter(c => ["cancelada", "anulada"].includes(norm(c.estado)));
      } else {
        filtered = data.filter(c => norm(c.estado) === estado);
      }
    }

    setText("countCitas", filtered.length);

    if (!tbody) return;

    if (filtered.length === 0) {
      tbody.innerHTML = `
        <tr>
          <td colspan="8" class="text-center muted py-4">
            No hay citas para este filtro.
          </td>
        </tr>
      `;
      return;
    }

    tbody.innerHTML = filtered.map(c => `
      <tr>
        <td>${c.id}</td>
        <td><strong>${formatDate(c.fechaHora)}</strong></td>
        <td class="muted">${formatTime(c.fechaHora)}</td>
        <td>${c.motivo ?? "<span class='muted'>—</span>"}</td>

        <td>
          <div><strong>${c.pacienteNombre ?? c.pacienteId}</strong></div>
          <div class="muted">Paciente ID: ${c.pacienteId}</div>
        </td>

        <td>
          <div><strong>${c.medicoNombre ?? c.medicoId}</strong></div>
        </td>

        <td>
          <span class="badge text-bg-primary">${c.medicoEspecialidad ?? "—"}</span>
        </td>

        <td>${estadoBadge(c.estado)}</td>
      </tr>
    `).join("");

  } catch (err) {
    console.error(err);
    if (tbody) {
      tbody.innerHTML = `<tr><td colspan="8" class="text-danger">Error cargando citas</td></tr>`;
    }
  }
}