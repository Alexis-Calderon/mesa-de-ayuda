import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { UserService, UsuarioCreate, UsuarioUpdate } from '../../../core/services/user.service';
import { Usuario } from '../../../core/models/auth.models';

@Component({
  selector: 'app-user-management',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './user-management.html',
  styleUrl: './user-management.css'
})
export class UserManagement implements OnInit {
  private userService = inject(UserService);
  private fb = inject(FormBuilder);

  users: Usuario[] = [];
  isLoading = false;
  showCreateForm = false;
  editingUser: Usuario | null = null;

  createForm: FormGroup;
  editForm: FormGroup;

  // Mapa de roles para conversión
  roleMap = {
    'Cliente': 3,
    'Técnico': 2
  };

  reverseRoleMap = {
    3: 'Cliente',
    2: 'Técnico',
    1: 'Administrador'
  };

  constructor() {
    this.createForm = this.fb.group({
      rut: ['', [Validators.required, Validators.pattern(/^\d{7,8}-[\dkK]$/)]],
      nombre: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      rol: [3, [Validators.required]], // Cliente por defecto
      contrasenia: ['', [Validators.required, Validators.minLength(6)]]
    });

    this.editForm = this.fb.group({
      nombre: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      rol: [3, [Validators.required]]
    });
  }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.isLoading = true;
    this.userService.getUsers().subscribe({
      next: (users) => {
        this.users = users;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading users:', error);
        this.isLoading = false;
      }
    });
  }

  toggleCreateForm(): void {
    this.showCreateForm = !this.showCreateForm;
    this.editingUser = null;
    if (!this.showCreateForm) {
      this.createForm.reset();
    }
  }

  onCreateUser(): void {
    if (this.createForm.valid) {
      this.isLoading = true;
      const userData: UsuarioCreate = this.createForm.value;

      this.userService.createUser(userData).subscribe({
        next: () => {
          this.loadUsers();
          this.toggleCreateForm();
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error creating user:', error);
          this.isLoading = false;
        }
      });
    } else {
      this.markFormGroupTouched(this.createForm);
    }
  }

  startEditUser(user: Usuario): void {
    this.editingUser = user;
    this.showCreateForm = false;
    this.editForm.patchValue({
      nombre: user.nombre,
      email: user.email,
      rol: user.rol // Ya viene como número del backend
    });
  }

  cancelEdit(): void {
    this.editingUser = null;
    this.editForm.reset();
  }

  onUpdateUser(): void {
    if (this.editForm.valid && this.editingUser) {
      this.isLoading = true;
      const userData: UsuarioUpdate = this.editForm.value;

      this.userService.updateUser(this.editingUser.rut, userData).subscribe({
        next: () => {
          this.loadUsers();
          this.cancelEdit();
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error updating user:', error);
          this.isLoading = false;
        }
      });
    } else {
      this.markFormGroupTouched(this.editForm);
    }
  }

  deleteUser(user: Usuario): void {
    if (confirm(`¿Estás seguro de que quieres eliminar al usuario ${user.nombre}?`)) {
      this.isLoading = true;
      this.userService.deleteUser(user.rut).subscribe({
        next: () => {
          this.loadUsers();
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error deleting user:', error);
          this.isLoading = false;
        }
      });
    }
  }

  private markFormGroupTouched(form: FormGroup): void {
    Object.keys(form.controls).forEach(key => {
      const control = form.get(key);
      control?.markAsTouched();
    });
  }

  getRoleDisplayName(role: number): string {
    return this.reverseRoleMap[role as keyof typeof this.reverseRoleMap] || 'Desconocido';
  }

  getRoleColor(role: number): string {
    switch (role) {
      case 1: return 'role-admin';
      case 2: return 'role-tech';
      case 3: return 'role-client';
      default: return '';
    }
  }
}
