import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, tap } from 'rxjs';
import { ProfileRequest, ProfileResponse } from '../models/profile.models';
import { Usuario } from '../models/auth.models';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  private readonly API_URL = 'api/api/perfil';
  private currentProfileSubject = new BehaviorSubject<Usuario | null>(null);
  public currentProfile$ = this.currentProfileSubject.asObservable();

  constructor(private http: HttpClient) {
    // Cargar perfil inicial si hay usuario autenticado
    this.loadCurrentProfile();
  }

  getProfile(): Observable<ProfileResponse> {
    return this.http.get<ProfileResponse>(this.API_URL);
  }

  updateProfile(profileData: ProfileRequest): Observable<ProfileResponse> {
    return this.http.put<ProfileResponse>(this.API_URL, profileData)
      .pipe(
        tap(updatedProfile => {
          // Actualizar el perfil local
          this.currentProfileSubject.next({
            rut: updatedProfile.rut,
            nombre: updatedProfile.nombre,
            email: updatedProfile.email,
            rol: updatedProfile.rol,
            fechaCreacion: updatedProfile.fechaCreacion
          });
        })
      );
  }

  private loadCurrentProfile(): void {
    this.getProfile().subscribe({
      next: (profile) => {
        this.currentProfileSubject.next({
          rut: profile.rut,
          nombre: profile.nombre,
          email: profile.email,
          rol: profile.rol,
          fechaCreacion: profile.fechaCreacion
        });
      },
      error: (error) => {
        console.error('Error al cargar perfil:', error);
      }
    });
  }

  refreshProfile(): void {
    this.loadCurrentProfile();
  }
}
