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
import { selectPatientById, selectPatientsLoading } from '../../../core/store/patients/patients.selectors';
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
  private returnUrl: string = '/patients'; // Ruta por defecto

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

    // Obtener la ruta de retorno desde el history state
    // El state se pasa cuando navegamos desde patient-detail o patients-list
    const historyState = window.history.state;
    if (historyState && historyState['returnUrl']) {
      this.returnUrl = historyState['returnUrl'];
    } else {
      // Si no hay state (navegación directa o refresh), determinar la ruta de retorno
      // Por defecto: si hay patientId, retornar al detalle del paciente
      // Si no hay patientId (crear nuevo), retornar a la lista
      this.returnUrl = this.patientId ? `/patients/${this.patientId}` : '/patients';
    }

    if (this.isEditMode && this.patientId) {
      // Dispatch acción para cargar paciente
      this.store.dispatch(PatientActions.loadPatient({ id: this.patientId }));
      
      // Suscribirse al selector para obtener el paciente cargado
      // Usar selectPatientById en lugar de selectSelectedPatient para obtener el paciente correcto
      this.store.select(selectPatientById(this.patientId))
        .pipe(
          filter(patient => patient !== null),
          take(1)
        )
        .subscribe(patient => {
          if (patient) {
            // Prellenar el formulario con los datos del paciente
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
    // Redirigir a la ruta de retorno determinada
    this.router.navigate([this.returnUrl]);
  }
}
