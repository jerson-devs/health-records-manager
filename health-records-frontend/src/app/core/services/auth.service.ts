import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { LoginRequest, LoginResponse } from '../models/auth.models';

/**
 * Servicio de autenticación - Solo maneja llamadas HTTP
 * NO debe conocer el Store ni hacer dispatch de acciones
 * Siguiendo el patrón Redux: los servicios son solo una capa de abstracción HTTP
 */
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private apiService: ApiService) {}

  /**
   * Realiza la llamada HTTP de login
   * El manejo del estado se hace en AuthEffects
   */
  login(request: LoginRequest): Observable<any> {
    return this.apiService.post<LoginResponse>('auth/login', request);
  }

  /**
   * Guarda el token en localStorage
   * Llamado desde AuthEffects (side effect)
   */
  saveToken(token: string): void {
    localStorage.setItem('token', token);
  }

  /**
   * Guarda el usuario en localStorage
   * Llamado desde AuthEffects (side effect)
   */
  saveUser(user: any): void {
    localStorage.setItem('user', JSON.stringify(user));
  }

  /**
   * Limpia el localStorage
   * Llamado desde AuthEffects (side effect)
   */
  clearStorage(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  }

  /**
   * Obtiene el token del localStorage
   * Usado para verificar autenticación al iniciar la app
   */
  getToken(): string | null {
    return localStorage.getItem('token');
  }

  /**
   * Obtiene el usuario del localStorage
   * Usado para verificar autenticación al iniciar la app
   */
  getUser(): any {
    const userStr = localStorage.getItem('user');
    return userStr ? JSON.parse(userStr) : null;
  }
}
