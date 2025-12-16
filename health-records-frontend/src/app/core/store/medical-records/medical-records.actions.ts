import { createAction, props } from '@ngrx/store';
import { MedicalRecord, CreateMedicalRecordRequest } from '../../models/medical-record.models';

export const loadRecords = createAction('[MedicalRecords] Load Records');

export const loadRecordsSuccess = createAction(
  '[MedicalRecords] Load Records Success',
  props<{ records: MedicalRecord[] }>()
);

export const loadRecordsFailure = createAction(
  '[MedicalRecords] Load Records Failure',
  props<{ error: string }>()
);

export const loadRecordsByPatient = createAction(
  '[MedicalRecords] Load Records By Patient',
  props<{ patientId: number }>()
);

export const loadRecordsByPatientSuccess = createAction(
  '[MedicalRecords] Load Records By Patient Success',
  props<{ records: MedicalRecord[]; patientId: number }>()
);

export const addRecord = createAction(
  '[MedicalRecords] Add Record',
  props<{ record: CreateMedicalRecordRequest }>()
);

export const addRecordSuccess = createAction(
  '[MedicalRecords] Add Record Success',
  props<{ record: MedicalRecord }>()
);

export const updateRecord = createAction(
  '[MedicalRecords] Update Record',
  props<{ id: number; record: Partial<MedicalRecord> }>()
);

export const updateRecordSuccess = createAction(
  '[MedicalRecords] Update Record Success',
  props<{ record: MedicalRecord }>()
);

export const deleteRecord = createAction(
  '[MedicalRecords] Delete Record',
  props<{ id: number }>()
);

export const deleteRecordSuccess = createAction(
  '[MedicalRecords] Delete Record Success',
  props<{ id: number }>()
);




