import { api } from './client';
import type { PurchaseDto, PurchaseInput } from '../types';

export async function purchaseTickets(input: PurchaseInput): Promise<PurchaseDto> {
  const { data } = await api.post<PurchaseDto>('/purchases', input);
  return data;
}

export async function getPurchasesByEvent(eventId: number): Promise<PurchaseDto[]> {
  const { data } = await api.get<PurchaseDto[]>(`/purchases/event/${eventId}`);
  return data;
}
