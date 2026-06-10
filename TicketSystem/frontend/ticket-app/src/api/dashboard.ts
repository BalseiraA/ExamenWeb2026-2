import { api } from './client';
import type { SalesDashboard } from '../types';

export async function getSalesDashboard(): Promise<SalesDashboard> {
  const { data } = await api.get<SalesDashboard>('/dashboard/sales');
  return data;
}
