import { useEffect, useState, useCallback } from "react";
import {
  ShieldCheck, Users, CreditCard, Activity, AlertTriangle,
  LogOut, X, Loader2, UserPlus, RefreshCw, CalendarClock,
  Pencil, Trash2, ChevronDown, ChevronUp
} from "lucide-react";
import api from "./api";
import "./App.css";

// ---- Utilidades ----
const eur = (n) =>
  new Intl.NumberFormat("es-ES", { style: "currency", currency: "EUR" }).format(n ?? 0);

const fmtDate = (d) =>
  d ? new Date(d).toLocaleDateString("es-ES", { day: "2-digit", month: "short", year: "numeric" }) : "-";

const today = () => new Date().toISOString().split("T")[0];

// ---- Componentes utiles ----
function Badge({ v, children }) {
  return <span className={`badge badge--${v}`}>{children}</span>;
}

function Spin({ size = 15 }) {
  return <Loader2 size={size} className="spin-icon" />;
}

function Empty({ text }) {
  return <p className="empty">{text}</p>;
}

function Toast({ msg, type, onClose }) {
  useEffect(() => {
    if (!msg) return;
    const t = setTimeout(onClose, 4500);
    return () => clearTimeout(t);
  }, [msg, onClose]);
  if (!msg) return null;
  return (
    <div className={`toast toast--${type}`}>
      <span>{msg}</span>
      <button onClick={onClose}><X size={13} /></button>
    </div>
  );
}

function ConfirmDialog({ msg, onOk, onCancel }) {
  return (
    <div className="modal-overlay">
      <div className="modal-box">
        <AlertTriangle size={26} className="modal-icon" />
        <p>{msg}</p>
        <div className="modal-actions">
          <button className="btn-sm btn-secondary" onClick={onCancel}>Cancelar</button>
          <button className="btn-sm btn-danger" onClick={onOk}>Confirmar</button>
        </div>
      </div>
    </div>
  );
}

// ============================================================
// LOGIN
// ============================================================
function LoginPage({ onLogin }) {
  const [email, setEmail]       = useState("Saquero@pruebas.com");
  const [password, setPassword] = useState("Admin1234!");
  const [error, setError]       = useState("");
  const [loading, setLoading]   = useState(false);

  async function submit(e) {
    e.preventDefault();
    setError(""); setLoading(true);
    try {
      const res = await api.post("/Auth/login", { email, password });
      localStorage.setItem("token", res.data.token);
      onLogin(res.data.token);
    } catch {
      setError("Credenciales incorrectas o API no disponible.");
    } finally { setLoading(false); }
  }

  return (
    <main className="login-page">
      <section className="login-card">
        <div className="brand">
          <div className="brand-icon"><img src="/favicon.svg?v=1" alt="SaqueroCloud logo" className="app-logo app-logo-large" /></div>
          <div>
            <h1>SaqueroCloud</h1>
            <p>Panel SaaS para usuarios y suscripciones</p>
          </div>
        </div>
        <form onSubmit={submit} className="login-form">
          <label>Email</label>
          <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} required />
          <label>Password</label>
          <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} required />
          <button type="submit" disabled={loading}>
            {loading ? <><Spin /> Entrando...</> : "Entrar al panel"}
          </button>
        </form>
        {error && <p className="message">{error}</p>}
        <small>Usa las credenciales seed de la API para entrar.</small>
      </section>
    </main>
  );
}

// ============================================================
// DASHBOARD
// ============================================================
function SectionDashboard({ users, plans, subscriptions, notify }) {
  const [days, setDays]           = useState(30);
  const [expiring, setExpiring]   = useState([]);
  const [loading, setLoading]     = useState(false);
  const [queried, setQueried]     = useState(false);
  const [usage, setUsage]         = useState([]);

  useEffect(() => {
    api.get("/Subscriptions/summary").then((r) => setUsage(r.data ?? [])).catch(() => {});
  }, [subscriptions]);

  async function loadExpiring() {
    setLoading(true);
    try {
      const r = await api.get(`/Subscriptions/expiring-soon?days=${days}`);
      setExpiring(r.data ?? []);
      setQueried(true);
    } catch { notify("No se pudieron cargar proximas a caducar.", "error"); }
    finally { setLoading(false); }
  }

  const expiringSoon = subscriptions.filter((s) => {
    const diff = (new Date(s.endDate) - Date.now()) / 86400000;
    return diff > 0 && diff <= 30;
  }).length;

  return (
    <>
      <section className="stats">
        <article>
          <Users />
          <div><strong>{users.length}</strong><span>Usuarios</span></div>
        </article>
        <article>
          <CreditCard />
          <div><strong>{plans.filter((p) => p.isActive).length}</strong><span>Planes activos</span></div>
        </article>
        <article>
          <Activity />
          <div><strong>{subscriptions.length}</strong><span>Suscripciones activas</span></div>
        </article>
        <article>
          <AlertTriangle />
          <div><strong>{expiringSoon}</strong><span>Caducan en 30d</span></div>
        </article>
      </section>

      {usage.length > 0 && (
        <div className="panel" style={{ marginBottom: 24 }}>
          <h2 style={{ margin: "0 0 16px" }}>Ocupacion por plan</h2>
          {usage.map((u) => (
            <div className="usage-row" key={u.planId}>
              <span className="usage-name">{u.planName}</span>
              <div className="usage-bar-wrap">
                <div
                  className="usage-bar"
                  style={{ width: `${Math.min(100, (u.activeUsers / u.maxUsers) * 100)}%` }}
                  data-full={u.isFull ? "true" : "false"}
                />
              </div>
              <span className="usage-count">{u.activeUsers} / {u.maxUsers}</span>
              {u.isFull && <Badge v="danger">Completo</Badge>}
            </div>
          ))}
        </div>
      )}

      <div className="panel expiring-panel">
        <div className="expiring-header">
          <div className="expiring-title"><CalendarClock size={17} /><h2>Proximas a caducar</h2></div>
          <div className="expiring-controls">
            <label>Dias:</label>
            <input type="number" min="1" max="365" value={days}
              onChange={(e) => setDays(Number(e.target.value))} className="days-input" />
            <button className="btn-action" onClick={loadExpiring} disabled={loading}>
              {loading ? <Spin /> : "Consultar"}
            </button>
          </div>
        </div>
        {!queried && <Empty text="Pulsa Consultar para ver suscripciones proximas a caducar." />}
        {queried && expiring.length === 0 && <Empty text={`Ninguna caduca en los proximos ${days} dias.`} />}
        {expiring.map((s) => (
          <div className="row" key={s.id}>
            <div><strong>{s.userName}</strong><span>{s.userEmail}</span></div>
            <div className="row-right">
              <Badge v="plan">{s.planName}</Badge>
              <span className="muted">{eur(s.planPrice)}/mes</span>
              <Badge v="warn">Caduca {fmtDate(s.endDate)}</Badge>
            </div>
          </div>
        ))}
      </div>
    </>
  );
}

// ============================================================
// USUARIOS
// ============================================================
function SectionUsers({ users, plans, subscriptions, notify, reload }) {
  const [showForm, setShowForm]   = useState(false);
  const [form, setForm]           = useState({ name: "", email: "", password: "", planId: "", endDate: "" });
  const [creating, setCreating]   = useState(false);
  const [openUser, setOpenUser]   = useState(null);
  const [rPlanId, setRPlanId]     = useState("");
  const [rEndDate, setREndDate]   = useState("");
  const [reassigning, setReassigning] = useState(false);

  const subByUser = {};
  subscriptions.forEach((s) => {
    if (!subByUser[s.userId]) subByUser[s.userId] = s;
  });
  const subByEmail = {};
  subscriptions.forEach((s) => { if (s.userEmail) subByEmail[s.userEmail.toLowerCase()] = s; });

  function getUserSub(u) {
    return subByUser[u.id] ?? subByEmail[u.email?.toLowerCase()] ?? null;
  }

  const f = (k, v) => setForm((p) => ({ ...p, [k]: v }));

  async function handleCreate(e) {
    e.preventDefault();
    setCreating(true);
    try {
      await api.post("/Auth/register", { name: form.name, email: form.email, password: form.password });
      if (form.planId && form.endDate) {
        const uRes = await api.get("/Users?page=1&pageSize=200");
        const all  = uRes.data.items ?? uRes.data ?? [];
        const created = all.find((u) => u.email.toLowerCase() === form.email.toLowerCase());
        if (created) {
          const res = await api.post(`/Subscriptions/assign/${created.id}`, {
            planId: Number(form.planId), endDate: form.endDate
          });
          if (res.status >= 400) notify(res.data?.message ?? "No se pudo asignar el plan.", "error");
        }
      }
      notify("Usuario creado correctamente.", "ok");
      setForm({ name: "", email: "", password: "", planId: "", endDate: "" });
      setShowForm(false);
      reload();
    } catch (err) {
      notify(err?.response?.data?.message ?? "No se pudo crear el usuario.", "error");
    } finally { setCreating(false); }
  }

  async function handleReassign(userId) {
    if (!rPlanId || !rEndDate) { notify("Selecciona plan y fecha.", "error"); return; }
    setReassigning(true);
    try {
      const res = await api.post(`/Subscriptions/assign/${userId}`, {
        planId: Number(rPlanId), endDate: rEndDate
      });
      if (res.status >= 400) { notify(res.data?.message ?? "Error al reasignar.", "error"); }
      else { notify("Plan asignado correctamente.", "ok"); setOpenUser(null); reload(); }
    } catch (err) {
      notify(err?.response?.data?.message ?? "No se pudo reasignar el plan.", "error");
    } finally { setReassigning(false); }
  }

  return (
    <>
      <div className="section-topbar">
        <h2 className="section-title">Usuarios <span className="count">{users.length}</span></h2>
        <button className="btn-action" onClick={() => setShowForm((v) => !v)}>
          <UserPlus size={14} />{showForm ? "Cerrar" : "Nuevo usuario"}
        </button>
      </div>

      {showForm && (
        <div className="panel form-panel">
          <h3 className="form-title">Crear usuario</h3>
          <form onSubmit={handleCreate} className="create-form">
            <div className="field"><label>Nombre</label>
              <input value={form.name} onChange={(e) => f("name", e.target.value)} placeholder="Nombre completo" required />
            </div>
            <div className="field"><label>Email</label>
              <input type="email" value={form.email} onChange={(e) => f("email", e.target.value)} placeholder="email@ejemplo.com" required />
            </div>
            <div className="field"><label>Password</label>
              <input type="password" value={form.password} onChange={(e) => f("password", e.target.value)} placeholder="Min. 6 caracteres" required />
            </div>
            <div className="field"><label>Plan (opcional)</label>
              <select value={form.planId} onChange={(e) => f("planId", e.target.value)}>
                <option value="">Sin plan</option>
                {plans.filter((p) => p.isActive).map((p) => (
                  <option key={p.id} value={p.id}>{p.name} - {eur(p.price)}</option>
                ))}
              </select>
            </div>
            {form.planId && (
              <div className="field"><label>Fecha fin</label>
                <input type="date" value={form.endDate} min={today()} onChange={(e) => f("endDate", e.target.value)} required />
              </div>
            )}
            <div className="field field--action">
              <button type="submit" className="btn-primary" disabled={creating}>
                {creating ? <><Spin /> Creando...</> : "Crear usuario"}
              </button>
            </div>
          </form>
        </div>
      )}

      <div className="panel">
        {users.length === 0 && <Empty text="No hay usuarios registrados." />}
        {users.map((u) => {
          const sub    = getUserSub(u);
          const isOpen = openUser === u.id;
          return (
            <div key={u.id} className="user-block">
              <div className="row user-row">
                <div className="user-info">
                  <strong>{u.name}</strong>
                  <span>{u.email}</span>
                </div>
                <div className="user-sub">
                  {sub
                    ? <><Badge v="plan">{sub.planName}</Badge><span className="muted">{eur(sub.planPrice)} -hasta {fmtDate(sub.endDate)}</span></>
                    : <Badge v="none">Sin plan activo</Badge>
                  }
                </div>
                <div className="user-actions">
                  <Badge v={u.role === "Admin" ? "admin" : "user"}>{u.role}</Badge>
                  <Badge v={u.isActive ? "active" : "inactive"}>{u.isActive ? "Activo" : "Inactivo"}</Badge>
                  <button className="btn-sm" onClick={() => { setOpenUser(isOpen ? null : u.id); setRPlanId(""); setREndDate(""); }}>
                    {isOpen ? <ChevronUp size={13} /> : <ChevronDown size={13} />}
                    {isOpen ? "Cerrar" : "Cambiar plan"}
                  </button>
                </div>
              </div>
              {isOpen && (
                <div className="inline-reassign">
                  <select value={rPlanId} onChange={(e) => setRPlanId(e.target.value)}>
                    <option value="">Selecciona plan...</option>
                    {plans.filter((p) => p.isActive).map((p) => (
                      <option key={p.id} value={p.id}>{p.name} - {eur(p.price)}</option>
                    ))}
                  </select>
                  <input type="date" value={rEndDate} min={today()} onChange={(e) => setREndDate(e.target.value)} />
                  <button className="btn-primary btn-sm" onClick={() => handleReassign(u.id)} disabled={reassigning}>
                    {reassigning ? <Spin /> : "Asignar"}
                  </button>
                </div>
              )}
            </div>
          );
        })}
      </div>
    </>
  );
}

// ============================================================
// PLANES
// ============================================================
function SectionPlans({ plans, notify, reload }) {
  const [usage, setUsage]         = useState([]);
  const [showCreate, setShowCreate] = useState(false);
  const [form, setForm]           = useState({ name: "", description: "", price: "", maxUsers: "" });
  const [creating, setCreating]   = useState(false);
  const [editId, setEditId]       = useState(null);
  const [editForm, setEditForm]   = useState({});
  const [saving, setSaving]       = useState(false);
  const [confirm, setConfirm]     = useState(null);

  useEffect(() => {
    api.get("/Subscriptions/summary").then((r) => setUsage(r.data ?? [])).catch(() => {});
  }, [plans]);

  const getUsage = (id) => usage.find((u) => u.planId === id);
  const fe = (k, v) => setForm((p) => ({ ...p, [k]: v }));
  const ee = (k, v) => setEditForm((p) => ({ ...p, [k]: v }));

  async function handleCreate(e) {
    e.preventDefault();
    setCreating(true);
    try {
      await api.post("/subscription-plans", {
        name: form.name, description: form.description,
        price: parseFloat(form.price), maxUsers: parseInt(form.maxUsers)
      });
      notify("Plan creado correctamente.", "ok");
      setForm({ name: "", description: "", price: "", maxUsers: "" });
      setShowCreate(false);
      reload();
    } catch (err) {
      notify(err?.response?.data?.message ?? "No se pudo crear el plan.", "error");
    } finally { setCreating(false); }
  }

  async function handleEdit(plan) {
    setSaving(true);
    try {
      await api.put(`/subscription-plans/${plan.id}`, {
        description: editForm.description,
        price: parseFloat(editForm.price),
        maxUsers: parseInt(editForm.maxUsers),
        isActive: editForm.isActive
      });
      notify("Plan actualizado.", "ok");
      setEditId(null);
      reload();
    } catch (err) {
      notify(err?.response?.data?.message ?? "No se pudo actualizar el plan.", "error");
    } finally { setSaving(false); }
  }

  async function handleDelete(id) {
    try {
      await api.delete(`/subscription-plans/${id}`);
      notify("Plan eliminado.", "ok");
      reload();
    } catch (err) {
      notify(err?.response?.data?.message ?? "No se puede eliminar: el plan tiene suscripciones activas.", "error");
    } finally { setConfirm(null); }
  }

  return (
    <>
      {confirm && (
        <ConfirmDialog
          msg={`Eliminar el plan "${confirm.name}"?`}
          onOk={() => handleDelete(confirm.id)}
          onCancel={() => setConfirm(null)}
        />
      )}

      <div className="section-topbar">
        <h2 className="section-title">Planes <span className="count">{plans.length}</span></h2>
        <button className="btn-action" onClick={() => setShowCreate((v) => !v)}>
          <CreditCard size={14} />{showCreate ? "Cerrar" : "Nuevo plan"}
        </button>
      </div>

      {showCreate && (
        <div className="panel form-panel">
          <h3 className="form-title">Crear plan</h3>
          <form onSubmit={handleCreate} className="create-form">
            <div className="field"><label>Nombre</label>
              <input value={form.name} onChange={(e) => fe("name", e.target.value)} placeholder="Ej: Pro" required />
            </div>
            <div className="field"><label>Descripcion</label>
              <input value={form.description} onChange={(e) => fe("description", e.target.value)} placeholder="Descripcion" required />
            </div>
            <div className="field"><label>Precio (EUR)</label>
              <input type="number" step="0.01" min="0" value={form.price} onChange={(e) => fe("price", e.target.value)} placeholder="9.99" required />
            </div>
            <div className="field"><label>Max usuarios</label>
              <input type="number" min="1" value={form.maxUsers} onChange={(e) => fe("maxUsers", e.target.value)} placeholder="10" required />
            </div>
            <div className="field field--action">
              <button type="submit" className="btn-primary" disabled={creating}>
                {creating ? <><Spin /> Creando...</> : "Crear plan"}
              </button>
            </div>
          </form>
        </div>
      )}

      <div className="grid">
        {plans.length === 0 && <Empty text="No hay planes configurados." />}
        {plans.map((plan) => {
          const u    = getUsage(plan.id);
          const isEditing = editId === plan.id;
          return (
            <div className="panel plan-card" key={plan.id} style={{ marginBottom: 0 }}>
              {isEditing ? (
                <div className="edit-form">
                  <h3 className="form-title">Editar {plan.name}</h3>
                  <div className="field"><label>Descripcion</label>
                    <input value={editForm.description} onChange={(e) => ee("description", e.target.value)} />
                  </div>
                  <div className="field"><label>Precio (EUR)</label>
                    <input type="number" step="0.01" min="0" value={editForm.price} onChange={(e) => ee("price", e.target.value)} />
                  </div>
                  <div className="field"><label>Max usuarios</label>
                    <input type="number" min="1" value={editForm.maxUsers} onChange={(e) => ee("maxUsers", e.target.value)} />
                  </div>
                  <div className="field">
                    <label>
                      <input type="checkbox" checked={editForm.isActive}
                        onChange={(e) => ee("isActive", e.target.checked)} style={{ marginRight: 6 }} />
                      Activo
                    </label>
                  </div>
                  <div className="form-row-btns">
                    <button className="btn-sm btn-secondary" onClick={() => setEditId(null)}>Cancelar</button>
                    <button className="btn-sm btn-primary" onClick={() => handleEdit(plan)} disabled={saving}>
                      {saving ? <Spin /> : "Guardar"}
                    </button>
                  </div>
                </div>
              ) : (
                <>
                  <div className="plan-card-header">
                    <strong>{plan.name}</strong>
                    <Badge v={plan.isActive ? "active" : "inactive"}>{plan.isActive ? "Activo" : "Inactivo"}</Badge>
                  </div>
                  <p className="plan-desc">{plan.description}</p>
                  {u && (
                    <div className="usage-inline">
                      <div className="usage-bar-wrap">
                        <div className="usage-bar"
                          style={{ width: `${Math.min(100, (u.activeUsers / u.maxUsers) * 100)}%` }}
                          data-full={u.isFull ? "true" : "false"} />
                      </div>
                      <span className="usage-count">{u.activeUsers}/{u.maxUsers}</span>
                      {u.isFull && <Badge v="danger">Completo</Badge>}
                    </div>
                  )}
                  <div className="plan-footer">
                    <span className="plan-price">{eur(plan.price)}<small>/mes</small></span>
                    <div className="plan-btns">
                      <button className="btn-icon" title="Editar" onClick={() => {
                        setEditId(plan.id);
                        setEditForm({ description: plan.description, price: plan.price, maxUsers: plan.maxUsers, isActive: plan.isActive });
                      }}><Pencil size={14} /></button>
                      <button className="btn-icon btn-icon--danger" title="Eliminar" onClick={() => setConfirm(plan)}>
                        <Trash2 size={14} />
                      </button>
                    </div>
                  </div>
                </>
              )}
            </div>
          );
        })}
      </div>
    </>
  );
}

// ============================================================
// SUSCRIPCIONES
// ============================================================
function SectionSubscriptions({ subscriptions, users, plans, notify, reload }) {
  const [usage, setUsage]         = useState([]);
  const [filterPlan, setFilterPlan] = useState("");
  const [assignForm, setAssignForm] = useState({ userId: "", planId: "", endDate: "" });
  const [assigning, setAssigning] = useState(false);
  const [editSub, setEditSub]     = useState(null);
  const [editPlanId, setEditPlanId] = useState("");
  const [editEndDate, setEditEndDate] = useState("");
  const [saving, setSaving]       = useState(false);
  const [confirm, setConfirm]     = useState(null);

  useEffect(() => {
    api.get("/Subscriptions/summary").then((r) => setUsage(r.data ?? [])).catch(() => {});
  }, [subscriptions]);

  const getUsage = (id) => usage.find((u) => u.planId === id);
  const af = (k, v) => setAssignForm((p) => ({ ...p, [k]: v }));

  const filtered = filterPlan
    ? subscriptions.filter((s) => s.planName?.toLowerCase() === filterPlan.toLowerCase())
    : subscriptions;

  async function handleAssign(e) {
    e.preventDefault();
    setAssigning(true);
    try {
      await api.post(`/Subscriptions/assign/${assignForm.userId}`, {
        planId: Number(assignForm.planId), endDate: assignForm.endDate
      });
      notify("Suscripcion asignada.", "ok");
      setAssignForm({ userId: "", planId: "", endDate: "" });
      reload();
    } catch (err) {
      notify(err?.response?.data?.message ?? "No se pudo asignar.", "error");
    } finally { setAssigning(false); }
  }

  async function handleUpdate() {
    if (!editPlanId || !editEndDate) { notify("Completa plan y fecha.", "error"); return; }
    setSaving(true);
    try {
      await api.put(`/Subscriptions/${editSub.id}`, {
        planId: Number(editPlanId), endDate: editEndDate
      });
      notify("Suscripcion actualizada.", "ok");
      setEditSub(null);
      reload();
    } catch (err) {
      notify(err?.response?.data?.message ?? "No se pudo actualizar.", "error");
    } finally { setSaving(false); }
  }

  async function handleCancel(id) {
    try {
      await api.patch(`/Subscriptions/${id}/cancel`, {});
      notify("Suscripcion cancelada.", "ok");
      setConfirm(null);
      reload();
    } catch {
      notify("No se pudo cancelar.", "error");
      setConfirm(null);
    }
  }

  return (
    <>
      {confirm && (
        <ConfirmDialog
          msg={`Cancelar suscripcion de ${confirm.userName}?`}
          onOk={() => handleCancel(confirm.id)}
          onCancel={() => setConfirm(null)}
        />
      )}

      <h2 className="section-title">
        Suscripciones activas <span className="count">{subscriptions.length}</span>
      </h2>

      {/* Formulario asignar */}
      <div className="panel form-panel">
        <h3 className="form-title">Asignar suscripcion</h3>
        <form onSubmit={handleAssign} className="assign-form">
          <div className="field"><label>Usuario</label>
            <select value={assignForm.userId} onChange={(e) => af("userId", e.target.value)} required>
              <option value="">Selecciona usuario...</option>
              {users.map((u) => <option key={u.id} value={u.id}>{u.name}</option>)}
            </select>
          </div>
          <div className="field"><label>Plan</label>
            <select value={assignForm.planId} onChange={(e) => af("planId", e.target.value)} required>
              <option value="">Selecciona plan...</option>
              {plans.filter((p) => p.isActive).map((p) => {
                const u    = getUsage(p.id);
                const full = u?.isFull ?? false;
                return (
                  <option key={p.id} value={p.id} disabled={full}>
                    {p.name} - {eur(p.price)}{full ? " (Completo)" : ""}
                  </option>
                );
              })}
            </select>
          </div>
          <div className="field"><label>Fecha fin</label>
            <input type="date" value={assignForm.endDate} min={today()} onChange={(e) => af("endDate", e.target.value)} required />
          </div>
          <div className="field field--action">
            <button type="submit" className="btn-primary" disabled={assigning}>
              {assigning ? <><Spin /> Asignando...</> : "Asignar"}
            </button>
          </div>
        </form>
      </div>

      {/* Lista suscripciones */}
      <div className="panel">
        <div className="panel-header">
          <h2 style={{ margin: 0 }}>Listado</h2>
          <select className="filter-select" value={filterPlan} onChange={(e) => setFilterPlan(e.target.value)}>
            <option value="">Todos los planes</option>
            {plans.map((p) => <option key={p.id} value={p.name}>{p.name}</option>)}
          </select>
        </div>

        {filtered.length === 0 && <Empty text="No hay suscripciones activas." />}

        {filtered.map((sub) => (
          <div key={sub.id}>
            <div className="row sub-row">
              <div>
                <strong>{sub.userName}</strong>
                <span>{sub.userEmail}</span>
              </div>
              <div className="sub-detail">
                <Badge v="plan">{sub.planName}</Badge>
                <span className="muted">{eur(sub.planPrice)}/mes</span>
                <span className="muted">{fmtDate(sub.startDate)} - {fmtDate(sub.endDate)}</span>
                <Badge v="active">Activa</Badge>
              </div>
              <div className="sub-btns">
                <button className="btn-sm" onClick={() => {
                  setEditSub(editSub?.id === sub.id ? null : sub);
                  setEditPlanId(String(sub.planId));
                  setEditEndDate(sub.endDate ? sub.endDate.split("T")[0] : "");
                }}>
                  <Pencil size={12} /> Editar
                </button>
                <button className="btn-sm btn-danger" onClick={() => setConfirm(sub)}>Cancelar</button>
              </div>
            </div>

            {editSub?.id === sub.id && (
              <div className="inline-edit-sub">
                <select value={editPlanId} onChange={(e) => setEditPlanId(e.target.value)}>
                  <option value="">Plan...</option>
                  {plans.filter((p) => p.isActive).map((p) => {
                    const u    = getUsage(p.id);
                    const full = u?.isFull && p.id !== sub.planId;
                    return (
                      <option key={p.id} value={p.id} disabled={full}>
                        {p.name} - {eur(p.price)}{full ? " (Completo)" : ""}
                      </option>
                    );
                  })}
                </select>
                <input type="date" value={editEndDate} min={today()} onChange={(e) => setEditEndDate(e.target.value)} />
                <button className="btn-sm btn-secondary" onClick={() => setEditSub(null)}>Cancelar</button>
                <button className="btn-sm btn-primary" onClick={handleUpdate} disabled={saving}>
                  {saving ? <Spin /> : "Guardar"}
                </button>
              </div>
            )}
          </div>
        ))}
      </div>
    </>
  );
}

// ============================================================
// APP
// ============================================================
const NAV = [
  { id: "dashboard",      label: "Dashboard" },
  { id: "usuarios",       label: "Usuarios" },
  { id: "planes",         label: "Planes" },
  { id: "suscripciones",  label: "Suscripciones" },
];

export default function App() {
  const [token, setToken]             = useState(localStorage.getItem("token"));
  const [section, setSection]         = useState("dashboard");
  const [users, setUsers]             = useState([]);
  const [plans, setPlans]             = useState([]);
  const [subscriptions, setSubs]      = useState([]);
  const [loadingMain, setLoadingMain] = useState(false);
  const [toast, setToast]             = useState({ msg: "", type: "ok" });

  const notify = useCallback((msg, type = "ok") => setToast({ msg, type }), []);

  const loadAll = useCallback(async () => {
    if (!token) return;
    setLoadingMain(true);
    try {
      const [uR, pR, sR] = await Promise.all([
        api.get("/Users?page=1&pageSize=200"),
        api.get("/subscription-plans"),
        api.get("/Subscriptions/active"),
      ]);
      setUsers(uR.data.items ?? uR.data ?? []);
      setPlans(pR.data ?? []);
      setSubs(sR.data ?? []);
    } catch {
      notify("No se pudieron cargar los datos. Revisa que la API esta ejecutandose.", "error");
    } finally { setLoadingMain(false); }
  }, [token, notify]);

  useEffect(() => { loadAll(); }, [loadAll]);

  function logout() {
    localStorage.removeItem("token");
    setToken(null);
    setUsers([]); setPlans([]); setSubs([]);
  }

  if (!token) return <LoginPage onLogin={setToken} />;

  return (
    <main className="dashboard">
      <Toast msg={toast.msg} type={toast.type} onClose={() => setToast({ msg: "", type: "ok" })} />

      <aside className="sidebar">
        <div className="sidebar-title"><img src="/favicon.svg?v=1" alt="SaqueroCloud logo" className="app-logo app-logo-small" /><span>SaqueroCloud</span></div>
        <nav>
          {NAV.map((n) => (
            <a key={n.id} className={section === n.id ? "active" : ""}
              onClick={() => setSection(n.id)} style={{ cursor: "pointer" }}>
              {n.label}
            </a>
          ))}
        </nav>
        <button className="logout" onClick={logout}><LogOut size={18} />Salir</button>
      </aside>

      <section className="content">
        <header className="topbar">
          <div>
            <h1>{NAV.find((n) => n.id === section)?.label}</h1>
            <p>Panel de administracion SaqueroCloud</p>
          </div>
          <button onClick={loadAll} disabled={loadingMain} className="topbar-refresh">
            {loadingMain ? <Spin /> : <RefreshCw size={14} />}
            {loadingMain ? "Cargando..." : "Actualizar"}
          </button>
        </header>

        {section === "dashboard" && (
          <SectionDashboard users={users} plans={plans} subscriptions={subscriptions} notify={notify} />
        )}
        {section === "usuarios" && (
          <SectionUsers users={users} plans={plans} subscriptions={subscriptions} notify={notify} reload={loadAll} />
        )}
        {section === "planes" && (
          <SectionPlans plans={plans} notify={notify} reload={loadAll} />
        )}
        {section === "suscripciones" && (
          <SectionSubscriptions subscriptions={subscriptions} users={users} plans={plans} notify={notify} reload={loadAll} />
        )}
      </section>
    </main>
  );
}



