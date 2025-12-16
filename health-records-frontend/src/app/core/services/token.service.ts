import { Injectable } from '@angular/core';

/**
 * Servicio para gestión profesional de tokens JWT
 * Maneja:
 * - Almacenamiento seguro de tokens
 * - Verificación de expiración
 * - Decodificación de tokens
 * - Renovación automática
 */
@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private readonly ACCESS_TOKEN_KEY = 'access_token';
  private readonly REFRESH_TOKEN_KEY = 'refresh_token';
  private readonly EXPIRES_AT_KEY = 'expires_at';
  private readonly REFRESH_EXPIRES_AT_KEY = 'refresh_expires_at';
  private readonly USER_KEY = 'user';

  /**
   * Guarda los tokens y datos del usuario
   */
  saveTokens(accessToken: string, refreshToken: string, expiresAt: string, refreshExpiresAt: string, user: any): void {
    localStorage.setItem(this.ACCESS_TOKEN_KEY, accessToken);
    localStorage.setItem(this.REFRESH_TOKEN_KEY, refreshToken);
    localStorage.setItem(this.EXPIRES_AT_KEY, expiresAt);
    localStorage.setItem(this.REFRESH_EXPIRES_AT_KEY, refreshExpiresAt);
    localStorage.setItem(this.USER_KEY, JSON.stringify(user));
  }

  /**
   * Obtiene el access token
   */
  getAccessToken(): string | null {
    return localStorage.getItem(this.ACCESS_TOKEN_KEY);
  }

  /**
   * Obtiene el refresh token
   */
  getRefreshToken(): string | null {
    return localStorage.getItem(this.REFRESH_TOKEN_KEY);
  }

  /**
   * Obtiene la fecha de expiración del access token
   */
  getExpiresAt(): string | null {
    return localStorage.getItem(this.EXPIRES_AT_KEY);
  }

  /**
   * Obtiene la fecha de expiración del refresh token
   */
  getRefreshExpiresAt(): string | null {
    return localStorage.getItem(this.REFRESH_EXPIRES_AT_KEY);
  }

  /**
   * Obtiene el usuario almacenado
   */
  getUser(): any {
    const userStr = localStorage.getItem(this.USER_KEY);
    return userStr ? JSON.parse(userStr) : null;
  }

  /**
   * Verifica si el access token está expirado
   */
  isAccessTokenExpired(): boolean {
    const token = this.getAccessToken();
    if (!token) {
      return true;
    }

    try {
      // Verificar expiración decodificando el token
      const decoded = this.decodeToken(token);
      if (!decoded || !decoded.exp) {
        return true;
      }

      // Verificar expiración del token (exp está en segundos Unix)
      const expirationTime = decoded.exp * 1000; // Convertir a milisegundos
      const now = Date.now();
      // Agregar buffer de 1 minuto antes de la expiración real
      const bufferTime = 60 * 1000; // 1 minuto en milisegundos
      if (expirationTime - now < bufferTime) {
        return true;
      }

      // Verificar también la fecha guardada
      const expiresAt = this.getExpiresAt();
      if (expiresAt) {
        const expirationDate = new Date(expiresAt);
        return expirationDate.getTime() - now < bufferTime;
      }

      return false;
    } catch (error) {
      return true;
    }
  }

  /**
   * Verifica si el refresh token está expirado
   */
  isRefreshTokenExpired(): boolean {
    const token = this.getRefreshToken();
    if (!token) {
      return true;
    }

    try {
      const decoded = this.decodeToken(token);
      if (!decoded || !decoded.exp) {
        return true;
      }

      // Verificar expiración del token
      const expirationTime = decoded.exp * 1000;
      const now = Date.now();
      if (expirationTime <= now) {
        return true;
      }

      const expiresAt = this.getRefreshExpiresAt();
      if (expiresAt) {
        const expirationDate = new Date(expiresAt);
        return expirationDate.getTime() <= now;
      }

      return false;
    } catch (error) {
      return true;
    }
  }

  /**
   * Verifica si hay tokens válidos
   */
  hasValidTokens(): boolean {
    const accessToken = this.getAccessToken();
    const refreshToken = this.getRefreshToken();
    
    if (!accessToken || !refreshToken) {
      return false;
    }

    // Si el refresh token está expirado, no hay tokens válidos
    if (this.isRefreshTokenExpired()) {
      return false;
    }

    return true;
  }

  /**
   * Decodifica el token para obtener información
   */
  decodeToken(token: string): any {
    try {
      const base64Url = token.split('.')[1];
      if (!base64Url) {
        return null;
      }
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      const jsonPayload = decodeURIComponent(
        atob(base64)
          .split('')
          .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
          .join('')
      );
      return JSON.parse(jsonPayload);
    } catch (error) {
      return null;
    }
  }

  /**
   * Limpia todos los tokens y datos del usuario
   */
  clearTokens(): void {
    localStorage.removeItem(this.ACCESS_TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_TOKEN_KEY);
    localStorage.removeItem(this.EXPIRES_AT_KEY);
    localStorage.removeItem(this.REFRESH_EXPIRES_AT_KEY);
    localStorage.removeItem(this.USER_KEY);
  }

  /**
   * Actualiza solo el access token (para renovación)
   */
  updateAccessToken(accessToken: string, expiresAt: string): void {
    localStorage.setItem(this.ACCESS_TOKEN_KEY, accessToken);
    localStorage.setItem(this.EXPIRES_AT_KEY, expiresAt);
  }

  /**
   * Actualiza todos los tokens (para renovación completa)
   */
  updateTokens(accessToken: string, refreshToken: string, expiresAt: string, refreshExpiresAt: string): void {
    this.saveTokens(accessToken, refreshToken, expiresAt, refreshExpiresAt, this.getUser());
  }
}
