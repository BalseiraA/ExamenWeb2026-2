import { useEffect, useMemo, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { getEventById } from '../../api/events';
import { purchaseTickets } from '../../api/purchases';
import { getApiError } from '../../api/client';
import type { EventDto, PurchaseDto } from '../../types';
import ZoneSelector from '../../components/ZoneSelector';
import { formatCurrency, formatDate } from '../../lib/format';

const purchaseSchema = z.object({
  buyerName: z.string().min(1, 'El nombre es obligatorio.'),
  buyerEmail: z.string().min(1, 'El email es obligatorio.').email('Email inválido.'),
  quantity: z
    .number({ message: 'Ingresa una cantidad válida.' })
    .int('Debe ser un número entero.')
    .min(1, 'Mínimo 1 boleto.')
    .max(10, 'Máximo 10 boletos por compra.'),
});

type PurchaseForm = z.infer<typeof purchaseSchema>;

export default function EventDetailPage() {
  const { id } = useParams<{ id: string }>();
  const eventId = Number(id);

  const [event, setEvent] = useState<EventDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [loadError, setLoadError] = useState<string | null>(null);

  const [selectedZoneId, setSelectedZoneId] = useState<number | null>(null);
  const [submitError, setSubmitError] = useState<string | null>(null);
  const [confirmation, setConfirmation] = useState<PurchaseDto | null>(null);

  const {
    register,
    handleSubmit,
    watch,
    reset,
    formState: { errors, isSubmitting },
  } = useForm<PurchaseForm>({
    resolver: zodResolver(purchaseSchema),
    defaultValues: { buyerName: '', buyerEmail: '', quantity: 1 },
  });

  const quantity = watch('quantity');

  const loadEvent = () => {
    setLoading(true);
    getEventById(eventId)
      .then((data) => {
        setEvent(data);
        const firstAvailable = data.zones.find((z) => z.availableCapacity > 0);
        setSelectedZoneId(firstAvailable?.id ?? null);
      })
      .catch((e) => setLoadError(getApiError(e)))
      .finally(() => setLoading(false));
  };

  useEffect(loadEvent, [eventId]);

  const selectedZone = useMemo(
    () => event?.zones.find((z) => z.id === selectedZoneId) ?? null,
    [event, selectedZoneId],
  );

  const total = selectedZone ? selectedZone.price * (Number(quantity) || 0) : 0;
  const isCancelled = event?.status === 'Cancelled';

  const onSubmit = async (data: PurchaseForm) => {
    setSubmitError(null);
    setConfirmation(null);
    if (!selectedZoneId) {
      setSubmitError('Selecciona una zona.');
      return;
    }
    const ok = window.confirm(
      `¿Confirmar compra de ${data.quantity} boleto(s) en ${selectedZone?.zoneType} por ${formatCurrency(total)}?`,
    );
    if (!ok) return;

    try {
      const result = await purchaseTickets({
        eventId,
        zoneId: selectedZoneId,
        buyerName: data.buyerName,
        buyerEmail: data.buyerEmail,
        quantity: data.quantity,
      });
      setConfirmation(result);
      reset({ buyerName: '', buyerEmail: '', quantity: 1 });
      loadEvent(); // refresca la disponibilidad
    } catch (e) {
      setSubmitError(getApiError(e));
    }
  };

  if (loading) return <p className="text-center text-slate-400 py-10">Cargando evento...</p>;
  if (loadError)
    return (
      <div className="text-center py-10">
        <p className="text-rose-600 mb-4">{loadError}</p>
        <Link to="/" className="text-indigo-600 hover:underline">
          ← Volver al inicio
        </Link>
      </div>
    );
  if (!event) return null;

  return (
    <div>
      <Link to="/" className="text-sm text-indigo-600 hover:underline">
        ← Volver a eventos
      </Link>

      <div className="grid lg:grid-cols-3 gap-8 mt-4">
        {/* Información del evento */}
        <div className="lg:col-span-2">
          <div className="h-48 bg-gradient-to-br from-indigo-500 to-purple-600 rounded-2xl flex items-center justify-center text-7xl mb-6">
            🎤
          </div>
          <div className="flex items-center gap-3">
            <h1 className="text-3xl font-bold text-slate-800">{event.name}</h1>
            {isCancelled && (
              <span className="text-sm px-3 py-1 rounded-full bg-rose-100 text-rose-700">
                Cancelado
              </span>
            )}
          </div>
          <p className="text-slate-500 mt-2">📍 {event.venue}</p>
          <p className="text-slate-500">📅 {formatDate(event.date)}</p>
          <p className="mt-4 text-slate-700 leading-relaxed">{event.description}</p>
        </div>

        {/* Panel de compra */}
        <div className="bg-white rounded-2xl border border-slate-200 p-6 h-fit lg:sticky lg:top-24">
          <h2 className="font-semibold text-lg text-slate-800 mb-4">Comprar boletos</h2>

          {isCancelled ? (
            <p className="text-rose-600 text-sm">
              Este evento fue cancelado. No es posible comprar boletos.
            </p>
          ) : confirmation ? (
            <div className="text-center py-4">
              <div className="text-5xl mb-2">✅</div>
              <p className="font-medium text-slate-800">¡Compra realizada!</p>
              <p className="text-sm text-slate-500 mt-1">
                {confirmation.quantity} boleto(s) en {confirmation.zoneType}
              </p>
              <p className="text-lg font-semibold text-indigo-600 mt-1">
                {formatCurrency(confirmation.totalAmount)}
              </p>
              <button
                onClick={() => setConfirmation(null)}
                className="mt-4 text-sm text-indigo-600 hover:underline"
              >
                Comprar más
              </button>
            </div>
          ) : (
            <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
              <div>
                <label className="block text-xs font-medium text-slate-500 mb-2">Zona</label>
                <ZoneSelector
                  zones={event.zones}
                  selectedZoneId={selectedZoneId}
                  onSelect={setSelectedZoneId}
                />
              </div>

              <div>
                <label className="block text-xs font-medium text-slate-500 mb-1">
                  Cantidad (máx. 10)
                </label>
                <input
                  type="number"
                  min={1}
                  max={10}
                  {...register('quantity', { valueAsNumber: true })}
                  className="w-full px-3 py-2 rounded-lg border border-slate-300 focus:outline-none focus:ring-2 focus:ring-indigo-500"
                />
                {errors.quantity && (
                  <p className="text-xs text-rose-600 mt-1">{errors.quantity.message}</p>
                )}
              </div>

              <div>
                <label className="block text-xs font-medium text-slate-500 mb-1">
                  Nombre del comprador
                </label>
                <input
                  type="text"
                  {...register('buyerName')}
                  className="w-full px-3 py-2 rounded-lg border border-slate-300 focus:outline-none focus:ring-2 focus:ring-indigo-500"
                />
                {errors.buyerName && (
                  <p className="text-xs text-rose-600 mt-1">{errors.buyerName.message}</p>
                )}
              </div>

              <div>
                <label className="block text-xs font-medium text-slate-500 mb-1">Email</label>
                <input
                  type="email"
                  {...register('buyerEmail')}
                  className="w-full px-3 py-2 rounded-lg border border-slate-300 focus:outline-none focus:ring-2 focus:ring-indigo-500"
                />
                {errors.buyerEmail && (
                  <p className="text-xs text-rose-600 mt-1">{errors.buyerEmail.message}</p>
                )}
              </div>

              {/* Resumen */}
              <div className="flex items-center justify-between pt-2 border-t border-slate-200">
                <span className="text-sm text-slate-500">Total</span>
                <span className="text-xl font-bold text-indigo-600">{formatCurrency(total)}</span>
              </div>

              {submitError && <p className="text-sm text-rose-600">{submitError}</p>}

              <button
                type="submit"
                disabled={isSubmitting || !selectedZone}
                className="w-full py-3 rounded-xl bg-indigo-600 text-white font-medium hover:bg-indigo-700 disabled:opacity-50 transition"
              >
                {isSubmitting ? 'Procesando...' : 'Comprar'}
              </button>
            </form>
          )}
        </div>
      </div>
    </div>
  );
}
