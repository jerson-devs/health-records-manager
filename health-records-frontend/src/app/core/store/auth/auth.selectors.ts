import { createFeatureSelector, createSelector } from '@ngrx/store';
import { AuthState } from '../../models/auth.models';

export const selectAuthState = createFeatureSelector<AuthState>('auth');

export const selectUser = createSelector(
  selectAuthState,
  (state: AuthState) => state.user
);

export const selectAccessToken = createSelector(
  selectAuthState,
  (state: AuthState) => state.accessToken
);

export const selectRefreshToken = createSelector(
  selectAuthState,
  (state: AuthState) => state.refreshToken
);

// Mantener selectToken para compatibilidad (retorna accessToken)
export const selectToken = createSelector(
  selectAuthState,
  (state: AuthState) => state.accessToken
);

export const selectIsAuthenticated = createSelector(
  selectAuthState,
  (state: AuthState) => state.isAuthenticated
);

export const selectAuthLoading = createSelector(
  selectAuthState,
  (state: AuthState) => state.loading
);

export const selectAuthError = createSelector(
  selectAuthState,
  (state: AuthState) => state.error
);




