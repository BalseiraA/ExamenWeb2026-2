import { Link } from 'react-router-dom';
import type { EventDto } from '../types';
import { formatCurrency, formatDate } from '../lib/format';

export default function EventCard({ event }: { event: EventDto }) {
  const minPrice = event.zones.length > 0 ? Math.min(...event.zones.map((z) => z.price)) : 0;
  const isCancelled = event.status === 'Cancelled';

  return (
    <Link
      to={`/events/${event.id}`}
      className="block bg-white rounded-2xl border border-slate-200 overflow-hidden hover:shadow-lg hover:-translate-y-0.5 transition"
    >
      <div className="h-32 bg-gradient-to-br from-indigo-500 to-purple-600 flex items-center justify-center text-5xl">
        🎤
      </div>
      <div className="p-5">
        <div className="flex items-start justify-between gap-2">
          <h3 className="font-semibold text-lg text-slate-800">{event.name}</h3>
          {isCancelled && (
            <span className="shrink-0 text-xs px-2 py-1 rounded-full bg-rose-100 text-rose-700">
              Cancelado
            </span>
          )}
        </div>
        <p className="text-sm text-slate-500 mt-1">📍 {event.venue}</p>
        <p className="text-sm text-slate-500">📅 {formatDate(event.date)}</p>

        <div className="mt-4 flex items-center justify-between">
          <div className="flex gap-1.5">
            {event.zones.map((z) => (
              <span
                key={z.id}
                className="text-xs px-2 py-1 rounded-full bg-slate-100 text-slate-600"
              >
                {z.zoneType}
              </span>
            ))}
          </div>
          <span className="text-sm font-medium text-indigo-600">
            desde {formatCurrency(minPrice)}
          </span>
        </div>
      </div>
    </Link>
  );
}
