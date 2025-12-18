import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { DatePipe } from '@angular/common';
import { MedicalRecord } from '../../../core/models/medical-record.models';
import { selectAllRecords, selectRecordsLoading, selectRecordsError } from '../../../core/store/medical-records/medical-records.selectors';
import * as MedicalRecordActions from '../../../core/store/medical-records/medical-records.actions';

/**
 * Componente para listar todos los historiales médicos
 * Siguiendo patrón Redux:
 * - Dispatcha acciones para cargar datos
 * - Se suscribe a selectores para obtener datos
 */
@Component({
  selector: 'app-medical-records-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatTooltipModule,
    DatePipe
  ],
  templateUrl: './medical-records-list.component.html',
  styleUrl: './medical-records-list.component.scss'
})
export class MedicalRecordsListComponent implements OnInit {
  private store = inject(Store);
  private router = inject(Router);

  records$: Observable<MedicalRecord[]> = this.store.select(selectAllRecords);
  loading$: Observable<boolean> = this.store.select(selectRecordsLoading);
  error$: Observable<string | null> = this.store.select(selectRecordsError);

  displayedColumns: string[] = ['fecha', 'paciente', 'diagnostico', 'medico', 'tratamiento', 'actions'];

  ngOnInit(): void {
    // Cargar todos los historiales médicos
    this.store.dispatch(MedicalRecordActions.loadRecords());
  }

  onViewRecord(id: number): void {
    this.router.navigate(['/records', id], {
      state: { returnUrl: '/medical-records' }
    });
  }

  onViewPatient(patientId: number): void {
    this.router.navigate(['/patients', patientId]);
  }

  onAddRecord(): void {
    this.router.navigate(['/records/new']);
  }
}
