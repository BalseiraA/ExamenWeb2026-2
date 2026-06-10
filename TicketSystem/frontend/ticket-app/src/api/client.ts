import axios from 'axios';

const TOKEN_KEY = 'ticket_token';

// Lee la URL de la API desde la variable de entorno de Vite (.env).
const baseURL = import.meta.env.VITE_API_URL ?? 'http://localhost:5227/api';

export const api = axios.create({ baseURL });

// Interceptor de petición: adjunta el JWT (si existe) en cada llamada.
api.interceptors.request.use((config) => {
  const token = localStorage.getItem(TOKEN_KEY);
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Interceptor de respuesta: si el token expiró o es inválido, lo limpia.
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem(TOKEN_KEY);
    }
    return Promise.reject(error);
  },
);

export { TOKEN_KEY };

/** Extrae un mensaje de error legible desde la respuesta ProblemDetails del backend. */
export function getApiError(error: unknown): string {
  if (axios.isAxiosError(error)) {
    const data = error.response?.data as { detail?: string; title?: string } | undefined;
    return data?.detail ?? data?.title ?? error.message;
  }
  return 'Ocurrió un error inesperado.';
}
