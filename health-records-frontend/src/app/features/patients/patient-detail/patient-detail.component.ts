import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable, Subject, combineLatest } from 'rxjs';
import { filter, take, takeUntil, map, startWith } from 'rxjs/operators';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Patient } from '../../../core/models/patient.models';
import { MedicalRecord } from '../../../core/models/medical-record.models';
import { selectPatientById, selectPatientsLoading } from '../../../core/store/patients/patients.selectors';
import { selectRecordsByPatientId, selectRecordsLoading, selectAllRecords } from '../../../core/store/medical-records/medical-records.selectors';
import * as PatientActions from '../../../core/store/patients/patients.actions';
import * as MedicalRecordActions from '../../../core/store/medical-records/medical-records.actions';
import { DatePipe } from '@angular/common';

/**
 * Componente de detalle de paciente
 * Siguiendo patrón Redux:
 * - Dispatcha acciones para cargar datos
 * - Se suscribe a selectores para obtener datos
 * - NO hace llamadas HTTP directamente
 */
@Component({
  selector: 'app-patient-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatTableModule,
    MatProgressSpinnerModule,
    DatePipe
  ],
  templateUrl: './patient-detail.component.html',
  styleUrl: './patient-detail.component.scss'
})
export class PatientDetailComponent implements OnInit, OnDestroy {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private store = inject(Store);
  private destroy$ = new Subject<void>();

  patient$!: Observable<Patient | null>;
  medicalRecords$!: Observable<MedicalRecord[]>;
  loading$: Observable<boolean>;
  recordsLoading$: Observable<boolean>;
  patientId!: number;

  displayedColumns: string[] = ['fecha', 'diagnostico', 'medico', 'actions'];

  constructor() {
    this.loading$ = this.store.select(selectPatientsLoading);
    this.recordsLoading$ = this.store.select(selectRecordsLoading);
  }

  ngOnInit(): void {
    // Obtener el patientId inicial de la ruta
    this.patientId = +this.route.snapshot.params['id'];
    
    // Cargar paciente usando selector por ID
    this.patient$ = this.store.select(selectPatientById(this.patientId));
    
    // Dispatch acción para cargar paciente
    this.store.dispatch(PatientActions.loadPatient({ id: this.patientId }));
    
    // Dispatch acción para cargar historiales del paciente
    this.store.dispatch(MedicalRecordActions.loadRecordsByPatient({ patientId: this.patientId }));
    
    // Crear observable reactivo que combine los records con el patientId de la ruta
    // Esto se actualiza automáticamente cuando cambian los records o el patientId
    const allRecords$ = this.store.select(selectAllRecords);
    const patientIdFromRoute$ = this.route.params.pipe(
      map(params => +params['id']),
      startWith(this.patientId)
    );

    this.medicalRecords$ = combineLatest([
      allRecords$.pipe(startWith([])),
      patientIdFromRoute$
    ]).pipe(
      map(([records, patientId]) => {
        if (!Array.isArray(records) || !patientId) {
          return [];
        }
        return records.filter(record => record && record.patientId === patientId);
      })
    );

    // Suscribirse a cambios en los parámetros de ruta para recargar cuando cambie el patientId
    this.route.params
      .pipe(takeUntil(this.destroy$))
      .subscribe(params => {
        const newPatientId = +params['id'];
        
        // Solo recargar si el patientId cambió
        if (this.patientId !== newPatientId) {
          this.patientId = newPatientId;
          
          // Actualizar selector de paciente
          this.patient$ = this.store.select(selectPatientById(this.patientId));
          
          // Dispatch acción para cargar paciente
          this.store.dispatch(PatientActions.loadPatient({ id: this.patientId }));
          
          // Dispatch acción para cargar historiales del paciente
          this.store.dispatch(MedicalRecordActions.loadRecordsByPatient({ patientId: this.patientId }));
        }
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onEdit(): void {
    this.router.navigate(['/patients', this.patientId, 'edit']);
  }

  onAddRecord(): void {
    this.router.navigate(['/records/new'], {
      queryParams: { patientId: this.patientId }
    });
  }

  onViewRecord(id: number): void {
    this.router.navigate(['/records', id]);
  }

  getAge(fechaNacimiento: string): number {
    const today = new Date();
    const birthDate = new Date(fechaNacimiento);
    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }
    return age;
  }
}
