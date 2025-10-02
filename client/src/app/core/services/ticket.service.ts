import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Ticket, TicketCreate, TicketUpdate, DashboardStats } from '../models/ticket.models';

@Injectable({
  providedIn: 'root'
})
export class TicketService {
  private readonly API_URL = 'api/api/tickets';

  constructor(private http: HttpClient) {}

  getTickets(filters?: any): Observable<Ticket[]> {
    let params = new HttpParams();

    if (filters) {
      Object.keys(filters).forEach(key => {
        if (filters[key] !== null && filters[key] !== undefined && filters[key] !== '') {
          params = params.set(key, filters[key]);
        }
      });
    }

    return this.http.get<Ticket[]>(this.API_URL, { params });
  }

  getTicket(id: number): Observable<Ticket> {
    return this.http.get<Ticket>(`${this.API_URL}/${id}`);
  }

  createTicket(ticket: TicketCreate): Observable<Ticket> {
    return this.http.post<Ticket>(this.API_URL, ticket);
  }

  updateTicket(id: number, ticket: TicketUpdate): Observable<Ticket> {
    return this.http.put<Ticket>(`${this.API_URL}/${id}`, ticket);
  }

  assignTecnico(id: number, rutTecnico: string): Observable<Ticket> {
    return this.http.put<Ticket>(`${this.API_URL}/${id}/asignar`, { rutTecnico });
  }

  resolveTicket(id: number): Observable<Ticket> {
    return this.http.put<Ticket>(`${this.API_URL}/${id}/resolver`, {});
  }

  getDashboardStats(): Observable<DashboardStats> {
    // Este endpoint no existe en el backend, calculamos estadísticas en el frontend
    return this.http.get<Ticket[]>(this.API_URL).pipe(
      // Aquí podríamos usar map para calcular estadísticas
    ) as any; // Temporal, implementar lógica de cálculo
  }
}
