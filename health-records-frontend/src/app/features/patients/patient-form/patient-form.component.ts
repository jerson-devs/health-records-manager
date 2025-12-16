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
import { MatProgressSpinnerModule, MatSpinner } from '@angular/material/progress-spinner';
import { CreatePatientRequest, UpdatePatientRequest } from '../../../core/models/patient.models';
import { selectSelectedPatient, selectPatientsLoading } from '../../../core/store/patients/patients.selectors';
import * as PatientActions from '../../../core/store/patients/patients.actions';

/**
 * Componente de formulario de paciente
 * Siguiendo patrón Redux:
 * - Dispatcha acciones para cargar/crear/actualizar
 * - Se suscribe a selectores para obtener datos y estado
 * - NO hace llamadas HTTP directamente
 */
@Component({
  selector: 'app-patient-form',
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
    MatProgressSpinnerModule,
    MatSpinner
  ],
  templateUrl: './patient-form.component.html',
  styleUrl: './patient-form.component.scss'
})
export class PatientFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private store = inject(Store);

  patientForm: FormGroup;
  patientId: number | null = null;
  isEditMode = false;
  loading$: Observable<boolean> = this.store.select(selectPatientsLoading);

  constructor() {
    this.patientForm = this.fb.group({
      nombre: ['', [Validators.required, Validators.maxLength(200)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(100)]],
      fechaNacimiento: ['', [Validators.required]],
      documento: ['', [Validators.required, Validators.maxLength(50)]]
    });
  }

  ngOnInit(): void {
    this.patientId = this.route.snapshot.params['id'];
    this.isEditMode = !!this.patientId;

    if (this.isEditMode && this.patientId) {
      // Dispatch acción para cargar paciente
      this.store.dispatch(PatientActions.loadPatient({ id: this.patientId }));
      
      // Suscribirse al selector para obtener el paciente cargado
      this.store.select(selectSelectedPatient)
        .pipe(
          filter(patient => patient !== null && patient.id === this.patientId),
          take(1)
        )
        .subscribe(patient => {
          if (patient) {
            this.patientForm.patchValue({
              nombre: patient.nombre,
              email: patient.email,
              fechaNacimiento: new Date(patient.fechaNacimiento),
              documento: patient.documento
            });
          }
        });
    }
  }

  onSubmit(): void {
    if (this.patientForm.valid) {
      const formValue = this.patientForm.value;
      const patientData = {
        ...formValue,
        fechaNacimiento: formValue.fechaNacimiento.toISOString()
      };

      if (this.isEditMode && this.patientId) {
        // Dispatch acción para actualizar
        this.store.dispatch(PatientActions.updatePatient({
          id: this.patientId,
          patient: patientData as UpdatePatientRequest
        }));
      } else {
        // Dispatch acción para crear
        this.store.dispatch(PatientActions.addPatient({
          patient: patientData as CreatePatientRequest
        }));
      }

      // Escuchar cuando la operación sea exitosa para navegar
      this.store.select(selectPatientsLoading)
        .pipe(
          filter(loading => !loading),
          take(1)
        )
        .subscribe(() => {
          this.router.navigate(['/patients']);
        });
    }
  }

  onCancel(): void {
    this.router.navigate(['/patients']);
  }
}
