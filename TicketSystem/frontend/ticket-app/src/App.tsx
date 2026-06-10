import { Routes, Route, Navigate } from 'react-router-dom';
import PublicLayout from './components/PublicLayout';
import AdminLayout from './components/AdminLayout';
import ProtectedRoute from './components/ProtectedRoute';
import HomePage from './pages/public/HomePage';
import EventDetailPage from './pages/public/EventDetailPage';
import LoginPage from './pages/admin/LoginPage';
import DashboardPage from './pages/admin/DashboardPage';
import EventsAdminPage from './pages/admin/EventsAdminPage';

export default function App() {
  return (
    <Routes>
      {/* Portal público */}
      <Route element={<PublicLayout />}>
        <Route path="/" element={<HomePage />} />
        <Route path="/events/:id" element={<EventDetailPage />} />
      </Route>

      {/* Login admin (sin layout) */}
      <Route path="/admin/login" element={<LoginPage />} />

      {/* Área protegida de administración */}
      <Route element={<ProtectedRoute />}>
        <Route element={<AdminLayout />}>
          <Route path="/admin/dashboard" element={<DashboardPage />} />
          <Route path="/admin/events" element={<EventsAdminPage />} />
        </Route>
      </Route>

      {/* Cualquier otra ruta → inicio */}
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}
