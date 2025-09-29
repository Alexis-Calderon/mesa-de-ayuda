import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TicketService } from '../../../core/services/ticket.service';
import { AuthService } from '../../../core/services/auth.service';
import { Ticket } from '../../../core/models/ticket.models';
import { Usuario } from '../../../core/models/auth.models';

@Component({
  selector: 'app-ticket-list',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './ticket-list.html',
  styleUrl: './ticket-list.css'
})
export class TicketList implements OnInit {
  private ticketService = inject(TicketService);
  private authService = inject(AuthService);
  private fb = inject(FormBuilder);

  tickets: Ticket[] = [];
  filteredTickets: Ticket[] = [];
  isLoading = false;
  currentUser: Usuario | null = null;

  filterForm: FormGroup;

  // Opciones para filtros
  statusOptions = [
    { value: '', label: 'Todos los estados' },
    { value: 'Abierto', label: 'Abierto' },
    { value: 'Revisión', label: 'En Revisión' },
    { value: 'Resuelto', label: 'Resuelto' }
  ];

  priorityOptions = [
    { value: '', label: 'Todas las prioridades' },
    { value: 'Baja', label: 'Baja' },
    { value: 'Media', label: 'Media' },
    { value: 'Alta', label: 'Alta' }
  ];

  areaOptions = [
    { value: '', label: 'Todas las áreas' },
    { value: 'Calidad', label: 'Calidad' },
    { value: 'Desarrollo', label: 'Desarrollo' },
    { value: 'Finanzas', label: 'Finanzas' },
    { value: 'Logística', label: 'Logística' },
    { value: 'Mantención', label: 'Mantención' },
    { value: 'Producción', label: 'Producción' },
    { value: 'RRHH', label: 'RRHH' },
    { value: 'Ventas', label: 'Ventas' }
  ];

  constructor() {
    this.filterForm = this.fb.group({
      estado: [''],
      prioridad: [''],
      area: [''],
      search: ['']
    });
  }

  ngOnInit(): void {
    this.currentUser = this.authService.getCurrentUser();
    this.loadTickets();

    // Suscribirse a cambios en el formulario de filtros
    this.filterForm.valueChanges.subscribe(() => {
      this.applyFilters();
    });
  }

  loadTickets(): void {
    this.isLoading = true;
    this.ticketService.getTickets().subscribe({
      next: (tickets) => {
        this.tickets = tickets;
        this.applyFilters();
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading tickets:', error);
        this.isLoading = false;
      }
    });
  }

  applyFilters(): void {
    const filters = this.filterForm.value;
    this.filteredTickets = this.tickets.filter(ticket => {
      // Filtro por estado
      if (filters.estado && ticket.estado !== filters.estado) {
        return false;
      }

      // Filtro por prioridad
      if (filters.prioridad && ticket.prioridad !== filters.prioridad) {
        return false;
      }

      // Filtro por área
      if (filters.area && ticket.area !== filters.area) {
        return false;
      }

      // Filtro por búsqueda (descripción u observaciones)
      if (filters.search) {
        const searchTerm = filters.search.toLowerCase();
        const matchesDescription = ticket.descripcion.toLowerCase().includes(searchTerm);
        const matchesObservations = ticket.observaciones.toLowerCase().includes(searchTerm);
        if (!matchesDescription && !matchesObservations) {
          return false;
        }
      }

      return true;
    });
  }

  clearFilters(): void {
    this.filterForm.reset();
  }

  canEditTicket(ticket: Ticket): boolean {
    if (!this.currentUser) return false;

    // Administradores pueden editar todos
    if (this.currentUser.rol === 1) return true;

    // Técnicos pueden editar tickets asignados
    if (this.currentUser.rol === 2 && ticket.usuarioRutTecnicoNavigation?.rut === this.currentUser.rut) {
      return true;
    }

    return false;
  }

  canResolveTicket(ticket: Ticket): boolean {
    if (!this.currentUser) return false;

    // Administradores pueden resolver todos
    if (this.currentUser.rol === 1) return true;

    // Técnicos pueden resolver tickets asignados
    if (this.currentUser.rol === 2 && ticket.usuarioRutTecnicoNavigation?.rut === this.currentUser.rut) {
      return ticket.estado !== 'Resuelto';
    }

    return false;
  }

  assignTecnico(ticket: Ticket): void {
    if (!this.currentUser || this.currentUser.rol !== 1) return;

    const rutTecnico = prompt('Ingrese el RUT del técnico:');
    if (!rutTecnico) return;

    this.ticketService.assignTecnico(ticket.id, rutTecnico).subscribe({
      next: () => {
        this.loadTickets();
      },
      error: (error) => {
        console.error('Error assigning técnico:', error);
        alert('Error al asignar técnico');
      }
    });
  }

  resolveTicket(ticket: Ticket): void {
    if (!this.canResolveTicket(ticket)) return;

    if (confirm('¿Está seguro de que desea marcar este ticket como resuelto?')) {
      this.ticketService.resolveTicket(ticket.id).subscribe({
        next: () => {
          this.loadTickets();
        },
        error: (error) => {
          console.error('Error resolving ticket:', error);
          alert('Error al resolver el ticket');
        }
      });
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

  getPriorityColor(priority: string): string {
    switch (priority) {
      case 'Alta': return 'priority-high';
      case 'Media': return 'priority-medium';
      case 'Baja': return 'priority-low';
      default: return '';
    }
  }

  getAreaDisplayName(area: string): string {
    return area || 'Sin área';
  }
}
