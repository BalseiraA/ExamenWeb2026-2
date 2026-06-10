import { createContext, useContext, useState, type ReactNode } from 'react';
import { TOKEN_KEY } from '../api/client';
import { login as loginRequest } from '../api/auth';

interface AuthState {
  token: string | null;
  email: string | null;
  isAuthenticated: boolean;
  login: (email: string, password: string) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthState | undefined>(undefined);

const EMAIL_KEY = 'ticket_email';

export function AuthProvider({ children }: { children: ReactNode }) {
  const [token, setToken] = useState<string | null>(() => localStorage.getItem(TOKEN_KEY));
  const [email, setEmail] = useState<string | null>(() => localStorage.getItem(EMAIL_KEY));

  const login = async (emailInput: string, password: string) => {
    const result = await loginRequest(emailInput, password);
    localStorage.setItem(TOKEN_KEY, result.token);
    localStorage.setItem(EMAIL_KEY, result.email);
    setToken(result.token);
    setEmail(result.email);
  };

  const logout = () => {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(EMAIL_KEY);
    setToken(null);
    setEmail(null);
  };

  return (
    <AuthContext.Provider value={{ token, email, isAuthenticated: !!token, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

// eslint-disable-next-line react-refresh/only-export-components
export function useAuth(): AuthState {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth debe usarse dentro de <AuthProvider>');
  return ctx;
}
