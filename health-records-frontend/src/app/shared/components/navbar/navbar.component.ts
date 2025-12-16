import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatDividerModule } from '@angular/material/divider';
import { selectUser, selectIsAuthenticated } from '../../../core/store/auth/auth.selectors';
import { UserInfo } from '../../../core/models/auth.models';
import * as AuthActions from '../../../core/store/auth/auth.actions';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatDividerModule
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent implements OnInit {
  private store = inject(Store);
  private router = inject(Router);

  user$: Observable<UserInfo | null> = this.store.select(selectUser);
  isAuthenticated$: Observable<boolean> = this.store.select(selectIsAuthenticated);

  ngOnInit(): void {}

  logout(): void {
    this.store.dispatch(AuthActions.logout());
  }
}

