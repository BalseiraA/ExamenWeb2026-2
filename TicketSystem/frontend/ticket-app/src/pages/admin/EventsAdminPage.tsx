import { useEffect, useState } from 'react';
import { getEvents, cancelEvent } from '../../api/events';
import { getApiError } from '../../api/client';
import type { EventDto } from '../../types';
import { formatCurrency, formatDateShort } from '../../lib/format';
import EventFormModal from './EventFormModal';

export default function EventsAdminPage() {
  const [events, setEvents] = useState<EventDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [showModal, setShowModal] = useState(false);
  const [editing, setEditing] = useState<EventDto | null>(null);

  const load = () => {
    setLoading(true);
    // Sin filtro de status: trae activos y cancelados.
    getEvents()
      .then(setEvents)
      .catch((e) => setError(getApiError(e)))
      .finally(() => setLoading(false));
  };

  useEffect(load, []);

  const openCreate = () => {
    setEditing(null);
    setShowModal(true);
  };

  const openEdit = (event: EventDto) => {
    setEditing(event);
    setShowModal(true);
  };

  const handleCancel = async (event: EventDto) => {
    if (!window.confirm(`¿Cancelar el evento "${event.name}"?`)) return;
    try {
      await cancelEvent(event.id);
      load();
    } catch (e) {
      alert(getApiError(e));
    }
  };

  const handleSaved = () => {
    setShowModal(false);
    setEditing(null);
    load();
  };

  return (
    <div>
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-2xl font-bold text-slate-800">Eventos</h1>
        <button
          onClick={openCreate}
          className="px-4 py-2 rounded-lg bg-indigo-600 text-white hover:bg-indigo-700"
        >
          + Crear evento
        </button>
      </div>

      {loading && <p className="text-slate-400">Cargando...</p>}
      {error && <p className="text-rose-600">{error}</p>}

      {!loading && !error && (
        <div className="bg-white rounded-2xl border border-slate-200 overflow-hidden">
          <table className="w-full text-sm">
            <thead>
              <tr className="text-left text-slate-500 bg-slate-50 border-b border-slate-200">
                <th className="px-4 py-3">Evento</th>
                <th className="px-4 py-3">Fecha</th>
                <th className="px-4 py-3">Lugar</th>
                <th className="px-4 py-3">Estado</th>
                <th className="px-4 py-3">Zonas</th>
                <th className="px-4 py-3 text-right">Acciones</th>
              </tr>
            </thead>
            <tbody>
              {events.map((event) => (
                <tr key={event.id} className="border-b border-slate-100">
                  <td className="px-4 py-3 font-medium text-slate-800">{event.name}</td>
                  <td className="px-4 py-3 text-slate-600">{formatDateShort(event.date)}</td>
                  <td className="px-4 py-3 text-slate-600">{event.venue}</td>
                  <td className="px-4 py-3">
                    <span
                      className={`text-xs px-2 py-1 rounded-full ${
                        event.status === 'Active'
                          ? 'bg-emerald-100 text-emerald-700'
                          : 'bg-rose-100 text-rose-700'
                      }`}
                    >
                      {event.status === 'Active' ? 'Activo' : 'Cancelado'}
                    </span>
                  </td>
                  <td className="px-4 py-3 text-slate-600">
                    {event.zones.map((z) => (
                      <span key={z.id} className="block text-xs">
                        {z.zoneType}: {formatCurrency(z.price)} ({z.availableCapacity}/
                        {z.totalCapacity})
                      </span>
                    ))}
                  </td>
                  <td className="px-4 py-3">
                    <div className="flex justify-end gap-2">
                      <button
                        onClick={() => openEdit(event)}
                        className="px-3 py-1.5 rounded-lg border border-slate-300 text-slate-600 hover:bg-slate-50 text-xs"
                      >
                        Editar
                      </button>
                      {event.status === 'Active' && (
                        <button
                          onClick={() => handleCancel(event)}
                          className="px-3 py-1.5 rounded-lg border border-rose-300 text-rose-600 hover:bg-rose-50 text-xs"
                        >
                          Cancelar
                        </button>
                      )}
                    </div>
                  </td>
                </tr>
              ))}
              {events.length === 0 && (
                <tr>
                  <td colSpan={6} className="px-4 py-8 text-center text-slate-400">
                    No hay eventos. Crea el primero.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      )}

      {showModal && (
        <EventFormModal event={editing} onClose={() => setShowModal(false)} onSaved={handleSaved} />
      )}
    </div>
  );
}
