import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { filter, take } from 'rxjs/operators';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule, MatSpinner } from '@angular/material/progress-spinner';
import { CreateMedicalRecordRequest, MedicalRecord, UpdateMedicalRecordRequest } from '../../../core/models/medical-record.models';
import { selectAllPatients, selectPatientsLoading } from '../../../core/store/patients/patients.selectors';
import { selectSelectedRecord, selectRecordsLoading } from '../../../core/store/medical-records/medical-records.selectors';
import * as PatientActions from '../../../core/store/patients/patients.actions';
import * as MedicalRecordActions from '../../../core/store/medical-records/medical-records.actions';
import { MedicalRecordService } from '../../../core/services/medical-record.service';

/**
 * Componente de formulario de historial médico
 * Siguiendo patrón Redux:
 * - Dispatcha acciones para cargar/crear/actualizar
 * - Se suscribe a selectores para obtener datos
 * - NO hace llamadas HTTP directamente
 */
@Component({
  selector: 'app-record-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    MatSpinner
  ],
  templateUrl: './record-form.component.html',
  styleUrl: './record-form.component.scss'
})
export class RecordFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private store = inject(Store);

  recordForm: FormGroup;
  recordId: number | null = null;
  isEditMode = false;
  patients$: Observable<any[]> = this.store.select(selectAllPatients);
  loading$: Observable<boolean> = this.store.select(selectRecordsLoading);
  selectedPatientId: number | null = null;
  recordPatientId: number | null = null; // Para guardar el patientId del record cuando se edita
  returnUrl: string | null = null; // Para guardar la URL de retorno (ej: /medical-records)
  
  // Para casos donde necesitamos cargar un record específico (edición)
  // Por ahora usamos el servicio directamente, pero idealmente debería ser una acción
  private medicalRecordService = inject(MedicalRecordService);

  constructor() {
    this.recordForm = this.fb.group({
      patientId: ['', [Validators.required]],
      fecha: [new Date(), [Validators.required]],
      diagnostico: ['', [Validators.required, Validators.maxLength(500)]],
      tratamiento: ['', [Validators.required, Validators.maxLength(1000)]],
      medico: ['', [Validators.required, Validators.maxLength(200)]]
    });
  }

  ngOnInit(): void {
    // Cargar pacientes si no están en el store
    this.store.dispatch(PatientActions.loadPatients());

    this.recordId = this.route.snapshot.params['id'];
    this.isEditMode = !!this.recordId;
    this.selectedPatientId = +this.route.snapshot.queryParams['patientId'] || null;
    
    // Obtener returnUrl del state de navegación (history.state persiste después de la navegación)
    this.returnUrl = (history.state && history.state['returnUrl']) || null;

    if (this.isEditMode && this.recordId) {
      // Cargar record para edición
      // TODO: En un patrón Redux completo, esto debería ser una acción loadRecord
      // Por ahora usamos el servicio directamente para casos específicos
      this.loadRecord(this.recordId);
    } else if (this.selectedPatientId) {
      this.recordForm.patchValue({ patientId: this.selectedPatientId });
      this.recordForm.get('patientId')?.disable();
    }
  }

  onSubmit(): void {
    if (this.recordForm.valid) {
      const formValue = this.recordForm.getRawValue();
      const recordData = {
        ...formValue,
        fecha: formValue.fecha.toISOString()
      };

      if (this.isEditMode && this.recordId) {
        // Dispatch acción para actualizar
        this.store.dispatch(MedicalRecordActions.updateRecord({
          id: this.recordId,
          record: recordData as UpdateMedicalRecordRequest
        }));
      } else {
        // Dispatch acción para crear
        this.store.dispatch(MedicalRecordActions.addRecord({
          record: recordData as CreateMedicalRecordRequest
        }));
      }

      // Escuchar cuando la operación sea exitosa para navegar
      this.store.select(selectRecordsLoading)
        .pipe(
          filter(loading => !loading),
          take(1)
        )
        .subscribe(() => {
          // Si hay returnUrl (viene de /medical-records), redirigir ahí
          if (this.returnUrl) {
            this.router.navigate([this.returnUrl]);
          } else {
            // Determinar el patientId: primero queryParams, luego del record si está en edición
            const patientId = this.selectedPatientId || this.recordPatientId;
            
            // Si viene de una vista de paciente específico, redirigir ahí
            if (patientId) {
              // Recargar los datos del paciente y sus historiales antes de navegar
              this.store.dispatch(PatientActions.loadPatient({ id: patientId }));
              this.store.dispatch(MedicalRecordActions.loadRecordsByPatient({ patientId }));
              setTimeout(() => {
                this.router.navigate(['/patients', patientId]);
              }, 100);
            } else {
              // Si viene de /records/new sin patientId, redirigir a /medical-records
              this.router.navigate(['/medical-records']);
            }
          }
        });
    }
  }

  onCancel(): void {
    // Si hay returnUrl (viene de /medical-records), redirigir ahí
    if (this.returnUrl) {
      this.router.navigate([this.returnUrl]);
    } else {
      // Determinar el patientId: primero queryParams, luego del record si está en edición
      const patientId = this.selectedPatientId || this.recordPatientId;
      
      // Si viene de una vista de paciente específico, redirigir ahí
      if (patientId) {
        this.router.navigate(['/patients', patientId]);
      } else {
        // Si viene de /records/new sin patientId, redirigir a /medical-records
        this.router.navigate(['/medical-records']);
      }
    }
  }

  /**
   * Carga un record específico para edición
   * En un patrón Redux completo, esto debería ser una acción
   */
  private loadRecord(id: number): void {
    this.medicalRecordService.getById(id).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          const record = response.data;
          // Guardar el patientId del record para poder redirigir después
          this.recordPatientId = record.patientId;
          
          this.recordForm.patchValue({
            patientId: record.patientId,
            fecha: new Date(record.fecha),
            diagnostico: record.diagnostico,
            tratamiento: record.tratamiento,
            medico: record.medico
          });
          this.recordForm.get('patientId')?.disable();
        }
      }
    });
  }
}
