import { api } from './client';
import type { CreateEventInput, EditEventInput, EventDto } from '../types';

export interface EventFilters {
  search?: string;
  status?: string;
  dateFrom?: string;
  dateTo?: string;
}

export async function getEvents(filters: EventFilters = {}): Promise<EventDto[]> {
  const params: Record<string, string> = {};
  if (filters.search) params.search = filters.search;
  if (filters.status) params.status = filters.status;
  if (filters.dateFrom) params.dateFrom = filters.dateFrom;
  if (filters.dateTo) params.dateTo = filters.dateTo;

  const { data } = await api.get<EventDto[]>('/events', { params });
  return data;
}

export async function getEventById(id: number): Promise<EventDto> {
  const { data } = await api.get<EventDto>(`/events/${id}`);
  return data;
}

export async function createEvent(input: CreateEventInput): Promise<EventDto> {
  const { data } = await api.post<EventDto>('/events', input);
  return data;
}

export async function editEvent(input: EditEventInput): Promise<EventDto> {
  const { data } = await api.put<EventDto>(`/events/${input.id}`, input);
  return data;
}

export async function cancelEvent(id: number): Promise<void> {
  await api.delete(`/events/${id}/cancel`);
}
