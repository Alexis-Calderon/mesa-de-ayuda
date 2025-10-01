export interface LoginRequest {
  rut: string;
  contrasenia: string;
}

export interface AuthResponse {
  token: string;
  usuario: {
    rut: string;
    nombre: string;
    email: string;
    rol: string;
  };
}

export interface Usuario {
  rut: string;
  nombre: string;
  email: string;
  rol: string;
  fechaCreacion?: string;
}
