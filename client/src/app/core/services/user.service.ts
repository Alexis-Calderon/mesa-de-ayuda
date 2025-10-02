import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Usuario } from '../models/auth.models';

export interface UsuarioCreate {
  rut: string;
  nombre: string;
  email: string;
  rol: number;
  contrasenia: string;
}

export interface UsuarioUpdate {
  nombre: string;
  email: string;
  rol: number;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly API_URL = 'api/api/usuarios';

  constructor(private http: HttpClient) {}

  getUsers(): Observable<Usuario[]> {
    return this.http.get<Usuario[]>(this.API_URL);
  }

  getUser(rut: string): Observable<Usuario> {
    return this.http.get<Usuario>(`${this.API_URL}/${rut}`);
  }

  createUser(user: UsuarioCreate): Observable<Usuario> {
    return this.http.post<Usuario>(this.API_URL, user);
  }

  updateUser(rut: string, user: UsuarioUpdate): Observable<Usuario> {
    return this.http.put<Usuario>(`${this.API_URL}/${rut}`, user);
  }

  deleteUser(rut: string): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${rut}`);
  }
}
