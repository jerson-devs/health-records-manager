import { createAction, props } from '@ngrx/store';
import { Patient, CreatePatientRequest, UpdatePatientRequest } from '../../models/patient.models';

export const loadPatients = createAction('[Patients] Load Patients');

export const loadPatientsSuccess = createAction(
  '[Patients] Load Patients Success',
  props<{ patients: Patient[] }>()
);

export const loadPatientsFailure = createAction(
  '[Patients] Load Patients Failure',
  props<{ error: string }>()
);

export const loadPatient = createAction(
  '[Patients] Load Patient',
  props<{ id: number }>()
);

export const loadPatientSuccess = createAction(
  '[Patients] Load Patient Success',
  props<{ patient: Patient }>()
);

export const loadPatientFailure = createAction(
  '[Patients] Load Patient Failure',
  props<{ error: string }>()
);

export const addPatient = createAction(
  '[Patients] Add Patient',
  props<{ patient: CreatePatientRequest }>()
);

export const addPatientSuccess = createAction(
  '[Patients] Add Patient Success',
  props<{ patient: Patient }>()
);

export const updatePatient = createAction(
  '[Patients] Update Patient',
  props<{ id: number; patient: UpdatePatientRequest }>()
);

export const updatePatientSuccess = createAction(
  '[Patients] Update Patient Success',
  props<{ patient: Patient }>()
);

export const deletePatient = createAction(
  '[Patients] Delete Patient',
  props<{ id: number }>()
);

export const deletePatientSuccess = createAction(
  '[Patients] Delete Patient Success',
  props<{ id: number }>()
);

