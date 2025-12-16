import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { MedicalRecordService } from '../../services/medical-record.service';
import { UpdateMedicalRecordRequest } from '../../models/medical-record.models';
import * as MedicalRecordActions from './medical-records.actions';

/**
 * Effects de historiales médicos
 * Maneja todos los side effects relacionados con historiales médicos:
 * - Llamadas HTTP
 * - Transformación de datos
 * - Dispatch de acciones de éxito/error
 */
@Injectable()
export class MedicalRecordsEffects {
  private actions$ = inject(Actions);
  private medicalRecordService = inject(MedicalRecordService);

  /**
   * Effect para cargar todos los historiales médicos
   */
  loadRecords$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MedicalRecordActions.loadRecords),
      switchMap(() =>
        this.medicalRecordService.getAll().pipe(
          map(response => {
            if (response.success && response.data) {
              return MedicalRecordActions.loadRecordsSuccess({
                records: response.data
              });
            } else {
              return MedicalRecordActions.loadRecordsFailure({
                error: response.message || 'Error al cargar historiales'
              });
            }
          }),
          catchError(error =>
            of(MedicalRecordActions.loadRecordsFailure({
              error: error.error?.message || 'Error al cargar historiales'
            }))
          )
        )
      )
    )
  );

  /**
   * Effect para cargar historiales por paciente
   */
  loadRecordsByPatient$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MedicalRecordActions.loadRecordsByPatient),
      switchMap(({ patientId }) =>
        this.medicalRecordService.getByPatientId(patientId).pipe(
          map(response => {
            if (response.success && response.data) {
              // El endpoint devuelve un Patient con medicalRecords dentro
              const patient = response.data;
              const medicalRecordsSummary = patient.medicalRecords || [];
              
              // Mapear MedicalRecordSummary a MedicalRecord completo
              // El summary ahora incluye todos los campos necesarios
              const records = medicalRecordsSummary.map((summary: any) => ({
                id: summary.id,
                patientId: patient.id,
                patientName: patient.nombre,
                fecha: summary.fecha,
                diagnostico: summary.diagnostico,
                tratamiento: summary.tratamiento || '',
                medico: summary.medico,
                createdAt: summary.createdAt || patient.createdAt || new Date().toISOString(),
                updatedAt: summary.updatedAt || null
              }));
              
              return MedicalRecordActions.loadRecordsByPatientSuccess({
                records: records,
                patientId: patientId
              });
            } else {
              // Si no hay datos, devolver array vacío
              return MedicalRecordActions.loadRecordsByPatientSuccess({
                records: [],
                patientId: patientId
              });
            }
          }),
          catchError(error =>
            of(MedicalRecordActions.loadRecordsFailure({
              error: error.error?.message || 'Error al cargar historiales del paciente'
            }))
          )
        )
      )
    )
  );

  /**
   * Effect para crear un historial médico
   */
  addRecord$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MedicalRecordActions.addRecord),
      switchMap(({ record }) =>
        this.medicalRecordService.create(record).pipe(
          map(response => {
            if (response.success && response.data) {
              return MedicalRecordActions.addRecordSuccess({
                record: response.data
              });
            } else {
              return MedicalRecordActions.loadRecordsFailure({
                error: response.message || 'Error al crear historial'
              });
            }
          }),
          catchError(error =>
            of(MedicalRecordActions.loadRecordsFailure({
              error: error.error?.message || 'Error al crear historial'
            }))
          )
        )
      )
    )
  );

  /**
   * Effect para recargar todos los historiales después de crear uno nuevo
   * Esto asegura que la lista completa esté actualizada
   */
  addRecordSuccess$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MedicalRecordActions.addRecordSuccess),
      switchMap(() => [
        // Recargar todos los historiales para asegurar que la lista esté actualizada
        MedicalRecordActions.loadRecords()
      ])
    )
  );

  /**
   * Effect para actualizar un historial médico
   */
  updateRecord$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MedicalRecordActions.updateRecord),
      switchMap(({ id, record }) => {
        // Mapear Partial<MedicalRecord> a UpdateMedicalRecordRequest
        // Validar que todos los campos requeridos estén presentes
        if (!record.fecha || !record.diagnostico || !record.tratamiento || !record.medico) {
          return of(MedicalRecordActions.loadRecordsFailure({
            error: 'Todos los campos son requeridos para actualizar el historial'
          }));
        }
        
        const updateRequest: UpdateMedicalRecordRequest = {
          fecha: record.fecha,
          diagnostico: record.diagnostico,
          tratamiento: record.tratamiento,
          medico: record.medico
        };
        
        return this.medicalRecordService.update(id, updateRequest).pipe(
          map(response => {
            if (response.success && response.data) {
              return MedicalRecordActions.updateRecordSuccess({
                record: response.data
              });
            } else {
              return MedicalRecordActions.loadRecordsFailure({
                error: response.message || 'Error al actualizar historial'
              });
            }
          }),
          catchError(error =>
            of(MedicalRecordActions.loadRecordsFailure({
              error: error.error?.message || 'Error al actualizar historial'
            }))
          )
        );
      })
    )
  );

  /**
   * Effect para eliminar un historial médico
   */
  deleteRecord$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MedicalRecordActions.deleteRecord),
      switchMap(({ id }) =>
        this.medicalRecordService.delete(id).pipe(
          map(response => {
            if (response.success) {
              return MedicalRecordActions.deleteRecordSuccess({ id });
            } else {
              return MedicalRecordActions.loadRecordsFailure({
                error: response.message || 'Error al eliminar historial'
              });
            }
          }),
          catchError(error =>
            of(MedicalRecordActions.loadRecordsFailure({
              error: error.error?.message || 'Error al eliminar historial'
            }))
          )
        )
      )
    )
  );
}
