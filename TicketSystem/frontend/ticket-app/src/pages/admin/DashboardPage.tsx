import { useEffect, useState } from 'react';
import { getSalesDashboard } from '../../api/dashboard';
import { getApiError } from '../../api/client';
import type { SalesDashboard } from '../../types';
import { formatCurrency } from '../../lib/format';

function MetricCard({ label, value, icon }: { label: string; value: string; icon: string }) {
  return (
    <div className="bg-white rounded-2xl border border-slate-200 p-6">
      <div className="text-3xl mb-2">{icon}</div>
      <p className="text-2xl font-bold text-slate-800">{value}</p>
      <p className="text-sm text-slate-500">{label}</p>
    </div>
  );
}

export default function DashboardPage() {
  const [data, setData] = useState<SalesDashboard | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    getSalesDashboard()
      .then(setData)
      .catch((e) => setError(getApiError(e)))
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <p className="text-slate-400">Cargando métricas...</p>;
  if (error) return <p className="text-rose-600">{error}</p>;
  if (!data) return null;

  const maxZoneRevenue = Math.max(1, ...data.ticketsByZone.map((z) => z.revenue));

  return (
    <div>
      <h1 className="text-2xl font-bold text-slate-800 mb-6">Dashboard de ventas</h1>

      {/* Métricas */}
      <div className="grid gap-4 sm:grid-cols-3 mb-8">
        <MetricCard label="Eventos totales" value={String(data.totalEvents)} icon="🎫" />
        <MetricCard label="Boletos vendidos" value={String(data.totalTicketsSold)} icon="🎟️" />
        <MetricCard label="Ventas totales" value={formatCurrency(data.totalSales)} icon="💰" />
      </div>

      <div className="grid gap-8 lg:grid-cols-2">
        {/* Tabla de ingresos por evento */}
        <div className="bg-white rounded-2xl border border-slate-200 p-6">
          <h2 className="font-semibold text-slate-800 mb-4">Ingresos por evento</h2>
          <table className="w-full text-sm">
            <thead>
              <tr className="text-left text-slate-500 border-b border-slate-200">
                <th className="pb-2">Evento</th>
                <th className="pb-2 text-right">Boletos</th>
                <th className="pb-2 text-right">Ingresos</th>
              </tr>
            </thead>
            <tbody>
              {data.revenueByEvent.map((row) => (
                <tr key={row.eventId} className="border-b border-slate-100">
                  <td className="py-2 text-slate-700">{row.eventName}</td>
                  <td className="py-2 text-right text-slate-600">{row.ticketsSold}</td>
                  <td className="py-2 text-right font-medium text-slate-800">
                    {formatCurrency(row.revenue)}
                  </td>
                </tr>
              ))}
              {data.revenueByEvent.length === 0 && (
                <tr>
                  <td colSpan={3} className="py-4 text-center text-slate-400">
                    Sin datos
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>

        {/* Gráfica de ventas por zona (barras CSS) */}
        <div className="bg-white rounded-2xl border border-slate-200 p-6">
          <h2 className="font-semibold text-slate-800 mb-4">Ventas por zona</h2>
          {data.ticketsByZone.length === 0 ? (
            <p className="text-slate-400 text-sm">Aún no hay ventas registradas.</p>
          ) : (
            <div className="space-y-4">
              {data.ticketsByZone.map((zone) => (
                <div key={zone.zoneType}>
                  <div className="flex justify-between text-sm mb-1">
                    <span className="text-slate-700">{zone.zoneType}</span>
                    <span className="text-slate-500">
                      {zone.ticketsSold} boletos · {formatCurrency(zone.revenue)}
                    </span>
                  </div>
                  <div className="h-3 bg-slate-100 rounded-full overflow-hidden">
                    <div
                      className="h-full bg-indigo-500 rounded-full transition-all"
                      style={{ width: `${(zone.revenue / maxZoneRevenue) * 100}%` }}
                    />
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
