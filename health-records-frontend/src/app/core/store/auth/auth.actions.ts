import { createAction, props } from '@ngrx/store';
import { UserInfo } from '../../models/auth.models';

export const login = createAction(
  '[Auth] Login',
  props<{ username: string; password: string }>()
);

export const loginSuccess = createAction(
  '[Auth] Login Success',
  props<{ 
    user: UserInfo; 
    accessToken: string;
    refreshToken: string;
    expiresAt: string;
    refreshTokenExpiresAt: string;
  }>()
);

export const loginFailure = createAction(
  '[Auth] Login Failure',
  props<{ error: string }>()
);

export const refreshToken = createAction(
  '[Auth] Refresh Token',
  props<{ refreshToken: string }>()
);

export const refreshTokenSuccess = createAction(
  '[Auth] Refresh Token Success',
  props<{ 
    accessToken: string;
    refreshToken: string;
    expiresAt: string;
    refreshTokenExpiresAt: string;
  }>()
);

export const refreshTokenFailure = createAction(
  '[Auth] Refresh Token Failure',
  props<{ error: string }>()
);

export const logout = createAction('[Auth] Logout');

export const logoutSuccess = createAction('[Auth] Logout Success');

export const checkAuth = createAction('[Auth] Check Auth');

export const setAuth = createAction(
  '[Auth] Set Auth',
  props<{ 
    user: UserInfo | null; 
    accessToken: string | null;
    refreshToken: string | null;
    expiresAt: string | null;
    refreshTokenExpiresAt: string | null;
  }>()
);




