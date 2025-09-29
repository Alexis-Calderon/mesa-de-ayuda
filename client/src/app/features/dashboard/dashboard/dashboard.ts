import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { TicketService } from '../../../core/services/ticket.service';
import { Usuario } from '../../../core/models/auth.models';
import { Ticket, DashboardStats } from '../../../core/models/ticket.models';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {
  private authService = inject(AuthService);
  private ticketService = inject(TicketService);

  currentUser: Usuario | null = null;
  stats: DashboardStats = {
    totalTickets: 0,
    ticketsAbiertos: 0,
    ticketsRevision: 0,
    ticketsResueltos: 0,
    ticketsAsignados: 0
  };
  recentTickets: Ticket[] = [];
  isLoading = true;

  ngOnInit(): void {
    this.currentUser = this.authService.getCurrentUser();
    this.loadDashboardData();
  }

  private loadDashboardData(): void {
    this.ticketService.getTickets().subscribe({
      next: (tickets) => {
        this.recentTickets = tickets.slice(0, 5); // Últimos 5 tickets
        this.calculateStats(tickets);
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading dashboard data:', error);
        this.isLoading = false;
      }
    });
  }

  private calculateStats(tickets: Ticket[]): void {
    this.stats.totalTickets = tickets.length;
    this.stats.ticketsAbiertos = tickets.filter(t => t.estado === 'Abierto').length;
    this.stats.ticketsRevision = tickets.filter(t => t.estado === 'Revisión').length;
    this.stats.ticketsResueltos = tickets.filter(t => t.estado === 'Resuelto').length;
    this.stats.ticketsAsignados = tickets.filter(t => t.usuarioRutTecnicoNavigation).length;
  }

  logout(): void {
    this.authService.logout();
  }

  getRoleDisplayName(role: number): string {
    switch (role) {
      case 1: return 'Administrador';
      case 2: return 'Técnico';
      case 3: return 'Cliente';
      default: return '';
    }
  }

  getStatusColor(status: string): string {
    switch (status) {
      case 'Abierto': return 'status-open';
      case 'Revisión': return 'status-review';
      case 'Resuelto': return 'status-resolved';
      default: return '';
    }
  }
}
