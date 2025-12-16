import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Patient, CreatePatientRequest, UpdatePatientRequest } from '../models/patient.models';

/**
 * Servicio de pacientes - Solo maneja llamadas HTTP
 * NO debe conocer el Store ni hacer dispatch de acciones
 * Siguiendo el patrón Redux: los servicios son solo una capa de abstracción HTTP
 */
@Injectable({
  providedIn: 'root'
})
export class PatientService {
  constructor(private apiService: ApiService) {}

  /**
   * Obtiene todos los pacientes
   * El dispatch de acciones se hace en los componentes o Effects
   */
  getAll(): Observable<any> {
    return this.apiService.get<Patient[]>('patients');
  }

  /**
   * Obtiene un paciente por ID
   */
  getById(id: number): Observable<any> {
    return this.apiService.get<Patient>(`patients/${id}`);
  }

  /**
   * Obtiene un paciente con sus historiales médicos
   */
  getByIdWithRecords(id: number): Observable<any> {
    return this.apiService.get<Patient>(`patients/${id}/records`);
  }

  /**
   * Crea un nuevo paciente
   * El dispatch de acciones se hace en los componentes o Effects
   */
  create(request: CreatePatientRequest): Observable<any> {
    return this.apiService.post<Patient>('patients', request);
  }

  /**
   * Actualiza un paciente existente
   * El dispatch de acciones se hace en los componentes o Effects
   */
  update(id: number, request: UpdatePatientRequest): Observable<any> {
    return this.apiService.put<Patient>(`patients/${id}`, request);
  }

  /**
   * Elimina un paciente
   * El dispatch de acciones se hace en los componentes o Effects
   */
  delete(id: number): Observable<any> {
    return this.apiService.delete<Patient>(`patients/${id}`);
  }
}
