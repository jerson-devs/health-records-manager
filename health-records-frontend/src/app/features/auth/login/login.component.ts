import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule, MatSpinner } from '@angular/material/progress-spinner';
import * as AuthActions from '../../../core/store/auth/auth.actions';
import { selectAuthLoading, selectAuthError, selectIsAuthenticated } from '../../../core/store/auth/auth.selectors';

/**
 * Componente de login
 * Siguiendo patrón Redux:
 * - Dispatcha acción login con credenciales
 * - Se suscribe a selectores para obtener estado de loading y errores
 * - NO hace llamadas HTTP directamente
 */
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSpinner
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  private fb = inject(FormBuilder);
  private store = inject(Store);
  private router = inject(Router);

  loginForm: FormGroup;
  loading$: Observable<boolean> = this.store.select(selectAuthLoading);
  error$: Observable<string | null> = this.store.select(selectAuthError);
  isAuthenticated$: Observable<boolean> = this.store.select(selectIsAuthenticated);

  hidePassword = true;

  constructor() {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  ngOnInit(): void {
    // Si ya está autenticado, redirigir a pacientes
    this.isAuthenticated$.subscribe(isAuth => {
      if (isAuth) {
        this.router.navigate(['/patients']);
      }
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      const { username, password } = this.loginForm.value;
      // Dispatch acción login - el Effect manejará la llamada HTTP
      this.store.dispatch(AuthActions.login({ username, password }));
    }
  }

  togglePasswordVisibility(): void {
    this.hidePassword = !this.hidePassword;
  }
}
