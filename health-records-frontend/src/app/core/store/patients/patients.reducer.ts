import { createReducer, on } from '@ngrx/store';
import { EntityAdapter, createEntityAdapter } from '@ngrx/entity';
import { Patient } from '../../models/patient.models';
import { PatientsState } from '../../models/state.models';
import * as PatientActions from './patients.actions';

export const adapter: EntityAdapter<Patient> = createEntityAdapter<Patient>({
  selectId: (patient: Patient) => patient.id
});

export const initialState: PatientsState = adapter.getInitialState({
  selectedPatientId: null,
  loading: false,
  error: null
});

export const patientsReducer = createReducer(
  initialState,
  on(PatientActions.loadPatients, (state) => ({
    ...state,
    loading: true,
    error: null
  })),
  on(PatientActions.loadPatientsSuccess, (state, { patients }) =>
    adapter.setAll(patients, {
      ...state,
      loading: false,
      error: null
    })
  ),
  on(PatientActions.loadPatientsFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),
  on(PatientActions.loadPatient, (state) => ({
    ...state,
    loading: true,
    error: null
  })),
  on(PatientActions.loadPatientSuccess, (state, { patient }) =>
    adapter.upsertOne(patient, {
      ...state,
      selectedPatientId: patient.id,
      loading: false,
      error: null
    })
  ),
  on(PatientActions.loadPatientFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),
  on(PatientActions.addPatientSuccess, (state, { patient }) =>
    adapter.addOne(patient, state)
  ),
  on(PatientActions.updatePatientSuccess, (state, { patient }) =>
    adapter.updateOne(
      { id: patient.id, changes: patient },
      state
    )
  ),
  on(PatientActions.deletePatientSuccess, (state, { id }) =>
    adapter.removeOne(id, state)
  )
);

export const { selectAll, selectEntities, selectIds, selectTotal } =
  adapter.getSelectors();

