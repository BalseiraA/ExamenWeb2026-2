import { Link, Outlet } from 'react-router-dom';

export default function PublicLayout() {
  return (
    <div className="min-h-screen flex flex-col">
      <header className="bg-white border-b border-slate-200 sticky top-0 z-10">
        <div className="max-w-6xl mx-auto px-4 h-16 flex items-center justify-between">
          <Link to="/" className="flex items-center gap-2 font-bold text-xl text-indigo-600">
            🎟️ TicketSystem
          </Link>
          <nav className="flex items-center gap-6 text-sm">
            <Link to="/" className="text-slate-600 hover:text-indigo-600">
              Eventos
            </Link>
            <Link
              to="/admin/login"
              className="px-4 py-2 rounded-lg bg-indigo-600 text-white hover:bg-indigo-700"
            >
              Admin
            </Link>
          </nav>
        </div>
      </header>

      <main className="flex-1 max-w-6xl mx-auto w-full px-4 py-8">
        <Outlet />
      </main>

      <footer className="border-t border-slate-200 py-6 text-center text-sm text-slate-400">
        TicketSystem — Sistema de venta de boletos para eventos
      </footer>
    </div>
  );
}
