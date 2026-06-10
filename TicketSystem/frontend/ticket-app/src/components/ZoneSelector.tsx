import type { Zone } from '../types';
import { formatCurrency } from '../lib/format';

interface Props {
  zones: Zone[];
  selectedZoneId: number | null;
  onSelect: (zoneId: number) => void;
}

export default function ZoneSelector({ zones, selectedZoneId, onSelect }: Props) {
  return (
    <div className="space-y-2">
      {zones.map((zone) => {
        const soldOut = zone.availableCapacity <= 0;
        const selected = zone.id === selectedZoneId;
        return (
          <button
            key={zone.id}
            type="button"
            disabled={soldOut}
            onClick={() => onSelect(zone.id)}
            className={`w-full flex items-center justify-between p-4 rounded-xl border-2 text-left transition ${
              selected
                ? 'border-indigo-600 bg-indigo-50'
                : 'border-slate-200 hover:border-slate-300'
            } ${soldOut ? 'opacity-50 cursor-not-allowed' : ''}`}
          >
            <div>
              <p className="font-medium text-slate-800">{zone.zoneType}</p>
              <p className="text-xs text-slate-500">
                {soldOut ? 'Agotado' : `${zone.availableCapacity} disponibles`}
              </p>
            </div>
            <span className="font-semibold text-indigo-600">{formatCurrency(zone.price)}</span>
          </button>
        );
      })}
    </div>
  );
}
