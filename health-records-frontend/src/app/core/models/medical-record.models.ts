export interface MedicalRecord {
  id: number;
  patientId: number;
  patientName: string;
  fecha: string;
  diagnostico: string;
  tratamiento: string;
  medico: string;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateMedicalRecordRequest {
  patientId: number;
  fecha: string;
  diagnostico: string;
  tratamiento: string;
  medico: string;
}

export interface UpdateMedicalRecordRequest {
  fecha: string;
  diagnostico: string;
  tratamiento: string;
  medico: string;
}




