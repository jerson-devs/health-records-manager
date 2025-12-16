import { ActionReducerMap, MetaReducer } from '@ngrx/store';
import { environment } from '../../../environments/environment';
import { authReducer } from './auth/auth.reducer';
import { patientsReducer } from './patients/patients.reducer';
import { medicalRecordsReducer } from './medical-records/medical-records.reducer';
import { AppState } from '../models/state.models';

export const reducers: ActionReducerMap<AppState> = {
  auth: authReducer,
  patients: patientsReducer,
  medicalRecords: medicalRecordsReducer
};

export const metaReducers: MetaReducer<AppState>[] = !environment.production ? [] : [];

// Re-export AppState para facilitar el acceso desde otros módulos
export type { AppState } from '../models/state.models';




