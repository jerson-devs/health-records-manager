import { ApplicationConfig, importProvidersFrom, isDevMode } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptorsFromDi, HTTP_INTERCEPTORS } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { routes } from './app.routes';
import { reducers, metaReducers } from './core/store';
import { AuthEffects } from './core/store/auth/auth.effects';
import { PatientsEffects } from './core/store/patients/patients.effects';
import { MedicalRecordsEffects } from './core/store/medical-records/medical-records.effects';
import { JwtInterceptor } from './core/interceptors/jwt.interceptor';
import { AuthService } from './core/services/auth.service';
import { Store } from '@ngrx/store';
import * as AuthActions from './core/store/auth/auth.actions';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi()),
    provideAnimations(),
    provideStore(reducers, { metaReducers }),
    provideEffects([AuthEffects, PatientsEffects, MedicalRecordsEffects]),
    provideStoreDevtools({ maxAge: 25, logOnly: !isDevMode() }),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    }
  ]
};
