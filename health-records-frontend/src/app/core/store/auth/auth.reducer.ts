import { createReducer, on } from '@ngrx/store';
import { AuthState } from '../../models/auth.models';
import * as AuthActions from './auth.actions';

export const initialState: AuthState = {
  user: null,
  accessToken: null,
  refreshToken: null,
  expiresAt: null,
  refreshTokenExpiresAt: null,
  isAuthenticated: false,
  loading: false,
  error: null
};

export const authReducer = createReducer(
  initialState,
  on(AuthActions.login, (state) => ({
    ...state,
    loading: true,
    error: null
  })),
  on(AuthActions.loginSuccess, (state, { user, accessToken, refreshToken, expiresAt, refreshTokenExpiresAt }) => ({
    ...state,
    user,
    accessToken,
    refreshToken,
    expiresAt,
    refreshTokenExpiresAt,
    isAuthenticated: true,
    loading: false,
    error: null
  })),
  on(AuthActions.loginFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
    isAuthenticated: false
  })),
  on(AuthActions.refreshToken, (state) => ({
    ...state,
    loading: true,
    error: null
  })),
  on(AuthActions.refreshTokenSuccess, (state, { accessToken, refreshToken, expiresAt, refreshTokenExpiresAt }) => ({
    ...state,
    accessToken,
    refreshToken,
    expiresAt,
    refreshTokenExpiresAt,
    loading: false,
    error: null
  })),
  on(AuthActions.refreshTokenFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
    isAuthenticated: false
  })),
  on(AuthActions.logout, (state) => ({
    ...state,
    user: null,
    accessToken: null,
    refreshToken: null,
    expiresAt: null,
    refreshTokenExpiresAt: null,
    isAuthenticated: false,
    error: null
  })),
  on(AuthActions.setAuth, (state, { user, accessToken, refreshToken, expiresAt, refreshTokenExpiresAt }) => ({
    ...state,
    user,
    accessToken,
    refreshToken,
    expiresAt,
    refreshTokenExpiresAt,
    isAuthenticated: !!accessToken && !!refreshToken && !!user
  }))
);




