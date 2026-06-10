import { Link, NavLink, Outlet, useNavigate } from 'react-router-dom';
import { useAuth } from '../auth/AuthContext';

const links = [
  { to: '/admin/dashboard', label: 'Dashboard', icon: '📊' },
  { to: '/admin/events', label: 'Eventos', icon: '🎫' },
];

export default function AdminLayout() {
  const { email, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/admin/login');
  };

  return (
    <div className="min-h-screen flex bg-slate-100">
      <aside className="w-60 bg-slate-900 text-slate-100 flex flex-col">
        <div className="h-16 flex items-center px-6 font-bold text-lg border-b border-slate-700">
          🎟️ Admin
        </div>
        <nav className="flex-1 p-4 space-y-1">
          {links.map((link) => (
            <NavLink
              key={link.to}
              to={link.to}
              className={({ isActive }) =>
                `flex items-center gap-3 px-4 py-2.5 rounded-lg text-sm transition ${
                  isActive ? 'bg-indigo-600 text-white' : 'text-slate-300 hover:bg-slate-800'
                }`
              }
            >
              <span>{link.icon}</span>
              {link.label}
            </NavLink>
          ))}
        </nav>
        <div className="p-4 border-t border-slate-700 text-xs text-slate-400">
          <Link to="/" className="block mb-3 hover:text-white">
            ← Ver portal público
          </Link>
          <p className="mb-2 truncate">{email}</p>
          <button
            onClick={handleLogout}
            className="w-full px-3 py-2 rounded-lg bg-slate-800 hover:bg-rose-600 text-white transition"
          >
            Cerrar sesión
          </button>
        </div>
      </aside>

      <main className="flex-1 p-8 overflow-auto">
        <Outlet />
      </main>
    </div>
  );
}
