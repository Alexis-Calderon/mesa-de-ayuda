export interface Ticket {
  id: number;
  tipo: string;
  prioridad: string;
  area: string;
  estado: string;
  descripcion: string;
  observaciones: string;
  fechaCreacion: string;
  fechaResolucion?: string;
  usuarioRutCreadorNavigation: {
    rut: string;
    nombre: string;
  };
  usuarioRutTecnicoNavigation?: {
    rut: string;
    nombre: string;
  };
}

export interface TicketCreate {
  tipo: string;
  prioridad: string;
  area: string;
  descripcion: string;
  observaciones: string;
}

export interface TicketUpdate {
  estado: string;
  observaciones: string;
}

export interface DashboardStats {
  totalTickets: number;
  ticketsAbiertos: number;
  ticketsRevision: number;
  ticketsResueltos: number;
  ticketsAsignados: number;
}
