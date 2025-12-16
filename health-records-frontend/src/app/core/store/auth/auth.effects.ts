import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, switchMap, tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { TokenService } from '../../services/token.service';
import * as AuthActions from './auth.actions';

/**
 * Effects de autenticación
 * Maneja todos los side effects relacionados con autenticación:
 * - Llamadas HTTP
 * - Guardado en localStorage
 * - Navegación
 */
@Injectable()
export class AuthEffects {
  private actions$ = inject(Actions);
  private apiService = inject(ApiService);
  private router = inject(Router);
  private tokenService = inject(TokenService);

  /**
   * Effect para login
   * Escucha la acción login, hace la llamada HTTP y dispatcha success o failure
   */
  login$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.login),
      switchMap(({ username, password }) =>
        this.apiService.post<any>('auth/login', { username, password }).pipe(
          map(response => {
            if (response.success && response.data) {
              // Guardar tokens usando TokenService
              this.tokenService.saveTokens(
                response.data.accessToken,
                response.data.refreshToken,
                response.data.expiresAt,
                response.data.refreshTokenExpiresAt,
                response.data.user
              );
              
              // Dispatch success action
              return AuthActions.loginSuccess({
                user: response.data.user,
                accessToken: response.data.accessToken,
                refreshToken: response.data.refreshToken,
                expiresAt: response.data.expiresAt,
                refreshTokenExpiresAt: response.data.refreshTokenExpiresAt
              });
            } else {
              return AuthActions.loginFailure({
                error: response.message || 'Error al iniciar sesión'
              });
            }
          }),
          catchError(error => of(AuthActions.loginFailure({
            error: error.error?.message || 'Error al iniciar sesión'
          })))
        )
      )
    )
  );

  /**
   * Effect para navegación después de login exitoso
   * No dispatcha acciones, solo hace side effects (navegación)
   */
  loginSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.loginSuccess),
        tap(() => {
          this.router.navigate(['/patients']);
        })
      ),
    { dispatch: false }
  );

  /**
   * Effect para verificar autenticación al iniciar la app
   */
  checkAuth$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.checkAuth),
      map(() => {
        const accessToken = this.tokenService.getAccessToken();
        const refreshToken = this.tokenService.getRefreshToken();
        const user = this.tokenService.getUser();
        const expiresAt = this.tokenService.getExpiresAt();
        const refreshExpiresAt = this.tokenService.getRefreshExpiresAt();

        // Verificar si hay tokens válidos
        if (this.tokenService.hasValidTokens() && user) {
          return AuthActions.setAuth({
            user,
            accessToken,
            refreshToken,
            expiresAt,
            refreshTokenExpiresAt: refreshExpiresAt
          });
        } else {
          // Tokens inválidos o expirados, limpiar
          this.tokenService.clearTokens();
          return AuthActions.logout();
        }
      })
    )
  );

  /**
   * Effect para logout
   * Llama al endpoint de logout y limpia todo
   */
  logout$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.logout),
      switchMap(() => {
        const refreshToken = this.tokenService.getRefreshToken();
        
        // Si hay refresh token, intentar llamar al endpoint de logout
        if (refreshToken && !this.tokenService.isRefreshTokenExpired()) {
          return this.apiService.post<any>('auth/logout', { refreshToken }).pipe(
            map(() => {
              this.tokenService.clearTokens();
              this.router.navigate(['/login']);
              return AuthActions.logoutSuccess();
            }),
            catchError(() => {
              // Si falla el logout en el servidor, limpiar localmente de todas formas
              this.tokenService.clearTokens();
              this.router.navigate(['/login']);
              return of(AuthActions.logoutSuccess());
            })
          );
        } else {
          // No hay refresh token válido, solo limpiar localmente
          this.tokenService.clearTokens();
          this.router.navigate(['/login']);
          return of(AuthActions.logoutSuccess());
        }
      })
    )
  );
}
