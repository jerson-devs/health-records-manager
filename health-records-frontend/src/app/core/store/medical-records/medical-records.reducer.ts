import { createReducer, on } from '@ngrx/store';
import { EntityAdapter, createEntityAdapter } from '@ngrx/entity';
import { MedicalRecord } from '../../models/medical-record.models';
import { MedicalRecordsState } from '../../models/state.models';
import * as MedicalRecordActions from './medical-records.actions';

export const adapter: EntityAdapter<MedicalRecord> = createEntityAdapter<MedicalRecord>({
  selectId: (record: MedicalRecord) => record.id
});

export const initialState: MedicalRecordsState = adapter.getInitialState({
  selectedRecordId: null,
  loading: false,
  error: null
});

export const medicalRecordsReducer = createReducer(
  initialState,
  on(MedicalRecordActions.loadRecords, (state) => ({
    ...state,
    loading: true,
    error: null
  })),
  on(MedicalRecordActions.loadRecordsSuccess, (state, { records }) => {
    // Asegurar que records sea un array
    const recordsArray = Array.isArray(records) ? records : [];
    
    return adapter.setAll(recordsArray, {
      ...state,
      loading: false,
      error: null
    });
  }),
  on(MedicalRecordActions.loadRecordsFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),
  on(MedicalRecordActions.loadRecordsByPatient, (state) => ({
    ...state,
    loading: true,
    error: null
  })),
  on(MedicalRecordActions.loadRecordsByPatientSuccess, (state, { records, patientId }) => {
    // Asegurar que records sea un array
    const recordsArray = Array.isArray(records) ? records : [];
    
    // Obtener todos los records actuales accediendo directamente a las entidades
    const currentRecords: MedicalRecord[] = [];
    if (state.ids && Array.isArray(state.ids)) {
      state.ids.forEach(id => {
        const record = state.entities[id];
        if (record && record.patientId !== patientId) {
          currentRecords.push(record);
        }
      });
    }
    
    // Combinar los records a mantener con los nuevos del paciente actual
    // Si no hay records para este paciente, solo mantenemos los de otros pacientes
    const allRecords = [...currentRecords, ...recordsArray];
    
    return adapter.setAll(allRecords, {
      ...state,
      loading: false,
      error: null
    });
  }),
  on(MedicalRecordActions.addRecordSuccess, (state, { record }) =>
    adapter.addOne(record, state)
  ),
  on(MedicalRecordActions.updateRecordSuccess, (state, { record }) =>
    adapter.updateOne(
      { id: record.id, changes: record },
      state
    )
  ),
  on(MedicalRecordActions.deleteRecordSuccess, (state, { id }) =>
    adapter.removeOne(id, state)
  )
);

export const { selectAll, selectEntities, selectIds, selectTotal } =
  adapter.getSelectors();




