import { createFeatureSelector, createSelector } from '@ngrx/store';
import { PatientsState } from '../../models/state.models';
import { selectAll, selectEntities } from './patients.reducer';

export const selectPatientsState = createFeatureSelector<PatientsState>('patients');

export const selectAllPatients = createSelector(
  selectPatientsState,
  selectAll
);

export const selectPatientsEntities = createSelector(
  selectPatientsState,
  selectEntities
);

export const selectSelectedPatientId = createSelector(
  selectPatientsState,
  (state: PatientsState) => state.selectedPatientId
);

export const selectSelectedPatient = createSelector(
  selectPatientsEntities,
  selectSelectedPatientId,
  (entities, id) => id ? entities[id] || null : null
);

/**
 * Selector para obtener un paciente por ID
 * Útil cuando necesitas obtener un paciente específico sin tenerlo en selectedPatientId
 */
export const selectPatientById = (id: number) => createSelector(
  selectPatientsEntities,
  (entities) => entities[id] || null
);

export const selectPatientsLoading = createSelector(
  selectPatientsState,
  (state: PatientsState) => state.loading
);

export const selectPatientsError = createSelector(
  selectPatientsState,
  (state: PatientsState) => state.error
);

