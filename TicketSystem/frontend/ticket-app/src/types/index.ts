export type EventStatus = 'Active' | 'Cancelled';
export type ZoneType = 'VIP' | 'Preferente' | 'General';

export interface Zone {
  id: number;
  zoneType: string;
  price: number;
  totalCapacity: number;
  availableCapacity: number;
}

export interface EventDto {
  id: number;
  name: string;
  description: string;
  date: string;
  venue: string;
  status: EventStatus;
  createdAt: string;
  zones: Zone[];
}

export interface PurchaseDto {
  id: number;
  eventId: number;
  zoneId: number;
  zoneType: string;
  buyerName: string;
  buyerEmail: string;
  quantity: number;
  totalAmount: number;
  purchasedAt: string;
}

export interface LoginResponse {
  token: string;
  email: string;
  role: string;
}

export interface EventRevenue {
  eventId: number;
  eventName: string;
  ticketsSold: number;
  revenue: number;
}

export interface ZoneSales {
  zoneType: string;
  ticketsSold: number;
  revenue: number;
}

export interface SalesDashboard {
  totalEvents: number;
  totalTicketsSold: number;
  totalSales: number;
  revenueByEvent: EventRevenue[];
  ticketsByZone: ZoneSales[];
}

// Payloads de escritura
export interface CreateZoneInput {
  zoneType: string;
  price: number;
  totalCapacity: number;
}

export interface CreateEventInput {
  name: string;
  description: string;
  date: string;
  venue: string;
  zones: CreateZoneInput[];
}

export interface EditZoneInput {
  id: number;
  price: number;
  totalCapacity: number;
}

export interface EditEventInput {
  id: number;
  name: string;
  description: string;
  date: string;
  venue: string;
  zones: EditZoneInput[];
}

export interface PurchaseInput {
  eventId: number;
  zoneId: number;
  buyerName: string;
  buyerEmail: string;
  quantity: number;
}
