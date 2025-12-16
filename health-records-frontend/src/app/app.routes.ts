import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/patients',
    pathMatch: 'full'
  },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'patients',
    loadComponent: () => import('./features/patients/patients-list/patients-list.component').then(m => m.PatientsListComponent),
    canActivate: [authGuard]
  },
  {
    path: 'patients/new',
    loadComponent: () => import('./features/patients/patient-form/patient-form.component').then(m => m.PatientFormComponent),
    canActivate: [authGuard]
  },
  {
    path: 'patients/:id',
    loadComponent: () => import('./features/patients/patient-detail/patient-detail.component').then(m => m.PatientDetailComponent),
    canActivate: [authGuard]
  },
  {
    path: 'patients/:id/edit',
    loadComponent: () => import('./features/patients/patient-form/patient-form.component').then(m => m.PatientFormComponent),
    canActivate: [authGuard]
  },
  {
    path: 'records/new',
    loadComponent: () => import('./features/medical-records/record-form/record-form.component').then(m => m.RecordFormComponent),
    canActivate: [authGuard]
  },
  {
    path: 'records/:id',
    loadComponent: () => import('./features/medical-records/record-detail/record-detail.component').then(m => m.RecordDetailComponent),
    canActivate: [authGuard]
  },
  {
    path: 'records/:id/edit',
    loadComponent: () => import('./features/medical-records/record-form/record-form.component').then(m => m.RecordFormComponent),
    canActivate: [authGuard]
  },
  {
    path: 'medical-records',
    loadComponent: () => import('./features/medical-records/medical-records-list/medical-records-list.component').then(m => m.MedicalRecordsListComponent),
    canActivate: [authGuard]
  },
  {
    path: '**',
    redirectTo: '/patients'
  }
];
