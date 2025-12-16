import { Component, OnInit, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Store } from '@ngrx/store';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from './shared/components/navbar/navbar.component';
import * as AuthActions from './core/store/auth/auth.actions';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, NavbarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  private store = inject(Store);

  ngOnInit(): void {
    // Verificar autenticación al iniciar la app
    // El effect checkAuth manejará la validación de tokens y actualización del estado
    this.store.dispatch(AuthActions.checkAuth());
  }
}
