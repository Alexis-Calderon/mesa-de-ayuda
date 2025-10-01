import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ProfileService } from '../../../core/services/profile.service';
import { AuthService } from '../../../core/services/auth.service';
import { ProfileRequest } from '../../../core/models/profile.models';
import { Usuario } from '../../../core/models/auth.models';

@Component({
  selector: 'app-profile-edit',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './profile-edit.html',
  styleUrls: ['./profile-edit.css']
})
export class ProfileEdit implements OnInit {
  profileForm!: FormGroup;
  currentUser: Usuario | null = null;
  isLoading = false;
  showPasswordFields = false;
  successMessage = '';
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private profileService: ProfileService,
    private authService: AuthService,
    private router: Router
  ) {
    this.initializeForm();
  }

  ngOnInit(): void {
    this.loadCurrentUser();
    this.loadCurrentProfile();
  }

  private initializeForm(): void {
    this.profileForm = this.fb.group({
      nombre: ['', [
        Validators.required,
        Validators.maxLength(100)
      ]],
      email: ['', [
        Validators.required,
        Validators.email,
        Validators.maxLength(100),
        this.emailValidator()
      ]],
      currentContrasenia: ['', [
        Validators.minLength(8)
      ]],
      newContrasenia: ['', [
        Validators.minLength(8),
        this.passwordValidator()
      ]],
      confirmNewContrasenia: ['', [
        Validators.minLength(8)
      ]]
    }, { validators: this.passwordMatchValidator });
  }

  private loadCurrentUser(): void {
    this.currentUser = this.authService.getCurrentUser();
  }

  private loadCurrentProfile(): void {
    this.isLoading = true;
    this.profileService.getProfile().subscribe({
      next: (profile) => {
        this.profileForm.patchValue({
          nombre: profile.nombre,
          email: profile.email
        });
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error al cargar perfil:', error);
        this.errorMessage = 'Error al cargar el perfil. Inténtalo nuevamente.';
        this.isLoading = false;
      }
    });
  }

  togglePasswordFields(): void {
    this.showPasswordFields = !this.showPasswordFields;
    if (!this.showPasswordFields) {
      this.profileForm.patchValue({
        currentContrasenia: '',
        newContrasenia: '',
        confirmNewContrasenia: ''
      });
      // Limpiar errores de validación cuando se ocultan los campos
      this.profileForm.get('currentContrasenia')?.markAsUntouched();
      this.profileForm.get('newContrasenia')?.markAsUntouched();
      this.profileForm.get('confirmNewContrasenia')?.markAsUntouched();
    }
  }

  passwordMatchValidator(control: AbstractControl): { [key: string]: any } | null {
    const newPassword = control.get('newContrasenia');
    const confirmPassword = control.get('confirmNewContrasenia');

    if (newPassword && confirmPassword && newPassword.value !== confirmPassword.value) {
      return { 'passwordMismatch': true };
    }
    return null;
  }

  onSubmit(): void {
    // Validar campos básicos primero
    const nombreField = this.profileForm.get('nombre');
    const emailField = this.profileForm.get('email');

    if (nombreField?.invalid && nombreField?.touched) {
      this.errorMessage = 'Por favor corrija los errores en el formulario.';
      return;
    }

    if (emailField?.invalid && emailField?.touched) {
      this.errorMessage = 'Por favor corrija los errores en el formulario.';
      return;
    }

    // Si se están cambiando contraseñas, validar campos de contraseña
    if (this.showPasswordFields) {
      const currentPasswordField = this.profileForm.get('currentContrasenia');
      const newPasswordField = this.profileForm.get('newContrasenia');
      const confirmPasswordField = this.profileForm.get('confirmNewContrasenia');

      if (currentPasswordField?.invalid && currentPasswordField?.touched) {
        this.errorMessage = 'Por favor corrija los errores en el formulario.';
        return;
      }

      if (newPasswordField?.invalid && newPasswordField?.touched) {
        this.errorMessage = 'Por favor corrija los errores en el formulario.';
        return;
      }

      if (confirmPasswordField?.invalid && confirmPasswordField?.touched) {
        this.errorMessage = 'Por favor corrija los errores en el formulario.';
        return;
      }

      // Verificar que todos los campos de contraseña estén llenos
      const formValue = this.profileForm.value;
      if (!formValue.currentContrasenia || !formValue.newContrasenia || !formValue.confirmNewContrasenia) {
        this.errorMessage = 'Para cambiar la contraseña debe completar todos los campos de contraseña.';
        return;
      }
    }

    this.isLoading = true;
    this.successMessage = '';
    this.errorMessage = '';

    const formValue = this.profileForm.value;
    const profileData: ProfileRequest = {
      nombre: formValue.nombre,
      email: formValue.email
    };

    // Solo incluir campos de contraseña si se están cambiando
    if (this.showPasswordFields && formValue.currentContrasenia) {
      profileData.currentContrasenia = formValue.currentContrasenia;
      profileData.newContrasenia = formValue.newContrasenia;
      profileData.confirmNewContrasenia = formValue.confirmNewContrasenia;
    }

    this.profileService.updateProfile(profileData).subscribe({
      next: (updatedProfile) => {
        this.successMessage = 'Perfil actualizado exitosamente';
        this.isLoading = false;

        // Actualizar información del usuario en AuthService si es necesario
        if (this.currentUser) {
          this.currentUser.nombre = updatedProfile.nombre;
          this.currentUser.email = updatedProfile.email;
        }

        // Ocultar campos de contraseña después de actualizar
        if (this.showPasswordFields) {
          setTimeout(() => {
            this.showPasswordFields = false;
            this.togglePasswordFields();
          }, 2000);
        }
      },
      error: (error) => {
        console.error('Error al actualizar perfil:', error);
        if (error.status === 400) {
          this.errorMessage = error.error || 'Error en los datos proporcionados';
        } else {
          this.errorMessage = 'Error al actualizar el perfil. Inténtalo nuevamente.';
        }
        this.isLoading = false;
      }
    });
  }

  private markFormGroupTouched(): void {
    Object.keys(this.profileForm.controls).forEach(key => {
      const control = this.profileForm.get(key);
      control?.markAsTouched();
    });
  }

  getFieldError(fieldName: string): string {
    const field = this.profileForm.get(fieldName);
    if (field && field.errors && field.touched) {
      // Validaciones básicas
      if (field.errors['required']) {
        switch (fieldName) {
          case 'nombre': return 'El nombre es requerido.';
          case 'email': return 'El email es requerido.';
          default: return `${fieldName} es requerido`;
        }
      }

      if (field.errors['email']) return 'El email no es válido.';
      if (field.errors['minlength']) {
        switch (fieldName) {
          case 'currentContrasenia': return 'La contraseña actual debe tener al menos 8 caracteres.';
          case 'newContrasenia': return 'La nueva contraseña debe tener al menos 8 caracteres.';
          case 'confirmNewContrasenia': return 'La confirmación de la nueva contraseña debe tener al menos 8 caracteres.';
          default: return `${fieldName} debe tener al menos ${field.errors['minlength'].requiredLength} caracteres.`;
        }
      }

      if (field.errors['maxlength']) {
        switch (fieldName) {
          case 'nombre': return 'El nombre no puede exceder los 100 caracteres.';
          case 'email': return 'El email no puede exceder los 100 caracteres.';
          default: return `${fieldName} no puede exceder ${field.errors['maxlength'].requiredLength} caracteres`;
        }
      }

      // Validaciones personalizadas
      if (field.errors['emailInvalido']) return field.errors['emailInvalido'];
      if (field.errors['passwordInvalido']) return field.errors['passwordInvalido'];
      if (field.errors['passwordMismatch']) return 'Las contraseñas no coinciden';
    }
    return '';
  }

  canEditProfile(): boolean {
    return this.authService.isAuthenticated();
  }

  getRoleName(rol: string): string {
    switch (rol) {
      case 'Administrador': return 'Administrador';
      case 'Técnico': return 'Técnico';
      case 'Cliente': return 'Cliente';
      default: return 'Desconocido';
    }
  }

  // Validador personalizado para email
  emailValidator() {
    return (control: AbstractControl): { [key: string]: any } | null => {
      const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
      const value = control.value;

      if (!value) {
        return null; // El campo es requerido, se maneja con Validators.required
      }

      if (!emailRegex.test(value)) {
        return { 'emailInvalido': 'El email no es válido.' };
      }

      return null;
    };
  }

  // Validador personalizado para contraseña
  passwordValidator() {
    return (control: AbstractControl): { [key: string]: any } | null => {
      const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
      const value = control.value;

      if (!value) {
        return null; // El campo es opcional hasta que se active el cambio de contraseña
      }

      if (!passwordRegex.test(value)) {
        return {
          'passwordInvalido': 'La contraseña debe contener al menos una letra mayúscula, una letra minúscula, un número y un carácter especial.'
        };
      }

      return null;
    };
  }

  goBack(): void {
    this.router.navigate(['/dashboard']);
  }
}