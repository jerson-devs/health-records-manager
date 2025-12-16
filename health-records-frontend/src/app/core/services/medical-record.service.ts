import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { MedicalRecord, CreateMedicalRecordRequest, UpdateMedicalRecordRequest } from '../models/medical-record.models';

/**
 * Servicio de historiales médicos - Solo maneja llamadas HTTP
 * NO debe conocer el Store ni hacer dispatch de acciones
 * Siguiendo el patrón Redux: los servicios son solo una capa de abstracción HTTP
 */
@Injectable({
  providedIn: 'root'
})
export class MedicalRecordService {
  constructor(private apiService: ApiService) {}

  /**
   * Obtiene todos los historiales médicos
   * El dispatch de acciones se hace en los componentes o Effects
   */
  getAll(): Observable<any> {
    return this.apiService.get<MedicalRecord[]>('medicalrecords');
  }

  /**
   * Obtiene un historial médico por ID
   */
  getById(id: number): Observable<any> {
    return this.apiService.get<MedicalRecord>(`medicalrecords/${id}`);
  }

  /**
   * Obtiene historiales médicos por ID de paciente
   * El dispatch de acciones se hace en los componentes o Effects
   */
  getByPatientId(patientId: number): Observable<any> {
    return this.apiService.get<MedicalRecord[]>(`patients/${patientId}/records`);
  }

  /**
   * Crea un nuevo historial médico
   * El dispatch de acciones se hace en los componentes o Effects
   */
  create(request: CreateMedicalRecordRequest): Observable<any> {
    return this.apiService.post<MedicalRecord>('medicalrecords', request);
  }

  /**
   * Actualiza un historial médico existente
   * El dispatch de acciones se hace en los componentes o Effects
   */
  update(id: number, request: UpdateMedicalRecordRequest): Observable<any> {
    return this.apiService.put<MedicalRecord>(`medicalrecords/${id}`, request);
  }

  /**
   * Elimina un historial médico
   * El dispatch de acciones se hace en los componentes o Effects
   */
  delete(id: number): Observable<any> {
    return this.apiService.delete<MedicalRecord>(`medicalrecords/${id}`);
  }
}
