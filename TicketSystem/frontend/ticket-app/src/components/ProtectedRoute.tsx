import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../auth/AuthContext';

/**
 * Envuelve rutas de administración: si no hay token, redirige al login.
 */
export default function ProtectedRoute() {
  const { isAuthenticated } = useAuth();
  return isAuthenticated ? <Outlet /> : <Navigate to="/admin/login" replace />;
}
