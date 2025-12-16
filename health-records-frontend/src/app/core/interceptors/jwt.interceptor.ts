import { Injectable, inject } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError, BehaviorSubject, EMPTY } from 'rxjs';
import { catchError, switchMap, filter, take } from 'rxjs/operators';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { TokenService } from '../services/token.service';
import { ApiService } from '../services/api.service';
import * as AuthActions from '../store/auth/auth.actions';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  private router = inject(Router);
  private store = inject(Store);
  private tokenService = inject(TokenService);
  private apiService = inject(ApiService);
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // No agregar token a las rutas de autenticación
    if (request.url.includes('/auth/login') || request.url.includes('/auth/refresh')) {
      return next.handle(request);
    }

    const accessToken = this.tokenService.getAccessToken();

    // Si hay token y no está expirado, agregarlo al header
    if (accessToken && !this.tokenService.isAccessTokenExpired()) {
      request = this.addTokenToRequest(request, accessToken);
    } else if (accessToken && this.tokenService.isAccessTokenExpired()) {
      // Token expirado, intentar renovarlo
      return this.handleTokenRefresh(request, next);
    }

    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401 && !request.url.includes('/auth/login')) {
          // Token inválido, intentar renovar
          return this.handleTokenRefresh(request, next);
        }
        return throwError(() => error);
      })
    );
  }

  private addTokenToRequest(request: HttpRequest<unknown>, token: string): HttpRequest<unknown> {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  private handleTokenRefresh(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      const refreshToken = this.tokenService.getRefreshToken();

      if (!refreshToken || this.tokenService.isRefreshTokenExpired()) {
        // Refresh token expirado o no existe, hacer logout
        this.isRefreshing = false;
        this.store.dispatch(AuthActions.logout());
        this.router.navigate(['/login']);
        return EMPTY;
      }

      // Intentar renovar el token
      return this.apiService.post<any>('auth/refresh', { refreshToken }).pipe(
        switchMap((response) => {
          this.isRefreshing = false;

          if (response.success && response.data) {
            // Guardar nuevos tokens
            this.tokenService.updateTokens(
              response.data.accessToken,
              response.data.refreshToken,
              response.data.expiresAt,
              response.data.refreshTokenExpiresAt
            );

            // Actualizar el store
            this.store.dispatch(AuthActions.refreshTokenSuccess({
              accessToken: response.data.accessToken,
              refreshToken: response.data.refreshToken,
              expiresAt: response.data.expiresAt,
              refreshTokenExpiresAt: response.data.refreshTokenExpiresAt
            }));

            this.refreshTokenSubject.next(response.data.accessToken);

            // Reintentar la petición original con el nuevo token
            return next.handle(this.addTokenToRequest(request, response.data.accessToken));
          } else {
            // Error al renovar, hacer logout
            this.store.dispatch(AuthActions.logout());
            this.router.navigate(['/login']);
            return EMPTY;
          }
        }),
        catchError((error) => {
          this.isRefreshing = false;
          this.store.dispatch(AuthActions.logout());
          this.router.navigate(['/login']);
          return EMPTY;
        })
      );
    } else {
      // Ya se está renovando, esperar a que termine
      return this.refreshTokenSubject.pipe(
        filter(token => token !== null),
        take(1),
        switchMap((token) => {
          return next.handle(this.addTokenToRequest(request, token));
        })
      );
    }
  }
}
