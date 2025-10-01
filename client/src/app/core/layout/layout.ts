import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, NavigationEnd } from '@angular/router';
import { Subject, filter, takeUntil } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { Usuario } from '../models/auth.models';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './layout.html',
  styleUrl: './layout.css',
})
export class Layout implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  currentUser: Usuario | null = null;
  currentRoute = '';
  showProfileButton = true;

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.loadCurrentUser();
    this.trackCurrentRoute();
    this.subscribeToUserChanges();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadCurrentUser(): void {
    this.currentUser = this.authService.getCurrentUser();
  }

  private subscribeToUserChanges(): void {
    this.authService.currentUser$
      .pipe(takeUntil(this.destroy$))
      .subscribe(user => {
        this.currentUser = user;
      });
  }

  private trackCurrentRoute(): void {
    // Obtener la ruta actual
    this.currentRoute = this.router.url;

    // Actualizar el estado del botón de perfil basado en la ruta actual
    this.updateProfileButtonVisibility();

    // Escuchar cambios de navegación
    this.router.events
      .pipe(
        filter((event) => event instanceof NavigationEnd),
        takeUntil(this.destroy$)
      )
      .subscribe((event: NavigationEnd) => {
        this.currentRoute = event.url;
        this.updateProfileButtonVisibility();
      });
  }

  private updateProfileButtonVisibility(): void {
    // Ocultar el botón de perfil cuando estemos en la página de perfil
    this.showProfileButton = !this.currentRoute.includes('/profile');
  }

  logout(): void {
    this.authService.logout();
  }

  isActiveRoute(route: string): boolean {
    return this.currentRoute === route || this.currentRoute.startsWith(route + '/');
  }

  isLoginPage(): boolean {
    return this.currentRoute === '/login';
  }

  getRoleDisplayName(role: string): string {
    switch (role) {
      case 'Administrador': return 'Administrador';
      case 'Técnico': return 'Técnico';
      case 'Cliente': return 'Cliente';
      default: return '';
    }
  }
}
