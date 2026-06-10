import { useEffect, useState } from 'react';
import { getEvents } from '../../api/events';
import { getApiError } from '../../api/client';
import type { EventDto } from '../../types';
import EventCard from '../../components/EventCard';

export default function HomePage() {
  const [events, setEvents] = useState<EventDto[]>([]);
  const [search, setSearch] = useState('');
  const [dateFrom, setDateFrom] = useState('');
  const [dateTo, setDateTo] = useState('');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Vuelve a consultar la API (con debounce) cada vez que cambian los filtros.
  useEffect(() => {
    const timeout = setTimeout(() => {
      setLoading(true);
      setError(null);
      getEvents({
        status: 'Active',
        search: search.trim() || undefined,
        dateFrom: dateFrom || undefined,
        dateTo: dateTo || undefined,
      })
        .then(setEvents)
        .catch((e) => setError(getApiError(e)))
        .finally(() => setLoading(false));
    }, 300);

    return () => clearTimeout(timeout);
  }, [search, dateFrom, dateTo]);

  return (
    <div>
      <section className="text-center mb-10">
        <h1 className="text-4xl font-bold text-slate-800">Encuentra tu próximo evento</h1>
        <p className="text-slate-500 mt-2">
          Conciertos, festivales y teatro. Compra tus boletos en segundos.
        </p>
      </section>

      {/* Filtros */}
      <div className="bg-white rounded-2xl border border-slate-200 p-4 mb-8 grid gap-4 md:grid-cols-3">
        <div className="md:col-span-1">
          <label className="block text-xs font-medium text-slate-500 mb-1">Buscar</label>
          <input
            type="text"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            placeholder="Nombre o lugar..."
            className="w-full px-3 py-2 rounded-lg border border-slate-300 focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
        </div>
        <div>
          <label className="block text-xs font-medium text-slate-500 mb-1">Desde</label>
          <input
            type="date"
            value={dateFrom}
            onChange={(e) => setDateFrom(e.target.value)}
            className="w-full px-3 py-2 rounded-lg border border-slate-300 focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
        </div>
        <div>
          <label className="block text-xs font-medium text-slate-500 mb-1">Hasta</label>
          <input
            type="date"
            value={dateTo}
            onChange={(e) => setDateTo(e.target.value)}
            className="w-full px-3 py-2 rounded-lg border border-slate-300 focus:outline-none focus:ring-2 focus:ring-indigo-500"
          />
        </div>
      </div>

      {/* Resultados */}
      {loading && <p className="text-center text-slate-400 py-10">Cargando eventos...</p>}
      {error && (
        <p className="text-center text-rose-600 py-10">No se pudieron cargar los eventos: {error}</p>
      )}
      {!loading && !error && events.length === 0 && (
        <p className="text-center text-slate-400 py-10">No hay eventos que coincidan con la búsqueda.</p>
      )}

      <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
        {events.map((event) => (
          <EventCard key={event.id} event={event} />
        ))}
      </div>
    </div>
  );
}
