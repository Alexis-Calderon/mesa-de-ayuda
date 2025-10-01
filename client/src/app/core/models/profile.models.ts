export interface ProfileRequest {
  nombre: string;
  email: string;
  currentContrasenia?: string;
  newContrasenia?: string;
  confirmNewContrasenia?: string;
}

export interface ProfileResponse {
  rut: string;
  nombre: string;
  email: string;
  rol: string;
  fechaCreacion?: string;
}