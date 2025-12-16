import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { PatientService } from '../../services/patient.service';
import { UpdatePatientRequest } from '../../models/patient.models';
import * as PatientActions from './patients.actions';

/**
 * Effects de pacientes
 * Maneja todos los side effects relacionados con pacientes:
 * - Llamadas HTTP
 * - Transformación de datos
 * - Dispatch de acciones de éxito/error
 */
@Injectable()
export class PatientsEffects {
  private actions$ = inject(Actions);
  private patientService = inject(PatientService);

  /**
   * Effect para cargar todos los pacientes
   * Escucha loadPatients, hace la llamada HTTP y dispatcha success o failure
   */
  loadPatients$ = createEffect(() =>
    this.actions$.pipe(
      ofType(PatientActions.loadPatients),
      switchMap(() =>
        this.patientService.getAll().pipe(
          map(response => {
            if (response.success && response.data) {
              return PatientActions.loadPatientsSuccess({
                patients: response.data
              });
            } else {
              return PatientActions.loadPatientsFailure({
                error: response.message || 'Error al cargar pacientes'
              });
            }
          }),
          catchError(error =>
            of(PatientActions.loadPatientsFailure({
              error: error.error?.message || 'Error al cargar pacientes'
            }))
          )
        )
      )
    )
  );

  /**
   * Effect para cargar un paciente por ID
   */
  loadPatient$ = createEffect(() =>
    this.actions$.pipe(
      ofType(PatientActions.loadPatient),
      switchMap(({ id }) =>
        this.patientService.getById(id).pipe(
          map(response => {
            if (response.success && response.data) {
              return PatientActions.loadPatientSuccess({
                patient: response.data
              });
            } else {
              return PatientActions.loadPatientsFailure({
                error: response.message || 'Error al cargar paciente'
              });
            }
          }),
          catchError(error =>
            of(PatientActions.loadPatientsFailure({
              error: error.error?.message || 'Error al cargar paciente'
            }))
          )
        )
      )
    )
  );

  /**
   * Effect para crear un paciente
   * Escucha addPatient, hace la llamada HTTP y dispatcha success o failure
   */
  addPatient$ = createEffect(() =>
    this.actions$.pipe(
      ofType(PatientActions.addPatient),
      switchMap(({ patient }) =>
        this.patientService.create(patient).pipe(
          map(response => {
            if (response.success && response.data) {
              return PatientActions.addPatientSuccess({
                patient: response.data
              });
            } else {
              return PatientActions.loadPatientsFailure({
                error: response.message || 'Error al crear paciente'
              });
            }
          }),
          catchError(error =>
            of(PatientActions.loadPatientsFailure({
              error: error.error?.message || 'Error al crear paciente'
            }))
          )
        )
      )
    )
  );

  /**
   * Effect para actualizar un paciente
   */
  updatePatient$ = createEffect(() =>
    this.actions$.pipe(
      ofType(PatientActions.updatePatient),
      switchMap(({ id, patient }) => {
        // Mapear Partial<Patient> a UpdatePatientRequest
        // Validar que todos los campos requeridos estén presentes
        if (!patient.nombre || !patient.email || !patient.fechaNacimiento || !patient.documento) {
          return of(PatientActions.loadPatientsFailure({
            error: 'Todos los campos son requeridos para actualizar el paciente'
          }));
        }
        
        const updateRequest: UpdatePatientRequest = {
          nombre: patient.nombre,
          email: patient.email,
          fechaNacimiento: patient.fechaNacimiento,
          documento: patient.documento
        };
        
        return this.patientService.update(id, updateRequest).pipe(
          map(response => {
            if (response.success && response.data) {
              return PatientActions.updatePatientSuccess({
                patient: response.data
              });
            } else {
              return PatientActions.loadPatientsFailure({
                error: response.message || 'Error al actualizar paciente'
              });
            }
          }),
          catchError(error =>
            of(PatientActions.loadPatientsFailure({
              error: error.error?.message || 'Error al actualizar paciente'
            }))
          )
        );
      })
    )
  );

  /**
   * Effect para eliminar un paciente
   */
  deletePatient$ = createEffect(() =>
    this.actions$.pipe(
      ofType(PatientActions.deletePatient),
      switchMap(({ id }) =>
        this.patientService.delete(id).pipe(
          map(response => {
            if (response.success) {
              return PatientActions.deletePatientSuccess({ id });
            } else {
              return PatientActions.loadPatientsFailure({
                error: response.message || 'Error al eliminar paciente'
              });
            }
          }),
          catchError(error =>
            of(PatientActions.loadPatientsFailure({
              error: error.error?.message || 'Error al eliminar paciente'
            }))
          )
        )
      )
    )
  );
}
