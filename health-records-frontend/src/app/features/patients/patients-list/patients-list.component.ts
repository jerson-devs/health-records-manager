import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatChipsModule } from '@angular/material/chips';
import { Patient } from '../../../core/models/patient.models';
import { selectAllPatients, selectPatientsLoading, selectPatientsError } from '../../../core/store/patients/patients.selectors';
import * as PatientActions from '../../../core/store/patients/patients.actions';

@Component({
  selector: 'app-patients-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatInputModule,
    MatFormFieldModule,
    MatChipsModule
  ],
  templateUrl: './patients-list.component.html',
  styleUrl: './patients-list.component.scss'
})
export class PatientsListComponent implements OnInit {
  private store = inject(Store);
  private router = inject(Router);

  patients$: Observable<Patient[]> = this.store.select(selectAllPatients);
  loading$: Observable<boolean> = this.store.select(selectPatientsLoading);
  error$: Observable<string | null> = this.store.select(selectPatientsError);

  displayedColumns: string[] = ['nombre', 'email', 'documento', 'fechaNacimiento', 'actions'];
  searchTerm = '';

  ngOnInit(): void {
    this.store.dispatch(PatientActions.loadPatients());
  }

  onAddPatient(): void {
    this.router.navigate(['/patients/new']);
  }

  onViewPatient(id: number): void {
    this.router.navigate(['/patients', id]);
  }

  onEditPatient(id: number): void {
    this.router.navigate(['/patients', id, 'edit']);
  }

  onDeletePatient(id: number): void {
    if (confirm('¿Está seguro de que desea eliminar este paciente?')) {
      this.store.dispatch(PatientActions.deletePatient({ id }));
    }
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




