import { Component, OnInit, inject } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
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

  ngOnInit(): void {
    this.recordId = +this.route.snapshot.params['id'];
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
    this.router.navigate(['/records', this.recordId, 'edit']);
  }

  onBack(): void {
    if (this.record?.patientId) {
      this.router.navigate(['/patients', this.record.patientId]);
    } else {
      this.router.navigate(['/patients']);
    }
  }
}

