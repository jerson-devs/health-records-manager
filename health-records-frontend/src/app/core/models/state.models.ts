import { EntityState } from '@ngrx/entity';
import { Patient } from './patient.models';
import { MedicalRecord } from './medical-record.models';
import { AuthState } from './auth.models';

/**
 * Estado de la aplicación completo
 * Contiene todos los slices de estado de la aplicación
 */
export interface AppState {
  auth: AuthState;
  patients: PatientsState;
  medicalRecords: MedicalRecordsState;
}

/**
 * Estado de pacientes
 * Extiende EntityState para aprovechar las utilidades de NgRx Entity
 */
export interface PatientsState extends EntityState<Patient> {
  selectedPatientId: number | null;
  loading: boolean;
  error: string | null;
}

/**
 * Estado de historiales médicos
 * Extiende EntityState para aprovechar las utilidades de NgRx Entity
 */
export interface MedicalRecordsState extends EntityState<MedicalRecord> {
  selectedRecordId: number | null;
  loading: boolean;
  error: string | null;
}
