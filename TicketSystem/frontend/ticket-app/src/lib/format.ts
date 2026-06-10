export function formatCurrency(value: number): string {
  return new Intl.NumberFormat('es-MX', {
    style: 'currency',
    currency: 'MXN',
    minimumFractionDigits: 2,
  }).format(value);
}

export function formatDate(iso: string): string {
  const date = new Date(iso);
  return new Intl.DateTimeFormat('es-MX', {
    dateStyle: 'long',
    timeStyle: 'short',
  }).format(date);
}

export function formatDateShort(iso: string): string {
  const date = new Date(iso);
  return new Intl.DateTimeFormat('es-MX', { dateStyle: 'medium' }).format(date);
}
