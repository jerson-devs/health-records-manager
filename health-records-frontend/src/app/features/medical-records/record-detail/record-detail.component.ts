import { Component, OnInit, inject } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { ActivatedRoute, Router, RouterModule, NavigationExtras } from '@angular/router';
import { MedicalRecordService } from '../../../core/services/medical-record.service';
import { MedicalRecord } from '../../../core/models/medical-record.models';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';

@Component({
  selector: 'app-record-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    DatePipe
  ],
  templateUrl: './record-detail.component.html',
  styleUrl: './record-detail.component.scss'
})
export class RecordDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private medicalRecordService = inject(MedicalRecordService);

  record: MedicalRecord | null = null;
  loading = true;
  recordId!: number;
  returnUrl: string | null = null;

  ngOnInit(): void {
    this.recordId = +this.route.snapshot.params['id'];
    // Obtener returnUrl del state de navegación (history.state persiste después de la navegación)
    this.returnUrl = (history.state && history.state['returnUrl']) || null;
    this.loadRecord();
  }

  loadRecord(): void {
    this.loading = true;
    this.medicalRecordService.getById(this.recordId).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.record = response.data;
        }
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  onEdit(): void {
    const navigationExtras: NavigationExtras = {};
    // Si hay returnUrl, pasarlo al formulario de edición
    if (this.returnUrl) {
      navigationExtras.state = { returnUrl: this.returnUrl };
    }
    this.router.navigate(['/records', this.recordId, 'edit'], navigationExtras);
  }

  onBack(): void {
    // Si hay returnUrl (viene de /medical-records), redirigir ahí
    if (this.returnUrl) {
      this.router.navigate([this.returnUrl]);
    } else if (this.record?.patientId) {
      // Si no hay returnUrl pero hay patientId, redirigir al paciente
      this.router.navigate(['/patients', this.record.patientId]);
    } else {
      // Por defecto, redirigir a pacientes
      this.router.navigate(['/patients']);
    }
  }
}

