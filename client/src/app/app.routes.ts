import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { RoleGuard } from './core/guards/role.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/dashboard',
    pathMatch: 'full'
  },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login').then(m => m.Login)
    // No AuthGuard aquí - página pública
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./features/dashboard/dashboard/dashboard').then(m => m.Dashboard),
    canActivate: [AuthGuard]
  },
  {
    path: 'tickets',
    loadComponent: () => import('./features/tickets/ticket-list/ticket-list').then(m => m.TicketList),
    canActivate: [AuthGuard]
  },
  {
    path: 'tickets/create',
    loadComponent: () => import('./features/tickets/ticket-create/ticket-create').then(m => m.TicketCreate),
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: ['Cliente'] } // Cliente
  },
  {
    path: 'users',
    loadComponent: () => import('./features/users/user-management/user-management').then(m => m.UserManagement),
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: ['Administrador'] } // Administrador
  },
  {
    path: 'profile',
    loadComponent: () => import('./features/profile/profile-edit/profile-edit').then(m => m.ProfileEdit),
    canActivate: [AuthGuard] // Cualquier usuario autenticado puede ver su perfil
  },
  {
    path: '**',
    redirectTo: '/dashboard'
  }
];
