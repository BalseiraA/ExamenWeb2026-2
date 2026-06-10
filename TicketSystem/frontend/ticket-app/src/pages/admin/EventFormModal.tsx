import { useState } from 'react';
import { useForm, useFieldArray } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { createEvent, editEvent } from '../../api/events';
import { getApiError } from '../../api/client';
import type { EventDto } from '../../types';

const ZONE_TYPES = ['VIP', 'Preferente', 'General'] as const;

const zoneSchema = z.object({
  id: z.number().optional(),
  zoneType: z.string(),
  price: z.number({ message: 'Inválido' }).min(0.01, 'Mínimo $0.01'),
  totalCapacity: z.number({ message: 'Inválido' }).int('Entero').min(1, 'Mínimo 1'),
});

const eventSchema = z.object({
  name: z.string().min(1, 'El nombre es obligatorio.'),
  description: z.string(),
  date: z.string().min(1, 'La fecha es obligatoria.'),
  venue: z.string().min(1, 'El lugar es obligatorio.'),
  zones: z.array(zoneSchema).min(1, 'Configura al menos una zona.'),
});

type EventForm = z.infer<typeof eventSchema>;

interface Props {
  /** Si se pasa, el modal está en modo edición. */
  event?: EventDto | null;
  onClose: () => void;
  onSaved: () => void;
}

function toLocalInput(iso: string): string {
  // Convierte ISO a formato datetime-local (YYYY-MM-DDTHH:mm).
  return iso.slice(0, 16);
}

export default function EventFormModal({ event, onClose, onSaved }: Props) {
  const isEdit = !!event;
  const [error, setError] = useState<string | null>(null);

  const {
    register,
    handleSubmit,
    control,
    formState: { errors, isSubmitting },
  } = useForm<EventForm>({
    resolver: zodResolver(eventSchema),
    defaultValues: isEdit
      ? {
          name: event!.name,
          description: event!.description,
          date: toLocalInput(event!.date),
          venue: event!.venue,
          zones: event!.zones.map((z) => ({
            id: z.id,
            zoneType: z.zoneType,
            price: z.price,
            totalCapacity: z.totalCapacity,
          })),
        }
      : {
          name: '',
          description: '',
          date: '',
          venue: '',
          zones: ZONE_TYPES.map((t) => ({ zoneType: t, price: 1, totalCapacity: 100 })),
        },
  });

  const { fields } = useFieldArray({ control, name: 'zones' });

  const onSubmit = async (data: EventForm) => {
    setError(null);
    try {
      if (isEdit) {
        await editEvent({
          id: event!.id,
          name: data.name,
          description: data.description,
          date: new Date(data.date).toISOString(),
          venue: data.venue,
          zones: data.zones.map((z) => ({
            id: z.id!,
            price: z.price,
            totalCapacity: z.totalCapacity,
          })),
        });
      } else {
        await createEvent({
          name: data.name,
          description: data.description,
          date: new Date(data.date).toISOString(),
          venue: data.venue,
          zones: data.zones.map((z) => ({
            zoneType: z.zoneType,
            price: z.price,
            totalCapacity: z.totalCapacity,
          })),
        });
      }
      onSaved();
    } catch (e) {
      setError(getApiError(e));
    }
  };

  return (
    <div className="fixed inset-0 bg-black/40 flex items-center justify-center z-50 p-4">
      <div className="bg-white rounded-2xl w-full max-w-lg max-h-[90vh] overflow-auto">
        <div className="flex items-center justify-between p-6 border-b border-slate-200">
          <h2 className="font-semibold text-lg text-slate-800">
            {isEdit ? 'Editar evento' : 'Crear evento'}
          </h2>
          <button onClick={onClose} className="text-slate-400 hover:text-slate-600 text-xl">
            ✕
          </button>
        </div>

        <form onSubmit={handleSubmit(onSubmit)} className="p-6 space-y-4">
          <div>
            <label className="block text-xs font-medium text-slate-500 mb-1">Nombre</label>
            <input
              {...register('name')}
              className="w-full px-3 py-2 rounded-lg border border-slate-300 focus:outline-none focus:ring-2 focus:ring-indigo-500"
            />
            {errors.name && <p className="text-xs text-rose-600 mt-1">{errors.name.message}</p>}
          </div>

          <div>
            <label className="block text-xs font-medium text-slate-500 mb-1">Descripción</label>
            <textarea
              rows={2}
              {...register('description')}
              className="w-full px-3 py-2 rounded-lg border border-slate-300 focus:outline-none focus:ring-2 focus:ring-indigo-500"
            />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-xs font-medium text-slate-500 mb-1">Fecha y hora</label>
              <input
                type="datetime-local"
                {...register('date')}
                className="w-full px-3 py-2 rounded-lg border border-slate-300 focus:outline-none focus:ring-2 focus:ring-indigo-500"
              />
              {errors.date && <p className="text-xs text-rose-600 mt-1">{errors.date.message}</p>}
            </div>
            <div>
              <label className="block text-xs font-medium text-slate-500 mb-1">Lugar</label>
              <input
                {...register('venue')}
                className="w-full px-3 py-2 rounded-lg border border-slate-300 focus:outline-none focus:ring-2 focus:ring-indigo-500"
              />
              {errors.venue && <p className="text-xs text-rose-600 mt-1">{errors.venue.message}</p>}
            </div>
          </div>

          <div>
            <label className="block text-xs font-medium text-slate-500 mb-2">
              Zonas (precio y capacidad)
            </label>
            <div className="space-y-2">
              {fields.map((field, index) => (
                <div key={field.id} className="grid grid-cols-3 gap-2 items-center">
                  <span className="text-sm text-slate-700">{field.zoneType}</span>
                  <div>
                    <input
                      type="number"
                      step="0.01"
                      placeholder="Precio"
                      {...register(`zones.${index}.price`, { valueAsNumber: true })}
                      className="w-full px-2 py-1.5 rounded-lg border border-slate-300 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
                    />
                    {errors.zones?.[index]?.price && (
                      <p className="text-[10px] text-rose-600">
                        {errors.zones[index]?.price?.message}
                      </p>
                    )}
                  </div>
                  <div>
                    <input
                      type="number"
                      placeholder="Capacidad"
                      {...register(`zones.${index}.totalCapacity`, { valueAsNumber: true })}
                      className="w-full px-2 py-1.5 rounded-lg border border-slate-300 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
                    />
                    {errors.zones?.[index]?.totalCapacity && (
                      <p className="text-[10px] text-rose-600">
                        {errors.zones[index]?.totalCapacity?.message}
                      </p>
                    )}
                  </div>
                </div>
              ))}
            </div>
          </div>

          {error && <p className="text-sm text-rose-600">{error}</p>}

          <div className="flex justify-end gap-3 pt-2">
            <button
              type="button"
              onClick={onClose}
              className="px-4 py-2 rounded-lg border border-slate-300 text-slate-600 hover:bg-slate-50"
            >
              Cancelar
            </button>
            <button
              type="submit"
              disabled={isSubmitting}
              className="px-4 py-2 rounded-lg bg-indigo-600 text-white hover:bg-indigo-700 disabled:opacity-50"
            >
              {isSubmitting ? 'Guardando...' : isEdit ? 'Guardar cambios' : 'Crear evento'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
