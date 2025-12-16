export interface Patient {
  id: number;
  nombre: string;
  email: string;
  fechaNacimiento: string;
  documento: string;
  createdAt: string;
  updatedAt?: string;
  medicalRecords?: MedicalRecordSummary[];
}

export interface MedicalRecordSummary {
  id: number;
  fecha: string;
  diagnostico: string;
  medico: string;
}

export interface CreatePatientRequest {
  nombre: string;
  email: string;
  fechaNacimiento: string;
  documento: string;
}

export interface UpdatePatientRequest {
  nombre: string;
  email: string;
  fechaNacimiento: string;
  documento: string;
}




