import { createFeatureSelector, createSelector } from '@ngrx/store';
import { MedicalRecordsState } from '../../models/state.models';
import { selectAll, selectEntities } from './medical-records.reducer';

export const selectMedicalRecordsState = createFeatureSelector<MedicalRecordsState>('medicalRecords');

export const selectAllRecords = createSelector(
  selectMedicalRecordsState,
  selectAll
);

export const selectRecordsEntities = createSelector(
  selectMedicalRecordsState,
  selectEntities
);

/**
 * Selector para obtener historiales por ID de paciente
 * Usa una factory function para crear un selector dinámico
 */
export const selectRecordsByPatientId = (patientId: number) => 
  createSelector(
    selectAllRecords,
    (records) => records.filter(record => record.patientId === patientId)
  );

export const selectSelectedRecordId = createSelector(
  selectMedicalRecordsState,
  (state: MedicalRecordsState) => state.selectedRecordId
);

export const selectSelectedRecord = createSelector(
  selectRecordsEntities,
  selectSelectedRecordId,
  (entities, id) => id ? entities[id] || null : null
);

export const selectRecordsLoading = createSelector(
  selectMedicalRecordsState,
  (state: MedicalRecordsState) => state.loading
);

export const selectRecordsError = createSelector(
  selectMedicalRecordsState,
  (state: MedicalRecordsState) => state.error
);

